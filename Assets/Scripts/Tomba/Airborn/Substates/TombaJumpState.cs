using UnityEngine;

public class TombaJumpState : TombaState {

    private float _jumpSpeed;
    private float _jTimer;

    public TombaJumpState(Tomba tomba) : base(tomba) {
    }
    public override TombaStateType Type() {
        return TombaStateType.Jump;
    }

    public override void OnEnter() {

        _tomba.AnimatorController.Play("Jump-Rise");
        SoundManager.Instance.PlaySound(SoundType.Jump, 1f);
        if (_tomba.IsDashing) {
            _jumpSpeed = _tomba.DashJumpSpeed;
            _jTimer = _tomba.DashMaxJTimer;
            _tomba.DashParticleSystem.Play();
        } else {
            _jumpSpeed = _tomba.JumpSpeed;
            _jTimer = _tomba.MaxJTimer;
        }

        _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x, _jumpSpeed);
    }

    public override void OnExit() {
    }

    public override void Update() {
        _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x, _jumpSpeed);
        _jTimer -= Time.deltaTime;
    }

    public override TombaStateType CheckStateChange() {
        if (_tomba.JumpInput == Tomba.InputType.NotPressed || _jTimer <= 0) {
            return TombaStateType.Fall;
        }

        return TombaStateType.None;
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }
}
