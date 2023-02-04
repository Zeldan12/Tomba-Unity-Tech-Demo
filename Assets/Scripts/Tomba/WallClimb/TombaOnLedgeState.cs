using UnityEngine;

public class TombaOnLedgeState : TombaState {
    public TombaOnLedgeState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }

    public override TombaStateType Type() {
        return TombaStateType.OnLedge;
    }

    public override void OnEnter() {
        _tomba.AnimatorController.Play("WallClimb-Ledge");
        SoundManager.Instance.PlaySound(SoundType.WallLatch, 1f);
        _tomba.LedgeAdjuster.enabled = true;
    }

    public override void OnExit() {
        _tomba.LedgeAdjuster.enabled = false;
    }

    public override void Update() {
        _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x, -15);
    }

    public override TombaStateType CheckStateChange() {
        if (_tomba.VerticalInput < 0) {
            return TombaStateType.OnWallDown;
        }
        return TombaStateType.None;
    }
}
