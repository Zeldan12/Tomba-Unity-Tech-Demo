public class TombaHitDieState : TombaState {
    public TombaHitDieState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }

    public override TombaStateType Type() {
        return TombaStateType.HitDie;
    }

    public override void OnEnter(TombaState previousState) {
        _tomba.AnimatorController.Play("Hit-Die");
    }

    public override void OnExit() {
        _tomba.AnimationEvent = false;
    }

    public override TombaStateType Update() {
        return TombaStateType.None;
    }
}
