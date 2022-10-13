using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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