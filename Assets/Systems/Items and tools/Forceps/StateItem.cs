using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum StateItems
{
    Default,
    Idle,
    Drag,
    LinearMove,
    LinearRotate,
    Interacts
}

public class StateItem : MonoBehaviour
{
    public StateItems State = StateItems.Idle;

    public void ChangeState(StateItems state)
    {
        State = (state != StateItems.Default)?state:State;

        switch (state)
        {
            case StateItems.Default: break;
            case StateItems.Drag: break;
            case StateItems.LinearMove:
                TryGetComponent(out LinearMove linearMove);
                linearMove.enabled = true;
                break;
            case StateItems.LinearRotate:
                TryGetComponent(out LinearRotate linearRotate);
                linearRotate.enabled = true;
                break;
            case StateItems.Interacts: break;
        }
    }
}