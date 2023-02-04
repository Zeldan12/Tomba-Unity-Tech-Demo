using UnityEngine.InputSystem;

[System.Serializable]
public class TombaIdleState : TombaState {
    public TombaIdleState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.Idle;
    }

    public override void OnEnter() {
        _tomba.AnimatorController.Play("Stop");
    }

    public override void OnExit() {
    }

    public override void Update() {
    }

    public override TombaStateType CheckStateChange() {
        if (_tomba.HorizontalInput != 0) {
            if (_tomba.DashInput) {
                return TombaStateType.Dash;
            }
            return TombaStateType.Walk;
        }
        return TombaStateType.None;
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }
}
