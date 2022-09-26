using System;
using System.Collections;
using System.Collections.Generic;
using GD.MinMaxSlider;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[Serializable]
public class ItemMoveMap
{
    public Transform target;
    public Vector3 offsetPos;
    public Quaternion offsetRot;
    public Vector3 offsetScale;
}


[Serializable]
public struct LinearValue
{
    public Axis axisInput;
    public Axis axis;
    public Vector2 edge;
}

[Serializable]
public class ItemMap
{
    public ItemMoveMap move;
    public StateItems nextState;

    public LinearValue linearMoveValue;

    public LinearValue linearRotateValue;

    public UnityEvent onEventInEnd;

    public ItemMap()
    {
        move = new ItemMoveMap();
    }
}

public class MoveMap : MonoBehaviour
{
    public List<ItemMap> datas;

    private StateItem _stateItem;
    private MoveToPoint _moveToMapPoint;

    public void StartToMove(int index)
    {
        _stateItem = GetComponent<StateItem>();
        var data = datas[index].move;

        _moveToMapPoint = new MoveToPoint(transform, data.target.position + data.offsetPos,
            data.target.rotation * data.offsetRot);
        StartCoroutine(MoveToPoint(index));
    }

    private IEnumerator MoveToPoint(int index)
    {
        while (_moveToMapPoint.Distance > 0.01f || _moveToMapPoint.Angle > 0.01f)
        {
            if (_stateItem.State == StateItems.Idle)
                yield break;

            _moveToMapPoint.Start(2.5f);

            yield return null;
        }

        switch (datas[index].nextState)
        {
            case StateItems.LinearMove:
                _stateItem.ChangeState(datas[index].nextState, datas[index].linearMoveValue);
                break;
            case StateItems.LinearRotate:
                _stateItem.ChangeState(datas[index].nextState, datas[index].linearRotateValue);
                break;
            default:
                _stateItem.ChangeState(datas[index].nextState);
                break;
        }
        
        datas[index].onEventInEnd?.Invoke();
    }
}

[CustomEditor(typeof(MoveMap))]
[CanEditMultipleObjects]
public class MoveMapEditor : Editor
{
    private MoveMap _map;

    private void OnEnable()
    {
        _map = (MoveMap)target;
    }

    public override void OnInspectorGUI()
    {
        _map = (MoveMap)target;

        Show();

        if (GUILayout.Button("Add item"))
        {
            if (_map.datas == null)
            {
                _map.datas = new List<ItemMap>();
            }

            _map.datas.Add(new ItemMap());
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_map);
    }

    private void Show()
    {
        if (_map.datas == null) return;

        for (int i = 0; i < _map.datas.Count; i++)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            _map.datas[i].move.target =
                (Transform)EditorGUILayout.ObjectField("Target", _map.datas[i].move.target, typeof(Transform), true);
            if (GUILayout.Button("X", GUILayout.Width(25f)))
            {
                _map.datas.RemoveAt(i);
                break;
            }

            GUILayout.EndHorizontal();
            _map.datas[i].move.offsetPos =
                EditorGUILayout.Vector3Field("Offset Position", _map.datas[i].move.offsetPos);
            _map.datas[i].move.offsetRot.eulerAngles =
                EditorGUILayout.Vector3Field("Offset Rotation", _map.datas[i].move.offsetRot.eulerAngles);
            _map.datas[i].move.offsetScale =
                EditorGUILayout.Vector3Field("Offset Scale", _map.datas[i].move.offsetScale);
            GUILayout.BeginHorizontal();
            _map.datas[i].nextState =
                (StateItems)EditorGUILayout.EnumPopup("ChangeStateInEnd", _map.datas[i].nextState);
            GUILayout.EndHorizontal();

            var space = 20f;

            if (_map.datas[i].nextState == StateItems.LinearMove)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(space);
                EditorGUILayout.LabelField("AxisInput", GUILayout.Width(EditorGUIUtility.labelWidth - space));
                _map.datas[i].linearMoveValue.axisInput =
                    (Axis)EditorGUILayout.EnumPopup(_map.datas[i].linearMoveValue.axisInput);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(space);
                EditorGUILayout.LabelField("Axis", GUILayout.Width(EditorGUIUtility.labelWidth - space));
                _map.datas[i].linearMoveValue.axis =
                    (Axis)EditorGUILayout.EnumPopup(_map.datas[i].linearMoveValue.axis);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(space);
                EditorGUILayout.LabelField("EdgeMove", GUILayout.Width(EditorGUIUtility.labelWidth - space));
                _map.datas[i].linearMoveValue.edge.x = EditorGUILayout.FloatField(_map.datas[i].linearMoveValue.edge.x,
                    GUILayout.Width(45f));
                EditorGUILayout.MinMaxSlider(ref _map.datas[i].linearMoveValue.edge.x,
                    ref _map.datas[i].linearMoveValue.edge.y, -100, 100);
                _map.datas[i].linearMoveValue.edge.y = EditorGUILayout.FloatField(_map.datas[i].linearMoveValue.edge.y,
                    GUILayout.Width(45f));
                GUILayout.EndHorizontal();
            }

            if (_map.datas[i].nextState == StateItems.LinearRotate)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(space);
                EditorGUILayout.LabelField("AxisInput", GUILayout.Width(EditorGUIUtility.labelWidth - space));
                _map.datas[i].linearRotateValue.axisInput =
                    (Axis)EditorGUILayout.EnumPopup(_map.datas[i].linearRotateValue.axisInput);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(space);
                EditorGUILayout.LabelField("Axis", GUILayout.Width(EditorGUIUtility.labelWidth - space));
                _map.datas[i].linearRotateValue.axis =
                    (Axis)EditorGUILayout.EnumPopup(_map.datas[i].linearRotateValue.axis);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(space);
                EditorGUILayout.LabelField("EdgeMove", GUILayout.Width(EditorGUIUtility.labelWidth - space));
                _map.datas[i].linearRotateValue.edge.x =
                    EditorGUILayout.FloatField(_map.datas[i].linearRotateValue.edge.x, GUILayout.Width(45f));
                EditorGUILayout.MinMaxSlider(ref _map.datas[i].linearRotateValue.edge.x,
                    ref _map.datas[i].linearRotateValue.edge.y, 0f, 360f);
                _map.datas[i].linearRotateValue.edge.y =
                    EditorGUILayout.FloatField(_map.datas[i].linearRotateValue.edge.y, GUILayout.Width(45f));
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("datas").GetArrayElementAtIndex(i).FindPropertyRelative("onEventInEnd"));

            GUILayout.EndVertical();
            GUILayout.Space(10f);
        }
    }
}