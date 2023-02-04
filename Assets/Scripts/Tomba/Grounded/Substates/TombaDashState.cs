using UnityEngine;

public class TombaDashState : TombaState {
    public TombaDashState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.Dash;
    }

    public override void OnEnter() {
        _tomba.AnimatorController.Play("Dash");
        _tomba.StartWalkSound();
        Quaternion rot = _tomba.transform.rotation;
        _tomba.transform.rotation = Quaternion.Euler(rot.x, (_tomba.HorizontalInput == 1) ? 0 : 180, rot.z);
        _tomba.DashParticleSystem.Play();
    }

    public override void OnExit() {
        //_tomba.IsDashing = false;
        _tomba.DashParticleSystem.Stop();
        _tomba.StopWalkSound();
    }

    public override void Update() {


        //Conditions to accelerate
        if (((_tomba.HorizontalInput > 0 && _tomba.HorizontalSpeed >= 0) || (_tomba.HorizontalInput < 0 && _tomba.HorizontalSpeed <= 0))) {
            //Accelerate based on the _direction of the input
            _tomba.HorizontalSpeed = _tomba.HorizontalInput == 1 ?
                Mathf.Min(_tomba.HorizontalSpeed + ((_tomba.DashAcceleration * Time.deltaTime)), _tomba.MaxDashSpeed) :
                    Mathf.Max(_tomba.HorizontalSpeed - ((_tomba.DashAcceleration * Time.deltaTime)), -_tomba.MaxDashSpeed);

            //Conditions to decelerate 
        } else if (_tomba.HorizontalSpeed != 0) {

            //Decelerate  
            _tomba.HorizontalSpeed = _tomba.HorizontalSpeed > 0 ?
                Mathf.Max(_tomba.HorizontalSpeed - (_tomba.DashDecceleration * Time.deltaTime), 0) :
                   Mathf.Min(_tomba.HorizontalSpeed + (_tomba.DashDecceleration * Time.deltaTime), 0);

        }

        _tomba.RigidBody.velocity = new Vector2(_tomba.HorizontalSpeed, _tomba.RigidBody.velocity.y);

    }

    public override TombaStateType CheckStateChange() {
        if ((_tomba.HorizontalInput < 0 && _tomba.HorizontalSpeed > 0) || (_tomba.HorizontalInput > 0 && _tomba.HorizontalSpeed < 0)) {
            //_tomba.IsDashing = true;
            return TombaStateType.Turn;
        }
        if (_tomba.HorizontalInput != 0 && _tomba.CheckPush()) {
            return TombaStateType.Push;
        }
        if (_tomba.HorizontalSpeed == 0) {
            return TombaStateType.Idle;
        }
        if (!_tomba.DashInput) {
            if (_tomba.HorizontalSpeed >= _tomba.MaxRunSpeed / 2) {
                return TombaStateType.Run;
            }
            return TombaStateType.Walk;
        }

        return TombaStateType.None;
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }
}
