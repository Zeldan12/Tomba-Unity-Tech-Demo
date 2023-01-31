using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaJumpState : TombaAirbornBaseState {

    private float _jumpSpeed;
    private float _jTimer;

    public TombaJumpState(Tomba tomba) : base(tomba) {
    }
    public override TombaStateType Type() {
        return TombaStateType.Jump;
    }

    public override void OnEnter(TombaState previousState) {
        
        _tomba.AnimatorController.Play("Jump-Rise");
        SoundManager.Instance.PlaySound(SoundType.Jump, 1f);
        if (_tomba.DashInput) {
            _tomba.IsDashing = true;
            _jumpSpeed = _tomba.DashJumpSpeed;
            _jTimer = _tomba.DashMaxJTimer;
            _tomba.DashParticleSystem.Play();
        } else {
            _jumpSpeed = _tomba.JumpSpeed;
            _jTimer = _tomba.MaxJTimer;
        }
        base.OnEnter(previousState);
        _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x, _jumpSpeed);
    }

    public override void OnExit() {
        base.OnExit();
    }

    public override TombaStateType Update() {


        TombaStateType prioState = base.Update();

        _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x, _jumpSpeed);

        _jTimer -= Time.deltaTime;

        if (prioState == TombaStateType.None) {
            if (_tomba.JumpInput == Tomba.JumpInputType.NotPressed || _jTimer <= 0) {
                return TombaStateType.Fall;
            }
        }
        return prioState;
    }
}
