using UnityEngine;

public class TombaOnWallDownState : TombaState {
    public TombaOnWallDownState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.OnWallDown;
    }

    public override void OnEnter() {
        _tomba.AnimatorController.Play("WallClimb-Down-Start");
    }

    public override void OnExit() {
    }

    public override void Update() {
        _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x, -_tomba.ClimbDownMoveSpeed);
    }

    public override TombaStateType CheckStateChange() {
        if (_tomba.CheckLedge() && _tomba.CheckWall()) {
            return TombaStateType.OnLedge;
        }
        if (_tomba.VerticalInput < 0) {
            return TombaStateType.None;
        } else if (_tomba.VerticalInput > 0) {
            return TombaStateType.OnWallUp;
        }
        return TombaStateType.OnWallIdle;
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }
}
