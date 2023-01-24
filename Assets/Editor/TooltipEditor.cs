using System;
using System.Linq;
using UnityEditor;


[CustomEditor(typeof(Tooltip))]
public class TooltipEditor : Editor
{
    private SerializedProperty index;
    private SerializedProperty offsetPosition;
    private void OnEnable()
    {
        index = serializedObject.FindProperty("index");
        offsetPosition = serializedObject.FindProperty("offsetPosition");
    }

    public override void OnInspectorGUI()
    {
        var tooltip = (Tooltip)target;
        
        index.intValue = EditorGUILayout.Popup("Tooltip", index.intValue, tooltip.Tooltips.Values.ToArray());
        EditorGUILayout.PropertyField(offsetPosition);
        serializedObject.ApplyModifiedProperties();
    }
}