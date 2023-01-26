using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaOnWallDownState : TombaOnWallBaseState {
    public TombaOnWallDownState(Tomba tomba) : base(tomba) {
    }

    public override bool Is(TombaStateType stateType) {
        return stateType == TombaStateType.OnWallDown;
    }

    public override void OnEnter(TombaState previousState) {
        base.OnEnter(previousState);
        _tomba.AnimatorController.Play("WallClimb-Down-Start");
    }

    public override void OnExit() {
        base.OnExit();
    }

    public override TombaStateType Update() {
        TombaStateType priorityState = base.Update();

        if (priorityState != TombaStateType.None) {
            return priorityState;
        }

        if (_tomba.VerticalInput < 0) {
            _tomba.RigidBody.velocity = new Vector2(_tomba.RigidBody.velocity.x, -_tomba.ClimbDownMoveSpeed);
            if (_tomba.Grounded) {
                return TombaGroundedBaseState.FindBestGroundedState(_tomba);
            }
            return TombaStateType.None;
        } else if (_tomba.VerticalInput > 0) {
            return TombaStateType.OnWallUp;
        }
        return TombaStateType.OnWallIdle;
    }
}
