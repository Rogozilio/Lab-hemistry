using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class InputActions : MonoBehaviour 
{
	InputReciever inputReciever; 
	PointMotion pointMotion; 
	ViewRotation viewRotation; 
	MotionEngine motionEngine; 



	public void Init (
		InputReciever inputReciever, 
		PointMotion pointMotion, 
		ViewRotation viewRotation, 
		MotionEngine motionEngine 
	) {
		this.inputReciever = inputReciever; 
		this.pointMotion = pointMotion; 
		this.viewRotation = viewRotation; 
		this.motionEngine = motionEngine; 
	}



	//  Actions  ---------------------------------------------------- 
	public void CreateActions () 
	{
		if (inputReciever.moveToPoint.hasInput) 
		{
			MoveToPoint(inputReciever.moveToPoint.targetPoint); 
		}

		if (inputReciever.viewRotation.hasInput) 
		{
			Rotate(inputReciever.viewRotation.input); 
		}
	}

	void MoveToPoint (ViewPoint point) 
	{
		motionEngine.StartNewMotion(); 
		pointMotion.SetTarget(point); 
	}

	void Rotate (Orientation input) 
	{
		viewRotation.Rotate(input); 
	}

}

}
