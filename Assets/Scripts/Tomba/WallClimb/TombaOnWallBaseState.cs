using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;

public class TombaOnWallBaseState : TombaState {

    protected float _gravity;
    protected float _direction;
    protected Vector3 _lastPosition;

    public TombaOnWallBaseState(Tomba tomba) : base(tomba) {
    }

    public override bool Is(TombaStateType stateType) {
        return false;
    }

    public override void OnEnter(TombaState previousState) {
        if (previousState != null && (!previousState.GetType().IsSubclassOf(typeof(TombaOnWallBaseState)) || GetType() == typeof(TombaOnLedgeState))) {    
            SoundManager.Instance.PlaySound(SoundType.WallLatch, 1f);
        }
        _gravity = _tomba.RigidBody.gravityScale;
        _direction = (_tomba.transform.rotation.y == 0 ? 1 : -1);
        _tomba.RigidBody.gravityScale = 0;
        _tomba.HorizontalSpeed = 0;
        _lastPosition = _tomba.transform.position;
    }

    public override void OnExit() {
        _tomba.RigidBody.gravityScale = _gravity;
    }

    public override TombaStateType Update() {
        _tomba.RigidBody.velocity = new Vector2(_direction * 10, _tomba.RigidBody.velocity.y);
        if (_tomba.OnLedge) {
            return TombaStateType.OnLedge;
        }
        if (!_tomba.OnWall) {
            return TombaStateType.Fall;
        }
        if (_tomba.JumpInput == Tomba.JumpInputType.JustPressed) {
            Vector3 pos = _tomba.transform.position;
            _tomba.transform.position = new Vector3(pos.x + (_tomba.WallJumpOffset * -_direction), pos.y, pos.z);
            return TombaStateType.Jump;
        }
        return TombaStateType.None;
    }

    static public TombaStateType FindBestOnWallState(Tomba tomba) {
        if (tomba.OnLedge) {
            return TombaStateType.OnLedge;
        }
        if (tomba.VerticalInput == 0) {
            return TombaStateType.OnWallIdle;
        } else if (tomba.VerticalInput > 0) {
            return TombaStateType.OnWallUp;
        } else{
           return TombaStateType.OnWallDown;
        }
    }

    public override void CameraBehaviour(CameraController cameraController) {

        Transform player = _tomba.transform;
        Transform transform = cameraController.transform;
        Camera camera = cameraController.Camera;

        float direction = (player.transform.rotation.y == 0) ? 1 : -1;

        float xMovement = (player.position.x - _lastPosition.x);
        float xlerp = Mathf.Lerp(transform.position.x, player.position.x + cameraController.WallOffSet * -direction, cameraController.Movespeed);
        _lastPosition = player.position;
        float distance = Mathf.Abs(transform.position.z - player.position.z);
        var frustumHeight = (2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad)) / 2;
        float yMovement = transform.position.y;
        

        if (player.position.y > transform.position.y + frustumHeight - cameraController.WallTopLimit) {
            yMovement = transform.position.y + Mathf.Abs((player.position.y - (transform.position.y + frustumHeight - cameraController.WallTopLimit)));
        } else if (player.position.y < transform.position.y - frustumHeight + cameraController.WallBotLimit) {
            yMovement = transform.position.y - Mathf.Abs((player.position.y - (transform.position.y - frustumHeight + cameraController.WallBotLimit)));
        }

        transform.position = new Vector3(xlerp + xMovement, yMovement, transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x, Mathf.Deg2Rad * cameraController.WallRotationDeggre * direction, transform.rotation.z, transform.rotation.w), cameraController.Movespeed) ;
    }
}
