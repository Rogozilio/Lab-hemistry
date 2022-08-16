using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEditor; 



namespace VirtualLab.PlayerMotion 
{

public class NavPoint : MonoBehaviour 
{
    public Transform focusPoint; 
	public bool available = true; 


    public static explicit operator ViewPoint (NavPoint transformPoint) 
    {
        return new ViewPoint(
            transformPoint.transform.position, 
            transformPoint.focusPoint.position 
        ); 
    }
}





#if UNITY_EDITOR 
[CustomEditor(typeof(NavPoint))] 
public class NavPointEditor : Editor 
{

	void OnEnable () 
	{
		Tools.hidden = true; 
	}

	void OnDisable () 
	{
		Tools.hidden = false; 
	}

    void OnSceneGUI () 
    {
		MovePositionPoint(); 
		MoveFocusPoint(); 
		
		if (Event.current.type == EventType.Repaint) 
		{
			DrawPositionLabel(); 
			DrawFocusPointLabel(); 
			DrawCameraPreview(); 
		}
    }



    //  Current object  --------------------------------------------- 
    NavPoint navPoint       => (NavPoint) target; 
    Transform positionPoint => navPoint.transform; 
    Transform focusPoint    => navPoint.focusPoint.transform; 
    Vector3 viewDirection   => focusPoint.position - positionPoint.position; 



    //  Drawing labels  --------------------------------------------- 
    void DrawPositionLabel () 
    {
        DrawLabel(positionPoint, "Position point"); 
    }

    void DrawFocusPointLabel () 
    {
        DrawLabel(focusPoint, "Focus point"); 
    }

    void DrawLabel (Transform transform, string text) 
    {
        GUIStyle style = GetLabelStyle(text); 
        Handles.Label(transform.position, text, style); 
    }

    GUIStyle GetLabelStyle (string text) 
    {
        GUIStyle style = new GUIStyle(); 
        style.normal.textColor = Color.white; 

        float width = style.CalcSize(new GUIContent(text)).x; 
        style.contentOffset = new Vector2(
            - width / 2, 
            10 
        ); 

        return style; 
    }



    //  Movement  --------------------------------------------------- 
    void MovePositionPoint () 
    {
        Undo.RecordObject(positionPoint, "Moving position point"); 

        Vector3 newPos = Handles.PositionHandle(
            positionPoint.position, 
            positionPoint.rotation 
        ); 
		Vector3 change = newPos - positionPoint.position; 

		positionPoint.position += change; 
		focusPoint.position -= change; 
    }

    void MoveFocusPoint () 
    {
        Undo.RecordObject(focusPoint, "Moving focus point"); 

        focusPoint.position = Handles.PositionHandle(
            focusPoint.position, 
            focusPoint.rotation 
        ); 
    }



    //  Camera preview  --------------------------------------------- 
    EditorCamera editorCamera = new EditorCamera(); 

    void DrawCameraPreview () 
    {
        try {
            editorCamera.Create(positionPoint.position, viewDirection); 

            Rect viewport = EditorCamera.ViewportBottomRight(500, 500 * 9 / 16, 25); 
            editorCamera.Render(viewport); 
        }
        finally 
        {
            editorCamera.Destroy(); 
        }
    }

}
#endif 

}
