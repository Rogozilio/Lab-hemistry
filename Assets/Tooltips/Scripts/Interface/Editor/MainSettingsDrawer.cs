using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace ERA.Tooltips.Core
{

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(MainSettings))]
public class MainSettingsEditor : PropertyDrawer
{
    const float VERTICAL_MARGIN = 3;


    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        position.y += VERTICAL_MARGIN / 2;
        position.height = EditorGUIUtility.singleLineHeight;

        DrawName(position, property);
    }

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight + VERTICAL_MARGIN;
    }


    void DrawName (Rect position, SerializedProperty property) 
    {
        SerializedProperty name = property.FindPropertyRelative("name");
        EditorGUI.PropertyField(position, name);
    }

}

#endif 

}