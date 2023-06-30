using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace ERA.Tooltips.Core
{

#if UNITY_EDITOR

[CustomEditor(typeof(Tooltip))]
public class TooltipEditor : Editor
{
    SerializedProperty data;
    SerializedProperty mainSettings;
    SerializedProperty geometrySettings;
    SerializedProperty logicSettings;

    static int selectedTab = 0;



    void OnEnable () 
    {
        data = serializedObject.FindProperty("data");

        mainSettings     = serializedObject.FindProperty("mainSettings");
        geometrySettings = serializedObject.FindProperty("geometrySettings");
        logicSettings    = serializedObject.FindProperty("logicSettings");
    }



    //  Inspector GUI  ----------------------------------------------
    public override void OnInspectorGUI () 
    {
        serializedObject.Update();

        DrawMainSettings();
        DrawTabsBar();

        switch (selectedTab) 
        {
            case 0: DrawDataTab();     break;
            case 1: DrawSettingsTab(); break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DrawMainSettings ()
    {
        EditorGUILayout.PropertyField(mainSettings);
    }

    void DrawTabsBar () 
    {
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            selectedTab = GUILayout.Toolbar(
                selectedTab,
                new string[] {"Data", "Settings"},
                new GUILayoutOption[] { GUILayout.Width(250), GUILayout.Height(25) }
            );
            GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }

    void DrawDataTab () 
    {
        EditorGUILayout.PropertyField(data);
    }

    void DrawSettingsTab () 
    {
        EditorGUILayout.PropertyField(geometrySettings);
        EditorGUILayout.PropertyField(logicSettings);
    }



    //  Scene GUI  --------------------------------------------------
    TooltipSceneGUI sceneGUI = new TooltipSceneGUI();

    void OnSceneGUI () 
    {
        Tooltip tooltip = (Tooltip) target;

        if (tooltip.__settingsReady) 
        {
            sceneGUI.DrawGUI(tooltip);
        }
    }

}

#endif 

}