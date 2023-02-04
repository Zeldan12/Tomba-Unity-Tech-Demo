using UnityEngine;


public class TombaGroundedBaseState : TombaState {

    private TombaState _subState;
    public TombaGroundedBaseState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.GroundedBase;
    }

    public override void OnEnter() {
        _subState = _tomba.GetState(FindBestGroundedState());
        _subState.OnEnter();
        SoundManager.Instance.PlaySound(SoundType.Land, 1f);
    }

    public override void OnExit() {
        _subState.OnExit();
    }

    public override void Update() {
        _subState.Update();

        TombaStateType newState = _subState.CheckStateChange();

        if (newState != TombaStateType.None) {
            _subState.OnExit();
            _subState = _tomba.GetState(newState);
            _subState.OnEnter();
        }
        
    }

    private TombaStateType FindBestGroundedState() {
        if (_tomba.HorizontalSpeed == 0) {
            return TombaStateType.Idle;
        } else if (_tomba.DashInput) {
            return TombaStateType.Dash;
        } else if (_tomba.HorizontalSpeed >= _tomba.MaxRunSpeed / 2) {
            return TombaStateType.Run;
        }
        return TombaStateType.Walk;
    }

    public override TombaStateType CheckStateChange() {
        if (_tomba.AttackInput == Tomba.InputType.JustPressed) {
            return TombaStateType.AttackCharge;
        }

        if (_tomba.JumpInput == Tomba.InputType.JustPressed || !_tomba.Grounded) {
            return TombaStateType.AirbornBase;
        }

        return TombaStateType.None;
    }

    public override void CameraBehaviour(CameraController cameraController) {
        Transform player = _tomba.transform;
        Transform transform = cameraController.transform;
        Camera camera = cameraController.Camera;

        float distance = Mathf.Abs(transform.position.z - player.position.z);
        var frustumHeight = (2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad)) / 2;
        float direction = (player.transform.rotation.y == 0) ? 1 : -1;
        float xMovement = Mathf.Lerp(transform.position.x, player.position.x + (cameraController.Offset * direction), cameraController.Movespeed);
        float yMovement = Mathf.Lerp(transform.position.y, player.position.y + cameraController.HeigthOffset, cameraController.Movespeed);

        transform.position = new Vector3(xMovement, yMovement, transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w), cameraController.Movespeed);

    }

}
