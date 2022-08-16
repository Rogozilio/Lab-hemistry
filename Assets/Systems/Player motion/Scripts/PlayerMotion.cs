using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events; 



namespace VirtualLab.PlayerMotion 
{

public class PlayerMotion : MonoBehaviour 
{
	[SerializeField] Transform targetTransform; 
	[Space] 
	[SerializeField] PointMotion pointMotion; 
	[SerializeField] ViewRotation viewRotation; 
	[Space]
	[SerializeField] Navigation navigation; 
	[SerializeField] InputReciever inputReciever; 
	[SerializeField] InputActions inputActions; 
	[SerializeField] MotionEngine motionEngine; 
	[Space] 
	[SerializeField] int startPoint = 1; 




	void Awake () 
	{
		InitNavigation(); 
		InitMotionParts(); 
		InitSystemParts(); 
	}

	void Start () 
	{
		MoveToPoint(startPoint); 
	}

	void Update () 
	{
		inputActions.CreateActions(); 
		inputReciever.ResetInput(); 
		
		pointMotion.UpdateMe(); 
		viewRotation.UpdateMe(); 
		
		motionEngine.UpdateMe(); 
	}

	void OnValidate () 
	{
		startPoint = Mathf.Clamp(startPoint, 1, navigation.pointCount); 
	}



	//  Init  ------------------------------------------------------- 
	void InitNavigation () 
	{
		navigation.Init(startPoint); 
	}

	void InitMotionParts () 
	{
		pointMotion.Init(); 
		viewRotation.Init(targetTransform); 
	}

	void InitSystemParts () 
	{
		motionEngine.Init(targetTransform, pointMotion, viewRotation); 
		inputActions.Init(inputReciever, pointMotion, viewRotation, motionEngine); 
	}



	//  Events  ----------------------------------------------------- 
	public UnityEvent onMoveToPoint; 


	
	//  Input  ------------------------------------------------------ 
	public void MoveToNextPoint () 
	{
		int nextPoint = navigation.GetNextPointID(); 
		MoveToPoint(nextPoint); 
	}

	public void MoveToPreviousPoint () 
	{
		int previousPoint = navigation.GetPreviousPointID(); 
		MoveToPoint(previousPoint); 
	}

	public void MoveToPoint (int pointID) 
	{
		ViewPoint viewPoint = navigation.MoveToPoint(pointID); 
		inputReciever.moveToPoint.SetInput(viewPoint); 

		onMoveToPoint.Invoke(); 
	}

	public void Rotate (Orientation input) 
	{
		inputReciever.viewRotation.AddInput(input); 
	}



	//  Navigation  ------------------------------------------------- 
	public int semanticLocation 
	{
		get => navigation.location; 
	}

	public int GetNextPointID () 	 => navigation.GetNextPointID(); 
	public int GetPreviousPointID () => navigation.GetPreviousPointID(); 

	public bool IsPointAvailable (int pointID) 
	{
		return navigation.IsPointAvailable(pointID); 
	}

	public void SetPointAvailable (int pointID, bool available) 
	{
		navigation.SetPointAvailable(pointID, available); 
	}



	//  Info  ------------------------------------------------------- 
	public bool IsAtPoint (int point) 
    {
		ViewPoint viewPoint = navigation.GetViewPoint(point); 
        return pointMotion.IsAtPoint(viewPoint.position); 
    }

	public bool CanRotateLeft  => viewRotation.CanRotateLeft; 
    public bool CanRotateRight => viewRotation.CanRotateRight; 
    public bool CanRotateUp    => viewRotation.CanRotateUp; 
    public bool CanRotateDown  => viewRotation.CanRotateDown; 



	//  Reset  ------------------------------------------------------ 
	public void Reset () 
	{

	}

}

}

