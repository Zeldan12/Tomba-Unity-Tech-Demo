using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TombaEdgeClimbState : TombaState {

    
    public TombaEdgeClimbState(Tomba tomba) : base(tomba) {
    }

    public override void CameraBehaviour(CameraController cameraController) {
        Transform player = _tomba.transform;
        Transform transform = cameraController.transform;
        Camera camera = cameraController.Camera;

        float distance = Mathf.Abs(transform.position.z - player.position.z);
        var frustumHeight = (2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad)) / 2;
        float direction = (player.transform.rotation.y == 0) ? 1 : -1;
        float xMovement = Mathf.Lerp(transform.position.x, player.position.x + (cameraController.Offset * direction), cameraController.Movespeed);
        float yMovement = Mathf.Lerp(transform.position.y, player.position.y + cameraController.HeigthOffset, cameraController.Movespeed);

        transform.position = new Vector3(xMovement, yMovement, transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w), cameraController.Movespeed);
    }

    public override TombaStateType Type() {
        return TombaStateType.EdgeClimb;
    }

    public override void OnEnter(TombaState previousState) {
        _tomba.AnimationEvent = false;
        _tomba.RigidBody.gravityScale = 0;
        _tomba.AnimatorController.Play("EdgeClimb");
        
    }

    public override void OnExit() {
        _tomba.RigidBody.gravityScale = 3.5f;
        _tomba.transform.position = _tomba.Body.transform.position;
        _tomba.AnimationEvent = false;
    }

    public override TombaStateType Update() {

        if (_tomba.AnimationEvent) {
            if (_tomba.Grounded) {
                return TombaGroundedBaseState.FindBestGroundedState(_tomba);
            }
            return TombaStateType.Fall;
        }
        return TombaStateType.None;
    }
}
