using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace ERA.Tooltips.Core
{

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(GeometrySettings))]
public class GeometrySettingsEditor : PropertyDrawer
{
    const float VERTICAL_MARGIN = 3;
    const int LINE_COUNT = 16;
    const int DIVIDER_LENGTH = 35;


    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty worldPoint            = property.FindPropertyRelative("worldPoint");
        SerializedProperty idealOffsetFromOrigin = property.FindPropertyRelative("idealOffsetFromOrigin");
        SerializedProperty maxDistanceFromOrigin = property.FindPropertyRelative("maxDistanceFromOrigin");
        SerializedProperty editModePosition      = property.FindPropertyRelative("editModePosition");
        SerializedProperty offsetScreenSpace     = property.FindPropertyRelative("offsetScreenSpace");
        SerializedProperty offsetWorldSpace      = property.FindPropertyRelative("offsetWorldSpace");
        SerializedProperty offsetLocalSpace      = property.FindPropertyRelative("offsetLocalSpace");
        SerializedProperty width                 = property.FindPropertyRelative("width");

        Rect area = position;
        area.y += VERTICAL_MARGIN / 2;

        area.y += DrawDivider(area, "Edit mode")            + VERTICAL_MARGIN;
        area.y += DrawProperty(area, editModePosition)         + VERTICAL_MARGIN;
        area.y += DrawDivider(area, "Tooltip origin")       + VERTICAL_MARGIN;
        area.y += DrawProperty(area, worldPoint)            + VERTICAL_MARGIN;
        area.y += DrawProperty(area, offsetScreenSpace)     + VERTICAL_MARGIN;
        area.y += DrawProperty(area, offsetWorldSpace)      + VERTICAL_MARGIN;
        area.y += DrawProperty(area, offsetLocalSpace)      + VERTICAL_MARGIN;
        area.y += DrawDivider(area, "Tooltip position")    + VERTICAL_MARGIN;
        area.y += DrawProperty(area, idealOffsetFromOrigin) + VERTICAL_MARGIN;
        area.y += DrawProperty(area, maxDistanceFromOrigin) + VERTICAL_MARGIN;
        area.y += DrawDivider(area, "UI")                   + VERTICAL_MARGIN;
        area.y += DrawProperty(area, width)                 + VERTICAL_MARGIN;
    }

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        return 
            LINE_COUNT * EditorGUIUtility.singleLineHeight + 
            LINE_COUNT * VERTICAL_MARGIN;
    }



    //  Tech  -------------------------------------------------------
    float lineHeight => EditorGUIUtility.singleLineHeight;

    float DrawDivider (Rect area, string text) 
    {
        if (text.Length % 2 == 1) text += " ";
        int spaceToAdd = DIVIDER_LENGTH - text.Length;
        int spaceOnOneSide = spaceToAdd / 2;

        string label = "";
        for (int i = 0; i < spaceOnOneSide - 2; i++) label += "-";
        for (int i = 0; i < 2; i++) label += " ";
        label += text;
        for (int i = 0; i < 2; i++) label += " ";
        for (int i = 0; i < spaceOnOneSide - 2; i++) label += "-";

        GUIStyle style = GUI.skin.label;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;

        area.height = 2 * lineHeight;
        GUI.Label(area, label, style);

        return area.height;
    }

    float DrawProperty (Rect area, SerializedProperty property) 
    {
        area.height = lineHeight;
        EditorGUI.PropertyField(area, property);
        return area.height;
    }

}

#endif 

}