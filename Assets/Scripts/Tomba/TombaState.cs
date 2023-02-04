public abstract class TombaState {

    protected Tomba _tomba;

    public TombaState(Tomba tomba) {
        _tomba = tomba;
    }

    public abstract void OnEnter();
    public abstract void Update();
    public abstract TombaStateType CheckStateChange();
    public abstract void OnExit();
    public abstract TombaStateType Type();
    public abstract void CameraBehaviour(CameraController cameraController);
}
