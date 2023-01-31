using UnityEngine;

public class TombaPushState : TombaGroundedBaseState {

    private int _direction = 0;
    public TombaPushState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
        base.CameraBehaviour(cameraController);
    }

    public override TombaStateType Type() {
        return TombaStateType.Push;
    }

    public override void OnEnter(TombaState previousState) {
        _direction =  (int)_tomba.HorizontalInput;
        _tomba.AnimatorController.Play("Push");
        _tomba.HorizontalSpeed = 0;
    }

    public override void OnExit() {
        
    }

    public override TombaStateType Update() {
        _tomba.RigidBody.velocity = new Vector2(_tomba.PushForce * _tomba.HorizontalInput, _tomba.RigidBody.velocity.y);
        if (_tomba.HorizontalInput != _direction || !_tomba.CheckPush()) {
            return FindBestGroundedState(_tomba);
        }
        TombaStateType prioState = base.Update();
        return prioState;
    }
}
