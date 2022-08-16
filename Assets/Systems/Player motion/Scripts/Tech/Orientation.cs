using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public struct Orientation 
{
    public float x;			// angle around Y axis, positive: right 
    public float y; 		// angle around X axis, positive: up 
	public float z; 		// angle around Z axis, positive: counter-clockwise when looking forward  



	public Orientation (float x, float y) 
	{
		this.x = x; 
		this.y = y; 
		this.z = 0; 
	}

	public Orientation (float x, float y, float z) 
	{
		this.x = x; 
		this.y = y; 
		this.z = z; 
	}



	//  Operations  ------------------------------------------------- 
	public void Normalize () 
	{
		x = ToRange_minus180to180(x); 
		y = ToRange_minus180to180(y); 
		z = ToRange_minus180to180(z); 
	}

	public Orientation normalized 
	{
		get {
			float x = this.x; 
			float y = this.y; 
			float z = this.z; 

			x = ToRange_minus180to180(x); 
			y = ToRange_minus180to180(y); 
			z = ToRange_minus180to180(z); 

			return new Orientation(x, y, z); 
		}
	}

	public void Clamp (float minAngle, float maxAngle) 
	{
		ClampHorizontal(minAngle, maxAngle); 
		ClampVertical(minAngle, maxAngle); 
	}

	public void ClampHorizontal (float minAngle, float maxAngle) 
	{
		x = Mathf.Clamp(x, - maxAngle, maxAngle); 
	}

	public void ClampVertical (float minAngle, float maxAngle) 
	{
		y = Mathf.Clamp(y, - maxAngle, maxAngle); 
	}



	//  Operators  -------------------------------------------------- 
	public static Orientation operator+ (Orientation A, Orientation B) 
	{
		return new Orientation(
			A.x + B.x, 
			A.y + B.y, 
			A.z + B.z 
		); 
	}

	public static Orientation operator* (Orientation A, float number) 
	{
		return new Orientation(
			number * A.x, 
			number * A.y, 
			number * A.z 
		); 
	}

	public static Orientation operator* (float number, Orientation A) 
	{
		return new Orientation(
			number * A.x, 
			number * A.y, 
			number * A.z 
		); 
	}



	//  To other types  --------------------------------------------- 
	public Vector3 eulerAngles 
	{
		get => new Vector3(-y, x, z); 
	}

	public Quaternion quaternion 
	{
		get => Quaternion.Euler(eulerAngles); 
	}



	//  Tech  ------------------------------------------------------- 
	static float ToRange_minus180to180 (float a) 
    {
        a %= 360; 
        if (a > 180) a -= 360; 

		return a; 
    }

	public override string ToString () 
	{
		return "(" + x + ", " + y + ", " + z + ")"; 
	}



	//  Factory  ---------------------------------------------------- 
	public static Orientation forward => new Orientation(0, 0, 0); 

	public static Orientation FromForward (Vector3 forward) 
	{
		Quaternion q = Quaternion.LookRotation(forward); 
		Vector3 angles = q.eulerAngles; 
		return FromAngles(angles); 
	}

	public static Orientation FromAngles (Vector3 eulerAngles) 
	{
		Orientation orientation = new Orientation(
			eulerAngles.y, 
			- eulerAngles.x, 
			eulerAngles.z 
		); 

		return orientation.normalized; 
	}

}

}
