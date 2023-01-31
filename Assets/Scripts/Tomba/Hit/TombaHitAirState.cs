using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaHitAirState : TombaState {

    private Vector3 _lastPosition;

    public TombaHitAirState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.HitAir;
    }

    public override void OnEnter(TombaState previousState) {
        _tomba.AnimationEvent = false;
        _tomba.AnimatorController.Play("Hit-Air");
        _lastPosition = _tomba.transform.position;
        _tomba.RigidBody.AddForce(new Vector2(_tomba.HitDirection * _tomba.HitKnockback.x, _tomba.HitKnockback.y), ForceMode2D.Impulse);
    }

    public override void OnExit() {
        _tomba.AnimationEvent = false;
    }

    public override TombaStateType Update() {
        if (_tomba.Grounded && _tomba.RigidBody.velocity.y < 0) {
            return TombaStateType.HitGround;
        }
        if (!_tomba.AnimationEvent || _tomba.Health <= 0) {
            return TombaStateType.None;
        }
        return TombaStateType.Fall;
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
