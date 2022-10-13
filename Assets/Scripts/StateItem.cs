using System;
using System.Collections;
using System.Collections.Generic;
using Cursor;
using UnityEditor;
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
    Interacts,
    BackToRespawn
}

public class StateItem : MonoBehaviour
{ 
    public StateItems State = StateItems.Idle;
    
    public void ChangeState(StateItems state, LinearValue linearValue = default)
    {
        SetState(state, linearValue);
        
        //Parent
        if(transform.parent.TryGetComponent(out StateItem parentStateItem))
            parentStateItem.SetState(state);

        //Child
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<StateItem>()?.SetState(state);
        }
    }

    private void SetState(StateItems state, LinearValue linearValue = default)
    {
        switch (state)
        {
            case StateItems.Default: break;
            case StateItems.Idle:
                State = state;
                CursorSkin.Instance.UseArrow();
                break;
            case StateItems.Interacts:
                State = state;
                CursorSkin.Instance.UseLoad();
                break;
            case StateItems.BackToMouse:
                if(State != StateItems.Idle && State != StateItems.Default)
                    State = state;
                break;
            case StateItems.BackToRespawn:
                State = state;
                CursorSkin.Instance.UseLoad();
                break;
            case StateItems.LinearMove:
                State = state;
                if (!TryGetComponent(out LinearMove linearMove)) break;
                linearMove.axisInput = linearValue.axisInput;
                linearMove.axis = linearValue.axis;
                linearMove.EdgeMove = linearValue.edge;
                linearMove.enabled = true;
                break;
            case StateItems.LinearRotate:
                State = state;
                if (!TryGetComponent(out LinearRotate linearRotate)) break;
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