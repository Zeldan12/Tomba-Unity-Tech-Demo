
public class TombaFallState : TombaState {
    public TombaFallState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.Fall;
    }

    public override void OnEnter() {
        _tomba.AnimatorController.Play("Jump-Apex");
    }

    public override void OnExit() {
    }

    public override void Update() {
    }

    public override TombaStateType CheckStateChange() {
        return TombaStateType.None;
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }
}
