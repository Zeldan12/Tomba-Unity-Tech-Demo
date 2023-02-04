public class TombaHitDieState : TombaState {
    public TombaHitDieState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }

    public override TombaStateType Type() {
        return TombaStateType.HitDie;
    }

    public override void OnEnter() {
        _tomba.AnimationEvent = false;
        _tomba.AnimatorController.Play("Hit-Die");
    }

    public override void OnExit() {
        _tomba.AnimationEvent = false;
    }

    public override void Update() {

    }

    public override TombaStateType CheckStateChange() {
        return TombaStateType.None;
    }
}
