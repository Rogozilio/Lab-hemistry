using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

public class InputAxis  
{
    float sensitivity = 3; 
    float gravity = 3; 

    public float value { get; private set; } 



	public InputAxis () {}  

    public InputAxis (float sensitivity, float gravity) 
    {
        this.sensitivity = sensitivity; 
        this.gravity = gravity; 
    }



    //  Interface  -------------------------------------------------- 
	float addedInput; 

	public void AddInput (float input) 
	{
		addedInput += input; 
	}

    public void Update () 
    {
        if (addedInput != 0) 
		{
			ApplyInput(addedInput); 
			addedInput = 0; 
		}
		else 
		{
			ApplyGravity(); 
		}
    }

    public void Reset () 
    {
        value = 0; 
    }



    //  Actions  ---------------------------------------------------- 
    void ApplyInput (float input) 
    {
        float motion = sensitivity * Mathf.Abs(input) * Time.deltaTime; 
        float destination = input > 0 ? 1 : -1; 

        value = Mathf.MoveTowards(value, destination, motion); 
    }

    void ApplyGravity () 
    {
        float motion = gravity * Time.deltaTime; 
        value = Mathf.MoveTowards(value, 0, motion); 
    }

}

}
