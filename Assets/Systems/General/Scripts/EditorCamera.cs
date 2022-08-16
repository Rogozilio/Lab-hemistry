using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 



namespace VirtualLab 
{

#if UNITY_EDITOR 
public class EditorCamera 
{

    //  Camera  ----------------------------------------------------- 
    GameObject cameraObject; 
    Camera camera; 

    public void Create (Vector3 position, Vector3 viewDirection) 
    {
        CreateCameraObject(position, viewDirection); 
        CreateCamera(); 
    }

    public void Destroy () 
    {
        GameObject.DestroyImmediate(cameraObject); 
        cameraObject = null; 
    }

    void CreateCameraObject (Vector3 position, Vector3 viewDirection) 
    {
        if (cameraObject != null) Destroy(); 

        cameraObject = new GameObject("Camera object"); 
        cameraObject.transform.position = position; 
        cameraObject.transform.forward = viewDirection; 
    }

    void CreateCamera () 
    {
        camera = cameraObject.AddComponent<Camera>(); 
    }



    //  Rendering  -------------------------------------------------- 
    public void Render (Rect viewport) 
    {   
        CheckIfCanRender(); 

        Handles.ClearCamera(viewport, camera); 
        Handles.DrawCamera(viewport, camera, DrawCameraMode.Normal, true); 
    }

    void CheckIfCanRender () 
    {
        if (cameraObject == null) throw new UnityException(); 
    }



    //  Viewports  -------------------------------------------------- 
    public static Rect ViewportBottomRight (float width, float height, float margin) 
    {
        float x = Screen.width - margin - width; 
        float y = Screen.height - margin - height; 

        return new Rect (x, y, width, height); 
    }

}
#endif 

}
