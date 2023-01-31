using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaOnWallUpState : TombaOnWallBaseState {
    public TombaOnWallUpState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.OnWallUp;
    }

    public override void OnEnter(TombaState previousState) {
        base.OnEnter(previousState);
        _tomba.AnimatorController.Play("WallClimb-Up");
    }

    public override void OnExit() {
        base.OnExit();
    }

    public override TombaStateType Update() {
        TombaStateType priorityState = base.Update();

        if (priorityState != TombaStateType.None) {
            return priorityState;
        }

        if (_tomba.VerticalInput > 0) {
            _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x, _tomba.ClimbUpMoveSpeed);
            return TombaStateType.None;
        }else if(_tomba.VerticalInput < 0) {
            return TombaStateType.OnWallDown;
        }
        return TombaStateType.OnWallIdle;
    }
}
