using System;
using System.Collections;
using System.Collections.Generic;using UnityEditor;
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
        SetState(state, linearValue);
        
        if(transform.parent.TryGetComponent(out StateItem parentStateItem))
            parentStateItem.SetState(state, linearValue);

        foreach (var child in GetComponentsInChildren<StateItem>())
        {
            child.SetState(state, linearValue);
        }
    }

    private void SetState(StateItems state, LinearValue linearValue = default)
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

[CustomEditor(typeof(StateItem))]
public class StateItemEditor : Editor
{
    private StateItem parentStateItem;
    private void OnEnable()
    {
        Selection.activeGameObject.transform.parent.TryGetComponent(out parentStateItem);
    }

    // public override void OnInspectorGUI()
    // {
    //     var stateItem = target as StateItem;
    //     
    //     if (parentStateItem)
    //     {
    //         EditorGUILayout.BeginHorizontal();
    //         EditorGUILayout.LabelField("State(Parent) = ");
    //         stateItem.State = (StateItems)EditorGUILayout.EnumPopup(stateItem.State);
    //         EditorGUILayout.EndHorizontal();
    //         parentStateItem.State = stateItem.State;
    //     }
    //     else
    //     {
    //         EditorGUILayout.BeginHorizontal();
    //         EditorGUILayout.LabelField("State");
    //         stateItem.State = (StateItems)EditorGUILayout.EnumPopup(stateItem.State);
    //         EditorGUILayout.EndHorizontal();
    //     }
    // }
}