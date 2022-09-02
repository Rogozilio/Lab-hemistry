using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum StateItems
{
    Default,
    Idle,
    BackToMouse,
    Drag,
    LinearMove,
    LinearRotate,
    Interacts
}

public class StateItem : MonoBehaviour
{
    public StateItems State = StateItems.Idle;

    public void ChangeState(StateItems state, LinearValue linearValue = default)
    {
        switch (state)
        {
            case StateItems.Default: break;
            case StateItems.BackToMouse:
                if(State != StateItems.Idle && State != StateItems.Default)
                    State = state;
                break;
            case StateItems.LinearMove:
                State = state;
                TryGetComponent(out LinearMove linearMove);
                linearMove.axisInput = linearValue.axisInput;
                linearMove.axis = linearValue.axis;
                linearMove.EdgeMove = linearValue.edge;
                linearMove.enabled = true;
                break;
            case StateItems.LinearRotate:
                State = state;
                TryGetComponent(out LinearRotate linearRotate);
                linearRotate.axisInput = linearValue.axisInput;
                linearRotate.axis = linearValue.axis;
                linearRotate.edgeRotate = linearValue.edge;
                linearRotate.enabled = true;
                break;
            default: 
                State = state;
                break;
        }
    }
}