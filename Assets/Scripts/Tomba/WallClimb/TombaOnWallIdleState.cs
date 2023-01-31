using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaOnWallIdleState : TombaOnWallBaseState {
    public TombaOnWallIdleState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.OnWallIdle;
    }

    public override void OnEnter(TombaState previousState) {
        _tomba.AnimatorController.Play("WallClimb-Idle");
        base.OnEnter(previousState);
    }

    public override void OnExit() {
        base.OnExit();
    }

    public override TombaStateType Update() {
        _tomba.RigidBody.velocity = Vector2.zero;
        TombaStateType priorityState = base.Update();

        if (priorityState != TombaStateType.None) {
            return priorityState;
        }

        if (_tomba.VerticalInput > 0) {
            return TombaStateType.OnWallUp;
        }
        if (_tomba.VerticalInput < 0) {
            return TombaStateType.OnWallDown;
        }
        return TombaStateType.None;
    }
}
