using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class PointDriver 
{
    FloatSource motionTime; 



    public PointDriver (float motionTime) 
    {
        this.motionTime = new FloatSource(motionTime); 
    }

    public PointDriver (Func<float> moveTimeSource) 
    {
        this.motionTime = new FloatSource(moveTimeSource); 
    }



    //  Current data  ----------------------------------------------- 
    public Vector3 point; 



    //  Target  ----------------------------------------------------- 
    public Vector3 target; 



    //  Life cycle  ------------------------------------------------- 
    public void StartNewMotion (Vector3 point) 
    {
        StartNewMotion(point, point); 
    }

    public void StartNewMotion (Vector3 point, Vector3 target) 
    {
        this.point = point; 
        this.target = target; 

        InitMovement(); 
    }

    public void Update () 
    {
        MovePoint(); 
    }



    //  Movement  --------------------------------------------------- 
    Vector3 velocity; 

    void InitMovement () 
    {
        velocity = new Vector3(); 
    }

    void MovePoint () 
    {
        point = Vector3.SmoothDamp(
            point, 
            target, 
            ref velocity, 
            motionTime 
        );
    }

}

}
