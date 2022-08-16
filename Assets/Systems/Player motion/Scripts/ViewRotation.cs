using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class ViewRotation : MonoBehaviour 
{
    // parameters 
    [SerializeField] float angularSpeed = 90; 
    [SerializeField] float acceleration = 3; 
    [SerializeField] float maxVerticalAngle = 90;     



    //  Init  ------------------------------------------------------- 
    public void Init (Transform targetTransform) 
	{
		InitInput(targetTransform); 
	} 

	void InitInput (Transform upsideDownSource) 
    {
		this.upsideDownSource = upsideDownSource; 

		inputX = new InputAxis(acceleration, acceleration); 
		inputY = new InputAxis(acceleration, acceleration); 
    }



    //  Current data  ----------------------------------------------- 
    Orientation orientation; 

    void InitData (Orientation orientation) 
    {
        this.orientation = orientation.normalized; 
    }



    //  Info  ------------------------------------------------------- 
    public bool CanRotateLeft  => true; 
    public bool CanRotateRight => true; 
    public bool CanRotateUp    => orientation.y <   maxVerticalAngle; 
    public bool CanRotateDown  => orientation.y > - maxVerticalAngle; 



    //  Input  ------------------------------------------------------ 
    InputAxis inputX; 
    InputAxis inputY; 
	Transform upsideDownSource; 

    public void Rotate (Orientation input) 
    {
		if (IsUpsideDown()) input.x = - input.x; 

		bool input_toRight = input.x > 0; 
		bool input_toLeft  = input.x < 0; 
		bool input_toUp    = input.y > 0; 
		bool input_toDown  = input.y < 0; 

        if (input_toRight && CanRotateRight) inputX.AddInput(input.x); 
        if (input_toLeft  && CanRotateLeft)  inputX.AddInput(input.x); 

        if (input_toUp    && CanRotateUp)    inputY.AddInput(input.y); 
        if (input_toDown  && CanRotateDown)  inputY.AddInput(input.y); 
    }

	bool IsUpsideDown () 
	{
		float dot = Vector3.Dot(upsideDownSource.up, Vector3.up); 
		return dot < 0; 
	}

	void UpdateInput () 
	{
		inputX.Update(); 
		inputY.Update(); 
	}

    void ResetInput () 
    {
        inputX.Reset(); 
        inputY.Reset(); 
	}



    //  Life cycle  ------------------------------------------------- 
    public void StartNewMotion (Orientation orientation) 
    {
        InitData(orientation); 
    }

    public void UpdateMe () 
    {
		UpdateInput(); 
        Rotate(); 
    }



	//  Rotation  --------------------------------------------------- 
    void Rotate () 
    {
        Orientation input = new Orientation(inputX.value, inputY.value, 0); 
        Orientation delta = input * angularSpeed * Time.deltaTime; 

        orientation += delta; 

        orientation.ClampVertical(- maxVerticalAngle, maxVerticalAngle); 
    }



	//  Output data  ------------------------------------------------ 
    public Orientation outOrientation => orientation; 

}

}
