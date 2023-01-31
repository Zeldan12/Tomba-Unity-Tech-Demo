using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaAirbornBaseState : TombaState {

    protected Vector3 _lastPosition;
    private float _acceleration, _maxRunSpeed, _decceleration;

    public TombaAirbornBaseState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.AirbornBase;
    }

    public override void OnEnter(TombaState previousState) {
        if (_tomba.IsDashing) {
            _acceleration = _tomba.DashAcceleration;
            _maxRunSpeed = _tomba.MaxDashSpeed;
            _decceleration = _tomba.DashDecceleration;
            _tomba.DashParticleSystem.Play();
        } else {
            _acceleration = _tomba.Acceleration;
            _maxRunSpeed = _tomba.MaxRunSpeed;
            _decceleration = _tomba.Decceleration;
        }

        _lastPosition = _tomba.transform.position;
    }

    public override void OnExit() {
        _tomba.DashParticleSystem.Stop();
    }

    public override TombaStateType Update() {
        
        //Conditions to accelerate
        if (((_tomba.HorizontalInput > 0 && _tomba.HorizontalSpeed >= 0) || (_tomba.HorizontalInput < 0 && _tomba.HorizontalSpeed <= 0))) {
            //Accelerate based on the _direction of the input
            _tomba.HorizontalSpeed = _tomba.HorizontalInput == 1 ?
                Mathf.Min(_tomba.HorizontalSpeed + ((_acceleration * Time.deltaTime)), _maxRunSpeed) :
                    Mathf.Max(_tomba.HorizontalSpeed - ((_acceleration * Time.deltaTime)), -_maxRunSpeed);

            //Conditions to decelerate 
        } else if (_tomba.HorizontalSpeed != 0) {

            //Decelerate  
            _tomba.HorizontalSpeed = _tomba.HorizontalSpeed > 0 ?
                Mathf.Max(_tomba.HorizontalSpeed - (_decceleration * Time.deltaTime), 0) :
                   Mathf.Min(_tomba.HorizontalSpeed + (_decceleration * Time.deltaTime), 0);
        }

        if (_tomba.HorizontalInput != 0) {
            _tomba.transform.rotation = Quaternion.Euler(_tomba.transform.rotation.x, (_tomba.HorizontalInput == 1) ? 0 : 180, _tomba.transform.rotation.z);
        }
        
        _tomba.RigidBody.velocity = new Vector2(_tomba.HorizontalSpeed, _tomba.RigidBody.velocity.y);

        if (_tomba.Grounded && _tomba.RigidBody.velocity.y <= 0) {
            _tomba.IsDashing = false;
            return TombaGroundedBaseState.FindBestGroundedState(_tomba);
        }
        if (_tomba.CheckWall()) {
            _tomba.IsDashing = false;
            return TombaOnWallBaseState.FindBestOnWallState(_tomba);
        }

        return TombaStateType.None;
    }

    public override void CameraBehaviour(CameraController cameraController) {
        Transform player = _tomba.transform;
        Transform transform = cameraController.transform;
        Camera camera = cameraController.Camera;

        float xMovement = player.position.x - _lastPosition.x;
        _lastPosition = player.position;
        float distance = Mathf.Abs(transform.position.z - player.position.z);
        var frustumHeight = (2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad)) / 2;
        float yMovement = transform.position.y;

        if (player.position.y > transform.position.y + frustumHeight - cameraController.HeigthOffset) {
            yMovement = transform.position.y + Mathf.Abs((player.position.y - (transform.position.y + frustumHeight - cameraController.HeigthOffset)));
        } else if (player.position.y < transform.position.y - frustumHeight + cameraController.HeigthOffset) {
            yMovement = transform.position.y - Mathf.Abs((player.position.y - (transform.position.y - frustumHeight + cameraController.HeigthOffset)));
        }
        transform.position = new Vector3(transform.position.x + xMovement, yMovement, transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w), cameraController.Movespeed);
    }
}
