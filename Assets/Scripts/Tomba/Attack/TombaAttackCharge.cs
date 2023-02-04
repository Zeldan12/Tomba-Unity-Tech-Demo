using UnityEngine;

public class TombaAttackCharge : TombaState {

    private Vector3 _lastPosition;
    private float _chargeTime = 0;

    public TombaAttackCharge(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
        Transform player = _tomba.transform;
        Transform transform = cameraController.transform;
        Camera camera = cameraController.Camera;

        float xMovement = player.position.x - _lastPosition.x;
        _lastPosition = player.position;
        float distance = Mathf.Abs(transform.position.z - player.position.z);
        var frustumHeight = (2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad)) / 2;
        float yMovement = transform.position.y;

        if (player.position.y > transform.position.y + frustumHeight - cameraController.HeigthOffset) {
            yMovement = transform.position.y + Mathf.Abs((player.position.y - (transform.position.y + frustumHeight - cameraController.HeigthOffset)));
        } else if (player.position.y < transform.position.y - frustumHeight + cameraController.HeigthOffset) {
            yMovement = transform.position.y - Mathf.Abs((player.position.y - (transform.position.y - frustumHeight + cameraController.HeigthOffset)));
        }
        transform.position = new Vector3(transform.position.x + xMovement, yMovement, transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w), cameraController.Movespeed);
    }

    public override TombaStateType CheckStateChange() {
        return TombaStateType.None;
    }

    public override void OnEnter() {
        _tomba.AnimatorController.Play("Attack-Charge");
        _tomba.HorizontalSpeed = 0;
    }

    public override void OnExit() {

    }

    public override TombaStateType Type() {
        return TombaStateType.AttackCharge;
    }

    public override void Update() {

    }
}
