using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TombaState{

    protected Tomba _tomba;

    public TombaState(Tomba tomba) {
        _tomba = tomba;
    }

    public abstract void OnEnter(TombaState previousState);
    public abstract TombaStateType Update();
    public abstract void OnExit();
    public abstract TombaStateType Type();

    public abstract void CameraBehaviour(CameraController cameraController);
}
