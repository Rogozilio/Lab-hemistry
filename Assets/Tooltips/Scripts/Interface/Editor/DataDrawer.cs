using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace ERA.Tooltips
{

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TooltipData))]
public class DataDrawer : PropertyDrawer
{
    const int VERTICAL_MARGIN = 5;
    const int TYPE_HEIGHT = 25;



    //  Life cycle  -------------------------------------------------
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
    {
        SerializedProperty type       = property.FindPropertyRelative("_type");
        SerializedProperty stringData = property.FindPropertyRelative("_stringData");
        SerializedProperty numberData = property.FindPropertyRelative("_numberData");

        Rect area = position;
        area.y += VERTICAL_MARGIN;

        area.y += DrawType(area, type)             + VERTICAL_MARGIN;
        area.y += DrawStringData(area, stringData) + VERTICAL_MARGIN;

        if (type.intValue == 2) 
        {
            area.y += DrawNumberData(area, numberData) + VERTICAL_MARGIN;
        }
    }



    //  Geometry  ---------------------------------------------------
    float lineHeight => EditorGUIUtility.singleLineHeight;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) 
    {
        SerializedProperty type = property.FindPropertyRelative("_type");
        bool hasNumberData = type.intValue == 2;

        return 
                                VERTICAL_MARGIN + TYPE_HEIGHT +             // type 
                                VERTICAL_MARGIN + lineHeight +              // string data 
            (hasNumberData ?   (VERTICAL_MARGIN + lineHeight) : 0) +        // number data 
                                VERTICAL_MARGIN;
    }



    //  Drawing  ----------------------------------------------------
    float DrawType (Rect area, SerializedProperty type) 
    {
        area.height = TYPE_HEIGHT;

        string [] labels = { "Title", "Text", "Progress" };
        type.intValue = GUI.Toolbar(area, type.intValue, labels);

        return area.height;
    }

    float DrawStringData (Rect area, SerializedProperty stringData) 
    {
        area.height = lineHeight;

        EditorGUI.PropertyField(area, stringData);

        return area.height;
    }

    float DrawNumberData (Rect area, SerializedProperty numberData) 
    {
        area.height = lineHeight;

        EditorGUI.PropertyField(area, numberData);

        return area.height;
    }

}
#endif 

}
