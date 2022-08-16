using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

[System.Serializable] 
public struct ViewPoint 
{
    public Vector3 position; 
    public Vector3 focusPoint; 


    public ViewPoint (Vector3 position, Vector3 focusPoint) 
    {
        this.position = position; 
        this.focusPoint = focusPoint; 
    }

    

    public Vector3 forward => focusPoint - position; 



    public Orientation GetOrientation () 
    {
        Vector3 forward = focusPoint - position; 
        return Orientation.FromForward(forward); 
    }
}

}
