using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.Animation 
{

[System.Serializable] 
public class LinearAnimation_AB : IAnimation_AB 
{
	LinearAnimation_01 animation01; 



	public LinearAnimation_AB () 
	{
		animation01 = new LinearAnimation_01(); 
	}



	//  Value  ------------------------------------------------------ 
	public float value 
	{
		get => Mathf.Lerp(A, B, animation01.value); 
		set {
			float distanceAtoValue = value - A; 
			float distance = B - A; 

			if (distance != 0) 
			{
				animation01.value = distanceAtoValue / distance; 
			}
			else 
			{
				animation01.value = 0; 
			}
		}
	}



	//  Target  ----------------------------------------------------- 
	public float A { get; set; } 
	public float B { get; set; } 

	public float target 
	{
		get => animation01.target == 0 ? A : B; 
	}

	public void GoToA ()
	{
		animation01.GoTo0(); 
	}

	public void GoToB ()
	{
		animation01.GoTo1(); 
	}



	//  Animation time  --------------------------------------------- 
	public float animationTime 
	{
		get => animation01.animationTime; 
		set => animation01.animationTime = value; 
	}



	//  Life cycle  ------------------------------------------------- 
	public bool active 
	{
		get => animation01.active; 
	}

	public void Update ()
	{
		animation01.Update(); 
	}

	public void Stop () 
	{
		animation01.Stop(); 
	}

	public void Resume () 
	{
		animation01.Resume(); 
	}

}

}
