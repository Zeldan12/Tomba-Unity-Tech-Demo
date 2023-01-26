using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombaTurnState : TombaGroundedBaseState {

    private float _decceleration;

    public TombaTurnState(Tomba tomba) : base(tomba) {
    }

    public override bool Is(TombaStateType stateType) {
        return stateType == TombaStateType.Turn;
    }

    public override void OnEnter(TombaState previousState) {
        base.OnEnter(previousState);
        _tomba.AnimatorController.Play("Turn1");
        if (_tomba.DashInput) {
            _decceleration = _tomba.DashTurnDecceleration;
        } else {
            _decceleration = _tomba.TurnDecceleration;
        }
        
    }

    public override void OnExit() {
        Quaternion rot = _tomba.transform.rotation;
        _tomba.transform.rotation = Quaternion.Euler(rot.x,(rot.y == 0) ? 180 : 0,rot.z);
    }

    public override TombaStateType Update() {
        _tomba.HorizontalSpeed = _tomba.HorizontalSpeed > 0 ?
                Mathf.Max(_tomba.HorizontalSpeed - (_decceleration * Time.deltaTime), 0) :
                   Mathf.Min(_tomba.HorizontalSpeed + (_decceleration * Time.deltaTime), 0);

        _tomba.RigidBody.velocity = new Vector2(_tomba.HorizontalSpeed, _tomba.RigidBody.velocity.y);

        TombaStateType priorityState = base.Update();

        if (priorityState == TombaStateType.None) {
           
            if (_tomba.HorizontalSpeed == 0) {
                if (_tomba.HorizontalInput == 0) {
                    return TombaStateType.Idle;
                }
                if (Mathf.Abs(_tomba.HorizontalInput) > 0) {
                    if (_tomba.DashInput) {
                        return TombaStateType.Dash;
                    }
                    return TombaStateType.Walk;
                }
            }
        }
        return priorityState;
    }
}
