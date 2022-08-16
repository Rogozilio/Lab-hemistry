using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class MotionEngine : MonoBehaviour 
{
	Transform targetTransform; 
	PointMotion pointMotion; 
	ViewRotation viewRotation; 



	public void Init (
		Transform targetTransform, 
		PointMotion pointMotion, 
		ViewRotation viewRotation 
	) {
		this.targetTransform = targetTransform; 
		this.pointMotion = pointMotion; 
		this.viewRotation = viewRotation; 
	}



	//  Start new motion  ------------------------------------------- 
	public void StartNewMotion () 
	{
		StartPointMotion(); 
		StartViewRotation(); 
	}

	void StartPointMotion () 
	{
		Vector3 position = targetTransform.position; 
		Orientation orientation = Orientation.FromAngles(targetTransform.eulerAngles); 

		pointMotion.StartNewMotion(position, orientation); 
	}
	
	void StartViewRotation () 
	{
		viewRotation.StartNewMotion(Orientation.forward); 
	}



	//  Updating  --------------------------------------------------- 
	public void UpdateMe () 
	{
		Rotate(); 
		Move(); 
	}



	//  Movement  --------------------------------------------------- 
	void Move () 
    {
		targetTransform.position = pointMotion.outPosition; 
    }



	//  Rotation  --------------------------------------------------- 
    void Rotate () 
    {
        Vector3 angles = 
			pointMotion.outOrientation.eulerAngles + 
			viewRotation.outOrientation.eulerAngles; 

        targetTransform.eulerAngles = angles; 
    }



	//  Reset  ------------------------------------------------------ 
	public void Reset () 
	{
		
	}

}

}
