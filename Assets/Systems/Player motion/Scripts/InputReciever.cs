using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class InputReciever : MonoBehaviour 
{

	//  Movement input  --------------------------------------------- 
	public MoveToPointInput moveToPoint = new MoveToPointInput(); 

	public class MoveToPointInput : InputData 
	{
		protected ViewPoint _targetPoint; 
		public ViewPoint targetPoint 
		{
			get {
				Check_HasInput(); 
				return _targetPoint; 
			}
		}

		public void SetInput (ViewPoint targetPoint) 
		{
			_targetPoint = targetPoint; 
			hasInput = true; 
		}

		public override void Reset () 
		{
			hasInput = false; 
		}
	}



	//  Rotation input  --------------------------------------------- 
	public ViewRotationInput viewRotation = new ViewRotationInput(); 

	public class ViewRotationInput : InputData 
	{
		protected Orientation _input; 
		public Orientation input 
		{
			get {
				Check_HasInput(); 
				return _input; 
			}
		}

		public void AddInput (Orientation input) 
		{
			_input += input; 
			hasInput = true; 
		}

		public override void Reset () 
		{
			_input = Orientation.forward; 
			hasInput = false; 
		}
	}



	//  Resetting  -------------------------------------------------- 
	public void ResetInput () 
	{
		moveToPoint.Reset(); 
		viewRotation.Reset(); 
	}



	//  Tech  ------------------------------------------------------- 
	public abstract class InputData 
	{
		public bool hasInput { get; protected set; } 
		
		protected void Check_HasInput () 
		{
			if (!hasInput) throw new UnityException("Trying to access input that doesn't exist"); 
		}

		public abstract void Reset (); 
	}

}

}
