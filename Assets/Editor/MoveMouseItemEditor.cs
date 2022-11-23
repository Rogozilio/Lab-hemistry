using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveMouseItem))]
public class MoveMouseItemEditor : Editor
{
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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OnMouseDown"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OnMouseUp"));
        serializedObject.ApplyModifiedProperties();
    }
}