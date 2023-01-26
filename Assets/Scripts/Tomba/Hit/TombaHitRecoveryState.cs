using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaHitRecoveryState : TombaState {
    public TombaHitRecoveryState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
    }

    public override bool Is(TombaStateType stateType) {
        return stateType == TombaStateType.HitRecovery;
    }

    public override void OnEnter(TombaState previousState) {
        _tomba.AnimationEvent = false;
        _tomba.AnimatorController.Play("Hit-Recovery");
    }

    public override void OnExit() {
        _tomba.AnimationEvent = false;
    }

    public override TombaStateType Update() {
        if (_tomba.AnimationEvent) {
            return TombaGroundedBaseState.FindBestGroundedState(_tomba);
        }
        return TombaStateType.None;
    }
}
