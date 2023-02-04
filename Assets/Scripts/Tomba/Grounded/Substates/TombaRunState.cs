using UnityEngine;

public class TombaRunState : TombaState {
    public TombaRunState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.Run;
    }

    public override void OnEnter() {
        _tomba.StartWalkSound();
        _tomba.AnimatorController.Play("Walk/Run");
    }

    public override void OnExit() {
        _tomba.StopWalkSound();
    }

    public override void Update() {

        //Conditions to accelerate
        if (((_tomba.HorizontalInput > 0 && _tomba.HorizontalSpeed >= 0) || (_tomba.HorizontalInput < 0 && _tomba.HorizontalSpeed <= 0))) {
            //Accelerate based on the _direction of the input
            _tomba.HorizontalSpeed = _tomba.HorizontalInput == 1 ?
                Mathf.Min(_tomba.HorizontalSpeed + ((_tomba.Acceleration * Time.deltaTime)), _tomba.MaxRunSpeed) :
                    Mathf.Max(_tomba.HorizontalSpeed - ((_tomba.Acceleration * Time.deltaTime)), -_tomba.MaxRunSpeed);
            //Conditions to decelerate 
        } else if (_tomba.HorizontalSpeed != 0) {

            //Decelerate  
            _tomba.HorizontalSpeed = _tomba.HorizontalSpeed > 0 ?
                Mathf.Max(_tomba.HorizontalSpeed - (_tomba.Decceleration * Time.deltaTime), 0) :
                   Mathf.Min(_tomba.HorizontalSpeed + (_tomba.Decceleration * Time.deltaTime), 0);
        }

        _tomba.RigidBody.velocity = new Vector2(_tomba.HorizontalSpeed, _tomba.RigidBody.velocity.y);
    }

    public override TombaStateType CheckStateChange() {
        if ((_tomba.HorizontalInput < 0 && _tomba.HorizontalSpeed > 0) || (_tomba.HorizontalInput > 0 && _tomba.HorizontalSpeed < 0)) {
            return TombaStateType.Turn;
        }
        if (_tomba.HorizontalInput != 0 && _tomba.CheckPush()) {
            return TombaStateType.Push;
        }
        if (_tomba.DashInput) {
            return TombaStateType.Dash;
        }
        if (Mathf.Abs(_tomba.HorizontalSpeed) <= _tomba.MaxRunSpeed / 2) {
            return TombaStateType.Walk;
        }
        return TombaStateType.None;
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }
}
