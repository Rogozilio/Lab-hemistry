using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ActionPlace : MonoBehaviour
{
    [System.Serializable]
    public struct Events
    {
        public UnityEvent onActionStart;
        public UnityEvent onAction;
        public UnityEvent onActionEnd;
    }

    [HideInInspector] [SerializeField] public StateItems stateAfterOnActionStart;
    [HideInInspector] [SerializeField] public StateItems stateAfterOnAction;
    [HideInInspector] [SerializeField] public StateItems stateAfterOnActionEnd;

    public GameObject target;
    public StateItems stateStart = StateItems.Drag;

    [SerializeField] Events _events;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target && target.GetComponent<StateItem>().State == stateStart)
        {
            _events.onActionStart.Invoke();
            target.GetComponent<StateItem>().ChangeState(stateAfterOnActionStart);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {
            _events.onAction.Invoke();
            target.GetComponent<StateItem>().ChangeState(stateAfterOnAction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            _events.onActionEnd.Invoke();
            target.GetComponent<StateItem>().ChangeState(stateAfterOnActionEnd);
        }
    }
}

[CustomEditor(typeof(ActionPlace))]
[CanEditMultipleObjects]
public class ActionPlaceEditor : Editor
{
    private ActionPlace _actionPlace;
    private string[] _optionStateItems;

    private int _indexOnActionStart;
    private int _indexOnAction;
    private int _indexOnActionEnd;

    private void OnEnable()
    {
        _actionPlace = (ActionPlace)target;
        _optionStateItems = new string[Enum.GetValues(typeof(StateItems)).Length + 1];

        _optionStateItems[0] = "None";
        for (var i = 1; i < _optionStateItems.Length; i++)
        {
            var state = (StateItems)(i - 1);
            _optionStateItems[i] = state.ToString();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _actionPlace = (ActionPlace)target;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Next State After \"OnActionStart\"");
        _actionPlace.stateAfterOnActionStart =
            (StateItems)EditorGUILayout.EnumPopup(_actionPlace.stateAfterOnActionStart, GUILayout.Width(150f));
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Next State for \"OnAction\"");
        _actionPlace.stateAfterOnAction =
            (StateItems)EditorGUILayout.EnumPopup(_actionPlace.stateAfterOnAction, GUILayout.Width(150f));
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Next State After \"OnActionEnd\"");
        _actionPlace.stateAfterOnActionEnd =
            (StateItems)EditorGUILayout.EnumPopup(_actionPlace.stateAfterOnActionEnd, GUILayout.Width(150f));
        GUILayout.EndHorizontal();

        if (_indexOnActionStart > 0)
            _actionPlace.stateAfterOnActionStart = (StateItems)_indexOnActionStart;
        if (_indexOnAction > 0)
            _actionPlace.stateAfterOnAction = (StateItems)_indexOnAction;
        if (_indexOnActionEnd > 0)
            _actionPlace.stateAfterOnActionEnd = (StateItems)_indexOnActionEnd;

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_actionPlace);
    }
}