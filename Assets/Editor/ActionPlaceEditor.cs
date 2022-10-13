using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

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
