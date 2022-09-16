using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public enum Operator
{
    [InspectorName("<")] Less,
    [InspectorName("<=")] LessEquals,
    [InspectorName("=")] Equally,
    [InspectorName(">=")] MoreEquals,
    [InspectorName(">")] More
}

public class ActionPlace : MonoBehaviour
{
    [Serializable]
    public struct Events
    {
        public UnityEvent onActionStart;
        public UnityEvent onAction;
        public UnityEvent onActionEnd;
    }

    [HideInInspector] [SerializeField] public StateItems stateAfterOnActionStart;
    [HideInInspector] [SerializeField] public StateItems stateAfterOnAction;
    [HideInInspector] [SerializeField] public StateItems stateAfterOnActionEnd;

    [HideInInspector] [SerializeField] public bool[] isChangeStateOnCondition;

    [HideInInspector] [SerializeField] public List<MonoBehaviour> startActionConditionScript;
    [HideInInspector] [SerializeField] public List<MonoBehaviour> actionConditionScript;
    [HideInInspector] [SerializeField] public List<MonoBehaviour> endActionConditionScript;

    [HideInInspector] [SerializeField] public List<int> indexPropertyStartAction;
    [HideInInspector] [SerializeField] public List<int> indexPropertyAction;
    [HideInInspector] [SerializeField] public List<int> indexPropertyEndAction;

    [HideInInspector] [SerializeField] public List<Operator> startActionOperators;
    [HideInInspector] [SerializeField] public List<Operator> actionOperators;
    [HideInInspector] [SerializeField] public List<Operator> endActionOperators;

    [HideInInspector] [SerializeField] public List<string> startActionValue;
    [HideInInspector] [SerializeField] public List<string> actionValue;
    [HideInInspector] [SerializeField] public List<string> endActionValue;

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
            if (!isChangeStateOnCondition[0])
            {
                _isActionStart = true;
                _stateItem?.ChangeState(stateAfterOnActionStart);
            }

            if (!IsCompliesCondition(startActionConditionScript, indexPropertyStartAction
                    , startActionOperators, startActionValue)) return;

            if (isChangeStateOnCondition[0])
            {
                _isActionStart = true;
                _stateItem?.ChangeState(stateAfterOnActionStart);
            }

            _events.onActionStart.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target && _isActionStart)
        {
            if (!isChangeStateOnCondition[1])
                _stateItem?.ChangeState(stateAfterOnAction);

            if (!IsCompliesCondition(actionConditionScript, indexPropertyAction
                    , actionOperators, actionValue)) return;

            if (isChangeStateOnCondition[1])
                _stateItem?.ChangeState(stateAfterOnAction);

            _events.onAction.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target && _isActionStart)
        {
            if (!isChangeStateOnCondition[1])
            {
                _isActionStart = false;
                _stateItem?.ChangeState(stateAfterOnActionEnd);
            }

            if (!IsCompliesCondition(endActionConditionScript, indexPropertyEndAction
                    , endActionOperators, endActionValue)) return;

            if (isChangeStateOnCondition[1])
            {
                _isActionStart = false;
                _stateItem?.ChangeState(stateAfterOnActionEnd);
            }

            _events.onActionEnd.Invoke();
        }
    }

    private bool IsCompliesCondition(List<MonoBehaviour> scripts, List<int> index, List<Operator> operators,
        List<string> values)
    {
        if (scripts.Count == 0) return true;

        var isCompletedAll = new bool[scripts.Count];

        for (var i = 0; i < scripts.Count; i++)
        {
            var property = scripts[i].GetType()
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            var type = property[index[i]].PropertyType.UnderlyingSystemType;


            var value = property[index[i]].GetValue(scripts[i]);

            if (type.BaseType == typeof(Enum))
                isCompletedAll[i] = IsValueCompleteCondition((int)value, operators[i], int.Parse(values[i]));
            if (type == typeof(bool))
                isCompletedAll[i] =
                    IsValueCompleteCondition((bool)value, operators[i], values[i] != "0");
            if (type == typeof(int))
                isCompletedAll[i] = IsValueCompleteCondition((int)value, operators[i], int.Parse(values[i]));
            if (type == typeof(float))
                isCompletedAll[i] = IsValueCompleteCondition((float)value, operators[i], float.Parse(values[i]));
        }

        foreach (var isCompleteCondition in isCompletedAll)
        {
            if (!isCompleteCondition) return false;
        }

        return true;
    }

    private bool IsValueCompleteCondition(bool value1, Operator op, bool value2)
    {
        switch (op)
        {
            case Operator.Equally: return value1 == value2;
        }

        return false;
    }

    private bool IsValueCompleteCondition(int value1, Operator op, int value2)
    {
        switch (op)
        {
            case Operator.Less: return value1 < value2;
            case Operator.LessEquals: return value1 <= value2;
            case Operator.Equally: return value1 == value2;
            case Operator.MoreEquals: return value1 >= value2;
            case Operator.More: return value1 > value2;
        }

        return false;
    }

    private bool IsValueCompleteCondition(float value1, Operator op, float value2)
    {
        switch (op)
        {
            case Operator.Less: return value1 < value2;
            case Operator.LessEquals: return value1 <= value2;
            case Operator.Equally: return value1 == value2;
            case Operator.MoreEquals: return value1 >= value2;
            case Operator.More: return value1 > value2;
        }

        return false;
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

    private bool _isShowConditions = true;

    private void OnEnable()
    {
        _actionPlace = (ActionPlace)target;

        if (_actionPlace.isChangeStateOnCondition == null)
            _actionPlace.isChangeStateOnCondition = new[] { false, false, false };
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _actionPlace = (ActionPlace)target;

        ShowConditions();

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Next State After \"OnActionStart\" = ", GUILayout.Width(200f));
        _actionPlace.stateAfterOnActionStart =
            (StateItems)EditorGUILayout.EnumPopup(_actionPlace.stateAfterOnActionStart, GUILayout.Width(130f));
        GUILayout.Label(" on ", GUILayout.Width(20f));
        _actionPlace.isChangeStateOnCondition[0] =
            EditorGUILayout.Toggle(_actionPlace.isChangeStateOnCondition[0], GUILayout.Width(40f));
        GUILayout.Label("condition", GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Next State for \"OnAction\" = ", GUILayout.Width(200f));
        _actionPlace.stateAfterOnAction =
            (StateItems)EditorGUILayout.EnumPopup(_actionPlace.stateAfterOnAction, GUILayout.Width(130f));
        GUILayout.Label(" on ", GUILayout.Width(20f));
        _actionPlace.isChangeStateOnCondition[1] =
            EditorGUILayout.Toggle(_actionPlace.isChangeStateOnCondition[1], GUILayout.Width(40f));
        GUILayout.Label("condition", GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Next State After \"OnActionEnd\" = ", GUILayout.Width(200f));
        _actionPlace.stateAfterOnActionEnd =
            (StateItems)EditorGUILayout.EnumPopup(_actionPlace.stateAfterOnActionEnd, GUILayout.Width(130f));
        GUILayout.Label(" on ", GUILayout.Width(20f));
        _actionPlace.isChangeStateOnCondition[2] =
            EditorGUILayout.Toggle(_actionPlace.isChangeStateOnCondition[2], GUILayout.Width(40f));
        GUILayout.Label("condition", GUILayout.ExpandWidth(true));
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

    private void ShowConditions()
    {
        _isShowConditions = EditorGUILayout.Foldout(_isShowConditions, "Conditions", true);

        if (!_isShowConditions) return;

        EditorGUI.indentLevel++;

        ShowCondition("Start Action Condition", ref _actionPlace.startActionConditionScript,
            ref _actionPlace.indexPropertyStartAction, ref _actionPlace.startActionOperators,
            ref _actionPlace.startActionValue);
        GUILayout.Space(10f);
        ShowCondition("Action Condition", ref _actionPlace.actionConditionScript,
            ref _actionPlace.indexPropertyAction, ref _actionPlace.actionOperators, ref _actionPlace.actionValue);
        GUILayout.Space(10f);
        ShowCondition("End Action Condition", ref _actionPlace.endActionConditionScript,
            ref _actionPlace.indexPropertyEndAction, ref _actionPlace.endActionOperators,
            ref _actionPlace.endActionValue);
    }

    private void ShowCondition(string name, ref List<MonoBehaviour> scripts, ref List<int> indexProperty,
        ref List<Operator> operators, ref List<string> values)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(name, EditorStyles.boldLabel);
        if (GUILayout.Button("Add condition"))
        {
            scripts ??= new List<MonoBehaviour>();
            indexProperty ??= new List<int>();
            operators ??= new List<Operator>();
            values ??= new List<string>();

            scripts.Add(null);
            indexProperty.Add(0);
            operators.Add(Operator.Equally);
            values.Add(null);
        }

        GUILayout.EndHorizontal();

        if(scripts != null)
            ShowScripts(ref scripts, ref indexProperty, ref operators, ref values);
    }

    private void ShowScripts(ref List<MonoBehaviour> scripts, ref List<int> indexProperty, ref List<Operator> operators,
        ref List<string> values)
    {
        for (var i = 0; i < scripts.Count; i++)
        {
            GUILayout.BeginHorizontal();

            scripts[i] =
                (MonoBehaviour)EditorGUILayout.ObjectField(scripts[i], typeof(MonoBehaviour), GUILayout.Width(150f));

            if (scripts[i] != null)
            {
                var property = scripts[i].GetType()
                    .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                List<string> optionsProperty = new List<string>();
                foreach (var prop in property)
                {
                    optionsProperty.Add(prop.Name);
                }

                if (property.Length > 0)
                {
                    ShowProperty(optionsProperty, ref indexProperty, i);
                    ShowOperator(ref operators, i);
                    ShowValue(property[indexProperty[i]].PropertyType, ref values, i);
                }
            }

            if (GUILayout.Button("X", GUILayout.Width(25f)))
            {
                scripts.RemoveAt(i);
                indexProperty.RemoveAt(i);
                operators.RemoveAt(i);
                values.RemoveAt(i);
                break;
            }

            GUILayout.EndHorizontal();
        }
    }

    private void ShowProperty(List<string> property, ref List<int> indexProperty, int i)
    {
        indexProperty[i] =
            EditorGUILayout.Popup(indexProperty[i], property.ToArray(), GUILayout.Width(150f));
    }

    private void ShowOperator(ref List<Operator> operators, int i)
    {
        operators[i] = (Operator)EditorGUILayout.EnumPopup(operators[i], GUILayout.Width(50f));
    }

    private void ShowValue(Type type, ref List<string> value, int i)
    {
        value[i] = EditorGUILayout.TextField(value[i]);
    }
}