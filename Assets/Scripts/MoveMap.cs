using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[Serializable]
public class ItemMoveMap
{
    public Transform target;
    public Vector3 offsetPos;
    public Quaternion offsetRot;
    public Vector3 offsetScale;
}


[Serializable]
public class ItemMap
{
    public ItemMoveMap move;
    public StateItems nextState;

    public ItemMap()
    {
        move = new ItemMoveMap();
    }
}

public class MoveMap : MonoBehaviour
{
    public List<ItemMap> datas;

    public void StartToMove(int index)
    {
        StartCoroutine(MoveToPoint(index));
    }

    private IEnumerator MoveToPoint(int index)
    {
        var data = datas[index].move;

        var distance = Vector3.Distance(transform.position, data.target.position + data.offsetPos);
        var angle = Quaternion.Angle(transform.rotation, data.target.rotation * data.offsetRot);

        while (distance > 0.01f || angle > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, data.target.position + data.offsetPos,
                distance * Time.fixedDeltaTime * 2f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, data.target.rotation * data.offsetRot,
                angle * Time.fixedDeltaTime * 2f);

            distance = Vector3.Distance(transform.position, data.target.position + data.offsetPos);
            angle = Quaternion.Angle(transform.rotation, data.target.rotation * data.offsetRot);

            yield return null;
        }

        GetComponent<StateItem>().ChangeState(datas[index].nextState);
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
                (Transform)EditorGUILayout.ObjectField("Target", _map.datas[i].move.target, typeof(Transform));
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
            GUILayout.EndVertical();
            GUILayout.Space(10f);
        }
    }
}