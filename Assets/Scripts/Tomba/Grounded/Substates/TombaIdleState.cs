using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class TombaIdleState : TombaGroundedBaseState
{
    public TombaIdleState(Tomba tomba) : base(tomba) {
    }

    public override TombaStateType Type() {
        return TombaStateType.Idle;
    }

    public override void OnEnter(TombaState previousState) {
        base.OnEnter(previousState);
        _tomba.AnimatorController.Play("Stop");
    }

    public override void OnExit() {
    }

    public override TombaStateType Update() {
        PlayerInput playerInput = _tomba.PlayerInput;
        float horizontalInput = playerInput.actions["HorizontalMove"].ReadValue<float>();

        TombaStateType priorityState = base.Update();

        if (priorityState == TombaStateType.None) {
            if (horizontalInput != 0) {
                if (_tomba.DashInput) {
                    return TombaStateType.Dash;
                }
                return TombaStateType.Walk;
            }
        }

        return priorityState;
    }

}
