using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace ERA.Tooltips.Core
{

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(TooltipDataList))]
public class DataListDrawer : PropertyDrawer
{

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty data = property.FindPropertyRelative("data");
        EditorGUI.PropertyField(position, data);
    }

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        SerializedProperty data = property.FindPropertyRelative("data");
        return EditorGUI.GetPropertyHeight(data, label);
    }

}

#endif 

}