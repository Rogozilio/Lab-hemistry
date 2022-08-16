using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class PlayerController : MonoBehaviour
{
	[SerializeField] PlayerMotion playerMotion; 


    
	void Update () 
	{
		CreatePointInput(); 
		CreateRotationInput(); 
	}



	//  Key input  -------------------------------------------------- 
	void CreatePointInput () 
	{
		if (Input.GetKeyDown(KeyCode.Q)) 
		{
			playerMotion.MoveToPreviousPoint(); 
		}

		if (Input.GetKeyDown(KeyCode.E)) 
		{
			playerMotion.MoveToNextPoint(); 
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) 
		{
			playerMotion.MoveToPoint(1); 
		}

		// if (Input.GetKeyDown(KeyCode.Alpha2)) 
		// {
		// 	playerMotion.MoveToPoint(2); 
		// }
	}

	void CreateRotationInput () 
    {
        float x = 0; 
        x += Input.GetKey(KeyCode.A) ? -1 : 0; 
        x += Input.GetKey(KeyCode.D) ?  1 : 0; 

        float y = 0; 
        y += Input.GetKey(KeyCode.S) ? -1 : 0; 
        y += Input.GetKey(KeyCode.W) ?  1 : 0; 

        Orientation input = new Orientation(x, y, 0); 
		playerMotion.Rotate(input); 
    }



	//  Tech  ------------------------------------------------------- 
	float GetInput (KeyCode negative, KeyCode positive) 
	{
		float input = 0; 
		input += Input.GetKey(negative) ? -1 : 0; 
		input += Input.GetKey(positive) ?  1 : 0; 
		return input; 
	}

}

}
