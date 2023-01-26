using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TombaOnLedgeState : TombaOnWallBaseState {
    public TombaOnLedgeState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
        base.CameraBehaviour(cameraController);
    }

    public override bool Is(TombaStateType stateType) {
        return stateType == TombaStateType.OnLedge;
    }

    public override void OnEnter(TombaState previousState) {
        base.OnEnter(previousState);
        _tomba.AnimatorController.Play("WallClimb-Ledge");
        _tomba.LedgeAdjuster.enabled = true;
    }

    public override void OnExit() {
        base.OnExit();
        _tomba.LedgeAdjuster.enabled = false;
        _tomba.RigidBody.velocity = new Vector2(_direction * 10, 0);
    }

    public override TombaStateType Update() {
        _tomba.RigidBody.velocity = new Vector2(_direction * 10, -15);

        if (_tomba.JumpInput == Tomba.JumpInputType.JustPressed) {
            return TombaStateType.EdgeClimb;
        }
        if (_tomba.PlayerInput.actions["VerticalMove"].WasPressedThisFrame()) {
            if (_tomba.VerticalInput > 0) {
                return TombaStateType.EdgeClimb;
            } else if (_tomba.VerticalInput < 0) {
                return TombaStateType.OnWallDown;
            }
        }
        /*if (!_tomba.OnLedge) {
            return TombaStateType.OnWallIdle;
        }*/
        return TombaStateType.None;

    }
}
