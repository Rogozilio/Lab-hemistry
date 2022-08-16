using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class RotationDriver 
{
	FloatSource rotationTime; 

	Rotation current = new Rotation(); 
	Rotation target = new Rotation(); 
	Vector3 velocity; 



	public RotationDriver (FloatSource rotationTime) 
	{
		this.rotationTime = rotationTime; 
	}



	//  Data access  ------------------------------------------------ 
	public Vector3 currentForward 
	{
		get => current.forward; 
		set => current.forward = value; 
	}

	public Quaternion currentRotation 
	{
		get => current.rotation; 
		set => current.rotation = value; 
	}

	public Orientation currentOrientation 
	{
		get => current.orientation; 
		set => current.orientation = value; 
	}

	public Vector3 targetForward 
	{
		get => target.forward; 
		set => target.forward = value; 
	}

	public Quaternion targetRotation 
	{
		get => target.rotation; 
		set => target.rotation = value; 
	}

	public Orientation targetOrientation 
	{
		get => target.orientation; 
		set => target.orientation = value; 
	}



	//  Life cycle  ------------------------------------------------- 
	public void StartNewMotion (Quaternion currentRotation) 
	{
		current.rotation = currentRotation; 
		target.Clear(); 
	}

	public void StartNewMotion (Quaternion currentRotation, Quaternion targetRotation) 
	{
		current.rotation = currentRotation; 
		target.rotation = targetRotation; 
	}

	public void Update () 
	{
		Move(); 
	}



	//  Motion  ----------------------------------------------------- 
	void Move () 
	{
		if (current.hasData && target.hasData) 
		{
			current.angles = new Vector3(
				MoveAngle(current.angles.x, target.angles.x, ref velocity.x), 
				MoveAngle(current.angles.y, target.angles.y, ref velocity.y), 
				MoveAngle(current.angles.z, target.angles.z, ref velocity.z) 
			); 
		}
	}

	float MoveAngle (float current, float target, ref float velocity) 
	{
		return Mathf.SmoothDampAngle(current, target, ref velocity, rotationTime); 
	}



	//  Tech  ------------------------------------------------------- 
	static Vector3 GetForwardVector (Quaternion rotation) 
	{
		return rotation * Vector3.forward; 
	}



	//  Classes  ---------------------------------------------------- 
	class Rotation 
	{
		public bool hasData { get; private set; } 
		Vector3 _angles; 

		public Vector3 angles 
		{
			get => _angles; 
			set {
				_angles = value; 
				hasData = true; 
			}
		} 

		public Vector3 forward 
		{
			get {
				Quaternion rotation = Quaternion.Euler(angles); 
				Vector3 forward = RotationDriver.GetForwardVector(rotation); 
				return forward; 
			}
			set {
				rotation = Quaternion.LookRotation(value); 
			}
		}

		public Quaternion rotation 
		{
			get {
				return Quaternion.Euler(angles); 
			}
			set {
				angles = value.eulerAngles; 
			}
		}

		public Orientation orientation 
		{
			get => Orientation.FromAngles(angles); 
			set => angles = value.eulerAngles; 
		}

		public void Clear () 
		{
			hasData = false; 
		}
	}

}

}

