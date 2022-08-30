using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 



namespace VirtualLab.OutlineNS 
{


#if UNITY_EDITOR || false 

[CustomEditor(typeof(OutlineOld))] [CanEditMultipleObjects] 
public class OutlineEditor : Editor 
{
	SerializedProperty autoLayer; 
	SerializedProperty outlineSettings; 
	SerializedProperty layer; 



	void OnEnable () 
	{
		autoLayer = serializedObject.FindProperty("autoOutlineLayer"); 
		outlineSettings = serializedObject.FindProperty("outlineSettings"); 
		layer = serializedObject.FindProperty("_outlineLayer"); 
	}

	public override void OnInspectorGUI () 
	{
		EditorGUI.BeginChangeCheck(); 

		autoLayer.boolValue = EditorGUILayout.Toggle(
			autoLayer.displayName, 
			autoLayer.boolValue
		); 

		if (autoLayer.boolValue) 
		{
			outlineSettings.objectReferenceValue = EditorGUILayout.ObjectField(
				outlineSettings.displayName, 
				outlineSettings.objectReferenceValue, 
				typeof(OutlineFeatureSettings), 
				true 
			); 
		}
		else 
		{
			layer.intValue = EditorGUILayout.IntField(
				layer.displayName, 
				layer.intValue
			); 
		}

		if (EditorGUI.EndChangeCheck()) 
		{
			serializedObject.ApplyModifiedProperties(); 
		}
	}

}

#endif 

}
