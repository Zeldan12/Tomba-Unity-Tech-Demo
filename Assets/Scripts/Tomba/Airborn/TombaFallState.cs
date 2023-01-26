using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaFallState : TombaAirbornBaseState {
    public TombaFallState(Tomba tomba) : base(tomba) {
    }

    public override bool Is(TombaStateType stateType) {
        return stateType == TombaStateType.Fall;
    }

    public override void OnEnter(TombaState previousState) {
        base.OnEnter(previousState);

        _tomba.AnimatorController.Play("Jump-Apex");
    }

    public override void OnExit() {
        base.OnExit();
    }

    public override TombaStateType Update() {
        return base.Update();
    }
}
