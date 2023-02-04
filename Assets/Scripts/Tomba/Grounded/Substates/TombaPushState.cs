using UnityEngine;

public class TombaPushState : TombaState {

    private int _direction = 0;
    public TombaPushState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }

    public override TombaStateType Type() {
        return TombaStateType.Push;
    }

    public override void OnEnter() {
        _direction = (int)_tomba.HorizontalInput;
        _tomba.AnimatorController.Play("Push");
        _tomba.HorizontalSpeed = 0;
    }

    public override void OnExit() {

    }

    public override void Update() {
        _tomba.RigidBody.velocity = new Vector2(_tomba.PushForce * _tomba.HorizontalInput, _tomba.RigidBody.velocity.y);
    }

    public override TombaStateType CheckStateChange() {
        if (_tomba.HorizontalInput == 0) {
            return TombaStateType.Idle;
        }
        if (_tomba.HorizontalInput != _direction || !_tomba.CheckPush()) {
            if (_tomba.DashInput) {
                return TombaStateType.Dash;
            }
            return TombaStateType.Walk;
        }
        return TombaStateType.None;
    }
}
