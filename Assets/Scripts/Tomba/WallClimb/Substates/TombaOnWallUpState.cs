using UnityEngine;

public class TombaOnWallUpState : TombaState {
    public TombaOnWallUpState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.OnWallUp;
    }

    public override void OnEnter() {
        _tomba.AnimatorController.Play("WallClimb-Up");
    }

    public override void OnExit() {
    }

    public override void Update() {
        _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x, _tomba.ClimbUpMoveSpeed);
    }

    public override TombaStateType CheckStateChange() {
        if (_tomba.VerticalInput > 0) {
            if (_tomba.CheckLedge() && _tomba.CheckWall()) {
                return TombaStateType.OnLedge;
            }
            return TombaStateType.None;
        } else if (_tomba.VerticalInput < 0) {
            return TombaStateType.OnWallDown;
        }
        return TombaStateType.OnWallIdle;
    }

    public override void CameraBehaviour(CameraController cameraController) {
        return;
    }
}
