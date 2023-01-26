using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaStartFallState : TombaState {
    public TombaStartFallState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }

    public override bool Is(TombaStateType stateType) {
        return stateType == TombaStateType.StartFall;
    }

    public override void OnEnter(TombaState previousState) {
        _tomba.AnimatorController.Play("Jump-Apex");
    }

    public override void OnExit() {
    }

    public override TombaStateType Update() {
        if (_tomba.Grounded && _tomba.RigidBody.velocity.y <= 0) {
            _tomba.IsDashing = false;
            return TombaGroundedBaseState.FindBestGroundedState(_tomba);
        }
        return TombaStateType.None;
    }
}
