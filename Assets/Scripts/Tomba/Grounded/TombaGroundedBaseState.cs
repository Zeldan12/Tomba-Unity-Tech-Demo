using UnityEngine;


public class TombaGroundedBaseState : TombaState {

    public TombaGroundedBaseState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.GroundedBase;
    }

    public override void OnEnter(TombaState previousState) {
        if (previousState != null && !previousState.GetType().IsSubclassOf(typeof(TombaGroundedBaseState))) {
            SoundManager.Instance.PlaySound(SoundType.Land, 1f);
        }
    }

    public override void OnExit() {
    }

    public override TombaStateType Update() {

        if (!_tomba.Grounded) {
            return TombaStateType.Fall;
        }

        if (_tomba.JumpInput == Tomba.JumpInputType.JustPressed) {
            return TombaStateType.Jump;
        }

        return TombaStateType.None;
    }

    static public TombaStateType FindBestGroundedState(Tomba tomba) {
        if (tomba.HorizontalSpeed == 0) {
            return TombaStateType.Idle;
        }else if (tomba.DashInput) {
            return TombaStateType.Dash;
        }else if (tomba.HorizontalSpeed >= tomba.MaxRunSpeed/2) {
            return TombaStateType.Run;
        }
        return TombaStateType.Walk;
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
