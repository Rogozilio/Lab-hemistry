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

    private bool _isActionStart;
    private StateItem _stateItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target 
            && (stateStart == StateItems.Default 
                || target.TryGetComponent(out _stateItem)
                && _stateItem.State == stateStart))
        {
            _isActionStart = true;
            _events.onActionStart.Invoke();
            _stateItem?.ChangeState(stateAfterOnActionStart);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target && _isActionStart)
        {
            _events.onAction.Invoke();
            _stateItem?.ChangeState(stateAfterOnAction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target && _isActionStart)
        {
            _isActionStart = false;
            _events.onActionEnd.Invoke();
            _stateItem?.ChangeState(stateAfterOnActionEnd);
        }
    }
}

[CustomEditor(typeof(ActionPlace))]
[CanEditMultipleObjects]
public class ActionPlaceEditor : Editor
{
    private ActionPlace _actionPlace;

    private int _indexOnActionStart;
    private int _indexOnAction;
    private int _indexOnActionEnd;

    private void OnEnable()
    {
        _actionPlace = (ActionPlace)target;
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