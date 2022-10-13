using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveMouseItem))]
public class MoveMouseItemEditor : Editor
{
    private bool _isUseBoundCollider = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var moveMouseItem = target as MoveMouseItem;
        if (moveMouseItem.IsMoveRigidbody)
        {
            _isUseBoundCollider = EditorGUILayout.Foldout(_isUseBoundCollider, "Bounds Extension", true);

            if (_isUseBoundCollider)
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
        }
    }
}
