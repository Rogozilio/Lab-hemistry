using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveMouseItem))]
public class MoveMouseItemEditor : Editor
{
    private SerializedProperty _onMouseDown;
    private SerializedProperty _onMouseUp;
    private SerializedProperty _outlineMapOffset;
    private SerializedProperty _outlineMapItem;

    private void OnEnable()
    {
        _onMouseDown = serializedObject.FindProperty("OnMouseDown");
        _onMouseUp = serializedObject.FindProperty("OnMouseUp");
        _outlineMapItem = serializedObject.FindProperty("outlineMap").FindPropertyRelative("outlineMapItem");
        _outlineMapOffset = serializedObject.FindProperty("outlineMap").FindPropertyRelative("offsetPoint");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var moveMouseItem = target as MoveMouseItem;

        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Is Move Rigidbody", GUILayout.Width(EditorGUIUtility.labelWidth));
        moveMouseItem.IsMoveRigidbody = EditorGUILayout.Toggle(moveMouseItem.IsMoveRigidbody);
        GUILayout.EndHorizontal();

        if (moveMouseItem.IsMoveRigidbody)
        {
            var width = 15f;
            var space = 20f;
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            GUILayout.Label("Use Extents", GUILayout.Width(EditorGUIUtility.labelWidth - space));
            moveMouseItem.IsExtentsX = EditorGUILayout.Toggle(moveMouseItem.IsExtentsX, GUILayout.Width(width));
            GUILayout.Label("X", GUILayout.Width(width));
            moveMouseItem.IsExtentsY = EditorGUILayout.Toggle(moveMouseItem.IsExtentsY, GUILayout.Width(width));
            GUILayout.Label("Y", GUILayout.Width(width));
            moveMouseItem.IsExtentsZ = EditorGUILayout.Toggle(moveMouseItem.IsExtentsZ, GUILayout.Width(width));
            GUILayout.Label("Z", GUILayout.Width(width));
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Is Rotate To Camera", GUILayout.Width(EditorGUIUtility.labelWidth));
        moveMouseItem.IsRotateToCamera = EditorGUILayout.Toggle(moveMouseItem.IsRotateToCamera);
        GUILayout.EndHorizontal();

        if (moveMouseItem.IsRotateToCamera)
        {
            var width = 15f;
            var space = 20f;

            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            GUILayout.Label("Face axis", GUILayout.Width(EditorGUIUtility.labelWidth - space));
            moveMouseItem.IsRight = EditorGUILayout.Toggle(moveMouseItem.IsRight, GUILayout.Width(width));
            GUILayout.Label("R", GUILayout.Width(width));
            moveMouseItem.IsUp = EditorGUILayout.Toggle(moveMouseItem.IsUp, GUILayout.Width(width));
            GUILayout.Label("U", GUILayout.Width(width));
            moveMouseItem.IsForward = EditorGUILayout.Toggle(moveMouseItem.IsForward, GUILayout.Width(width));
            GUILayout.Label("F", GUILayout.Width(width));
            GUILayout.Space(3f);
            moveMouseItem.IsInverse = EditorGUILayout.Toggle(moveMouseItem.IsInverse, GUILayout.Width(width));
            GUILayout.Label("Inverse");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            GUILayout.Label("Lock axis", GUILayout.Width(EditorGUIUtility.labelWidth - space));
            moveMouseItem.IsLockX = EditorGUILayout.Toggle(moveMouseItem.IsLockX, GUILayout.Width(width));
            GUILayout.Label("X", GUILayout.Width(width));
            moveMouseItem.IsLockY = EditorGUILayout.Toggle(moveMouseItem.IsLockY, GUILayout.Width(width));
            GUILayout.Label("Y", GUILayout.Width(width));
            moveMouseItem.IsLockZ = EditorGUILayout.Toggle(moveMouseItem.IsLockZ, GUILayout.Width(width));
            GUILayout.Label("Z", GUILayout.Width(width));
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10f);
        serializedObject.Update();
        EditorGUILayout.PropertyField(_onMouseDown);
        EditorGUILayout.PropertyField(_onMouseUp);
        GUILayout.Space(10f);

        EditorGUILayout.PropertyField(_outlineMapOffset);
        for (var i = 0; i < _outlineMapItem.arraySize; i++)
        {
            var item = _outlineMapItem.GetArrayElementAtIndex(i);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(item.FindPropertyRelative("value"), GUIContent.none);
            if (GUILayout.Button("X", GUILayout.Width(20f)))
            {
                _outlineMapItem.DeleteArrayElementAtIndex(i);
                break;
            }

            if (GUILayout.Button("Add Condition", GUILayout.Width(120f)))
            {
                item.FindPropertyRelative("conditions").arraySize++;
                break;
            }

            EditorGUILayout.EndHorizontal();

            var conditions = item.FindPropertyRelative("conditions");

            for (var j = 0; j < conditions.arraySize; j++)
            {
                var condition = conditions.GetArrayElementAtIndex(j);
                var target = condition.FindPropertyRelative("target");
                var component = condition.FindPropertyRelative("component");
                var nameProperty = condition.FindPropertyRelative("nameProperty");
                var indexComponent = condition.FindPropertyRelative("indexComponent");
                var indexProperty = condition.FindPropertyRelative("indexProperty");
                var sign = condition.FindPropertyRelative("sign");
                var requiredValue = condition.FindPropertyRelative("requiredValue");
                    
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(18f);
                EditorGUILayout.PropertyField(target, GUIContent.none);

                if (target.objectReferenceValue != null)
                {
                    var GO = (GameObject)target.objectReferenceValue;

                    indexComponent.intValue = EditorGUILayout.Popup(indexComponent.intValue, GetNamesFromComponents(GO.GetComponents<Component>()));
                    component.objectReferenceValue = GO.GetComponents<Component>()[indexComponent.intValue];

                    if (component.objectReferenceValue != null)
                    {
                        var properties = component.objectReferenceValue.GetType()
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        var namesProperty = GetNamesFromProperty(properties);
                        indexProperty.intValue = EditorGUILayout.Popup(indexProperty.intValue, namesProperty);
                        nameProperty.stringValue = namesProperty[indexProperty.intValue];
                        
                        EditorGUILayout.PropertyField(sign, GUIContent.none, GUILayout.Width(40f));
                        EditorGUILayout.PropertyField(requiredValue, GUIContent.none);
                    }
                }
                
                if (GUILayout.Button("X", GUILayout.Width(20f)))
                {
                    conditions.DeleteArrayElementAtIndex(j);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        if (GUILayout.Button("Add Outline Item"))
        {
            _outlineMapItem.arraySize++;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private string[] GetNamesFromComponents(Component[] array)
    {
        var names = new List<string>();
        for (int i = 0; i < array.Length; i++)
        {
            names.Add(i + " " + array[i].GetType().Name);
        }
        return names.ToArray();
    }
    
    private string[] GetNamesFromProperty(PropertyInfo[] array)
    {
        var names = new List<string>();
        foreach (var value in array)
        {
            names.Add(value.Name);
        }
        return names.ToArray();
    }
}