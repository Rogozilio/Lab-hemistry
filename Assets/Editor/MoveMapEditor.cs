using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


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
                    (AxisInput)EditorGUILayout.EnumPopup(_map.datas[i].linearMoveValue.axisInput);
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
                    (AxisInput)EditorGUILayout.EnumPopup(_map.datas[i].linearRotateValue.axisInput);
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
                    ref _map.datas[i].linearRotateValue.edge.y, -180f, 180f);
                _map.datas[i].linearRotateValue.edge.y =
                    EditorGUILayout.FloatField(_map.datas[i].linearRotateValue.edge.y, GUILayout.Width(45f));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                _map.datas[i].offsetPosition =
                    EditorGUILayout.Vector3Field("Offset position", _map.datas[i].offsetPosition);
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("datas").GetArrayElementAtIndex(i)
                .FindPropertyRelative("onEventInEnd"));

            GUILayout.EndVertical();
            GUILayout.Space(10f);
        }
    }
}