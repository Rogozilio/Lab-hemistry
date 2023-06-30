using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips
{

public class FreeMotion : MonoBehaviour
{
	[SerializeField] float _speed           = 5; 
	[SerializeField] float rotationSpeed    = 90;
	[SerializeField] float mouseSensitivity = 1.5f; 
	[SerializeField] float shiftMultiplier  = 3; 
    [SerializeField] bool  useMouse            = true;
    [SerializeField] bool  useKeyboardRotation = true;
    [SerializeField] bool  useKeyboardMovement = true;

    

	void Update () 
	{
		UpdateMouseCursor(); 

		UpdateMovement(); 
		UpdateRotation(); 
	}



	//  Speed  ------------------------------------------------------ 
	float speed 
	{
		get {
			if (Input.GetKey(KeyCode.LeftShift)) 
			{
				return _speed * shiftMultiplier; 
			}
			else 
			{
				return _speed; 
			}
		}
	}



	//  Movement  --------------------------------------------------- 
	InputAxis inputAD = new InputAxis(); 
	InputAxis inputQE = new InputAxis(); 
	InputAxis inputSW = new InputAxis(); 

	void UpdateMovement () 
	{
		Vector3 input = useKeyboardMovement ? GetMoveInput() : Vector3.zero; 
		Move(input); 
	}

	Vector3 GetMoveInput () 
	{
		inputAD.AddInput(GetInput(KeyCode.A, KeyCode.D)); 
		inputQE.AddInput(GetInput(KeyCode.Q, KeyCode.E)); 
		inputSW.AddInput(GetInput(KeyCode.S, KeyCode.W)); 

		inputAD.Update(); 
		inputQE.Update(); 
		inputSW.Update(); 

		return new Vector3(
			inputAD.value, 
			inputQE.value, 
			inputSW.value 
		); 
	}

	void Move (Vector3 input) 
	{
		Vector3 velocity = speed * input; 
		Vector3 motion = velocity * Time.deltaTime; 

		transform.Translate(motion, Space.Self); 
	}



	//  Rotation  --------------------------------------------------- 
	InputAxis inputJL = new InputAxis(); 
	InputAxis inputKI = new InputAxis(); 

	void UpdateRotation () 
	{
		Vector2 speedInput  = useKeyboardRotation ? GetRotationInputKey() : Vector2.zero; 
		Vector2 anglesInput = useMouse ? GetRotationInputMouse() : Vector2.zero;
		Rotate(speedInput, anglesInput); 
	}

	Vector2 GetRotationInputKey () 
	{
		inputJL.AddInput(GetInput(KeyCode.J, KeyCode.L)); 
		inputKI.AddInput(GetInput(KeyCode.K, KeyCode.I)); 

		inputJL.Update(); 
		inputKI.Update(); 

		Vector2 input = new Vector2(
			- inputKI.value, 
			inputJL.value 
		);

		return input * Time.deltaTime; 
	}

	Vector2 GetRotationInputMouse () 
	{
		if (Input.GetMouseButton(1)) 
		{
			Vector2 input = new Vector2(
				- Input.GetAxis("Mouse Y"), 
				Input.GetAxis("Mouse X")
			); 

			input *= mouseSensitivity; 

			return input; 
		}
		else 
		{
			return Vector2.zero;
		}
	}

	void Rotate (Vector2 speedInput, Vector2 anglesInput) 
	{
		Vector2 angles = transform.eulerAngles; 

		Vector2 change = rotationSpeed * speedInput; 
		change += anglesInput;
		angles += change; 

		angles.x = ClampAngle(angles.x, 90); 
		
		transform.eulerAngles = angles; 
	}

	float ClampAngle (float angle, float maxAngle) 
	{
		angle = ToRange_minus180to180(angle); 
		angle = Mathf.Clamp(angle, - maxAngle, maxAngle); 
		return angle; 
	}

	float ToRange_minus180to180 (float angle) 
	{
		angle %= 360; 
		if (angle > 180) angle -= 360; 

		return angle; 
	}



	//  Mouse cursor  ----------------------------------------------- 
	void UpdateMouseCursor () 
	{
		if (Input.GetMouseButton(1)) 
			HideCursor(); 
		else 
			ShowCursor(); 
	}

	void ShowCursor () 
	{
		Cursor.visible = true; 
		Cursor.lockState = CursorLockMode.None; 
	}

	void HideCursor () 
	{
		Cursor.visible = false; 
		Cursor.lockState = CursorLockMode.Confined; 
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
