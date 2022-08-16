using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.Animation 
{

public class LinearAnimation_01 : IAnimation_01
{
	//  Value  ------------------------------------------------------ 
	public float value { get; set; } = 0; 



	//  Target  ----------------------------------------------------- 
	public float target { get; private set; } = 0; 

	public void GoTo0 ()
	{
		target = 0; 
	}

	public void GoTo1 ()
	{
		target = 1; 
	}



	//  Animation time  --------------------------------------------- 
	float speed = 1; 

	public float animationTime 
	{
		get => 1 / speed; 
		set => speed = 1 / value; 
	}



	//  Life cycle  ------------------------------------------------- 
	public bool active { get; private set; } = true; 

	public void Update ()
	{
		if (active) 
		{
			value = Mathf.MoveTowards(
				value, 
				target, 
				speed * Time.deltaTime
			); 
		}
	}

	public void Stop () 
	{
		active = false; 
	}

	public void Resume () 
	{
		active = true; 
	}

}

}
