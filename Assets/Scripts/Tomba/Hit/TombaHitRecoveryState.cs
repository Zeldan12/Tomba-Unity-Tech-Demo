public class TombaHitRecoveryState : TombaState {
    public TombaHitRecoveryState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }

    public override TombaStateType Type() {
        return TombaStateType.HitRecovery;
    }

    public override void OnEnter() {
        _tomba.AnimationEvent = false;
        _tomba.AnimatorController.Play("Hit-Recovery");
    }

    public override void OnExit() {
        _tomba.AnimationEvent = false;
    }

    public override void Update() {

    }

    public override TombaStateType CheckStateChange() {
        if (_tomba.AnimationEvent) {
            return TombaStateType.GroundedBase;
        }
        return TombaStateType.None;
    }
}
