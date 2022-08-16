using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEditor; 



namespace VirtualLab.Testing 
{

[System.Serializable] 
public class TestActionRepeat 
{
	[SerializeField] bool doAction; 

	public bool Read () 
	{
		bool value = doAction; 
		doAction = false; 

		return value; 
	}
}




#if UNITY_EDITOR 

[CustomPropertyDrawer(typeof(TestActionRepeat))] 
public class TestActionRepeatEditor : PropertyDrawer 
{
	const float BUTTON_RELATIVE_WIDTH = 0.5f; 
	const float BUTTON_HEIGHT = 30; 
	const float VERTICAL_MARGIN = 5; 



	public override void OnGUI (Rect space, SerializedProperty prop, GUIContent label) 
	{
		SerializedProperty doAction = prop.FindPropertyRelative("doAction"); 
		EditorGUI.BeginProperty(space, label, prop); 

		Rect buttonArea = CreateButtonArea(space); 
		doAction.boolValue = GUI.RepeatButton(buttonArea, prop.displayName); 

		EditorGUI.EndProperty(); 
	}

	Rect CreateButtonArea (Rect space) 
	{
		float width = space.width * BUTTON_RELATIVE_WIDTH; 
		float height = BUTTON_HEIGHT; 

		Rect rect = new Rect(
			space.center.x - width / 2, 
			space.center.y - height / 2, 
			width, 
			height 
		); 

		return rect; 
	}



	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return BUTTON_HEIGHT + 2 * VERTICAL_MARGIN; 
	}
}

#endif 

}
