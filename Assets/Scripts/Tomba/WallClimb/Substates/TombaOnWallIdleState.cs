using System;
using UnityEngine;

public class TombaOnWallIdleState : TombaState {
    public TombaOnWallIdleState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.OnWallIdle;
    }

    public override void OnEnter() {
        _tomba.AnimatorController.Play("WallClimb-Idle");
    }

    public override void OnExit() {
    }

    public override void Update() {
        _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x,0);
    }

    public override TombaStateType CheckStateChange() {
        if (_tomba.CheckLedge() && _tomba.CheckWall()) {
            return TombaStateType.OnLedge;
        }
        if (_tomba.VerticalInput > 0) {
            return TombaStateType.OnWallUp;
        }
        if (_tomba.VerticalInput < 0) {
            return TombaStateType.OnWallDown;
        }
        return TombaStateType.None;
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }
}
