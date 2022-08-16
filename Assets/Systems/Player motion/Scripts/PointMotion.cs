using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class PointMotion : MonoBehaviour 
{
    [SerializeField] float timeToReachPoint = 0.5f; 
    [SerializeField] float atPointMargin = 0.025f; 
    [SerializeField] float almostAtPointMargin = 0.1f; 

    PointDriver positionDriver; 
    RotationDriver rotationDriver; 



    //  Init  ------------------------------------------------------- 
    public void Init () 
    {
        CreatePositionDriver(); 
        CreateRotationDriver(); 
    }

    void CreatePositionDriver () 
    {
        positionDriver = new PointDriver(
            () => timeToReachPoint 
        ); 
    }

    void CreateRotationDriver () 
    {
        rotationDriver = new RotationDriver(
            new FloatSource(
                () => timeToReachPoint 
            ) 
        ); 
    }



    //  Info  ------------------------------------------------------- 
    public bool IsAtPoint (Vector3 point) 
    {
        return GetDistance(point) <= atPointMargin; 
    }

    public bool IsAlmostAtPoint (Vector3 point) 
    {
        return GetDistance(point) <= almostAtPointMargin; 
    }

    public bool HasAlmostReachedTarget () 
    {
        return IsAtPoint(positionDriver.target); 
    }

    float GetDistance (Vector3 point) 
    {
        Vector3 position = positionDriver.point; 
        float distance = Vector3.Distance(position, point); 
        return distance; 
    }



    //  Input  ------------------------------------------------------ 
    public void SetTarget (ViewPoint target) 
    {
        positionDriver.target = target.position; 
        rotationDriver.targetForward = target.forward; 
    }



    //  Life cycle  ------------------------------------------------- 
    public void StartNewMotion (Vector3 position, Orientation orientation) 
    {
        positionDriver.StartNewMotion(position); 
        rotationDriver.StartNewMotion(orientation.quaternion); 
    }

    public void UpdateMe () 
    {
        positionDriver.Update(); 
        rotationDriver.Update(); 
    }



	//  Output data  ------------------------------------------------ 
	public Vector3     outPosition    => positionDriver.point; 
	public Orientation outOrientation => rotationDriver.currentOrientation; 

}

}
