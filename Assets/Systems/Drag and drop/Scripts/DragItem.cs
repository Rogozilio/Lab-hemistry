using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events; 



namespace VirtualLab.DragAndDropNS 
{

public class DragItem : MonoBehaviour, IResettable 
{
	[SerializeField] float returnToOriginTime = 0.25f; 
	[SerializeField] float enterPlaceTime = 0.1f; 
	[SerializeField] float reachMousePointerTime = 0.1f; 
	[SerializeField] Events events; 
	public bool StopeMove { set; get; }


	void Awake () 
	{
		InitOriginalPoint();
	}

	//  Life cycle  ------------------------------------------------- 
	public void StartDrag (Vector3 contactPoint) 
	{
		CleanupPreviousDrag(); 
		InitOffset(contactPoint); 

		events.onDragStart.Invoke(); 
	}

	public void MoveTo (Vector3 position) 
	{
		if(!StopeMove)
			transform.position = ApplyOffset(position); 
		events.onDrag.Invoke(); 
	}

	public void DropToAir () 
	{
		GoToOriginalPoint(); 
		events.onDragEnd.Invoke(); 
	}

	public void DropToPlace (DropPlace place) 
	{
		EnterPlace(place); 
		events.onDragEnd.Invoke(); 
	}

	void CleanupPreviousDrag () 
	{
		StopAllCoroutines(); 
		if (place) LeavePlace(); 
	}



	//  Events  ----------------------------------------------------- 
	[System.Serializable] 
	public struct Events 
	{
		public UnityEvent onDragStart; 
		public UnityEvent onDrag; 
		public UnityEvent onDragEnd; 
	}



	//  Original point  --------------------------------------------- 
	Vector3 originalPoint; 

	void InitOriginalPoint () 
	{
		originalPoint = transform.position; 
	}

	void GoToOriginalPoint () 
	{
		StartMoveAnimation(originalPoint, returnToOriginTime); 
	}



	//  Offset  ----------------------------------------------------- 
	Vector3 offset; 
	float offsetVelocity; 

	void InitOffset (Vector3 contactPoint) 
	{
		offset = transform.position - contactPoint; 
		StartDecreasingOffset(); 
	}

	Vector3 ApplyOffset (Vector3 point) 
	{
		return point + offset; 
	}

	void StartDecreasingOffset () 
	{
		offsetVelocity = 0; 
		StartCoroutine(DecreaseOffset()); 
	}

	IEnumerator DecreaseOffset () 
	{
		while (offset.magnitude > 0.001f) 
		{
			float offsetValue = Mathf.SmoothDamp(
				offset.magnitude, 
				0, 
				ref offsetVelocity, 
				reachMousePointerTime 
			); 

			offset = offset.normalized * offsetValue; 

			yield return null; 
		}
	}



	//  Place  ------------------------------------------------------ 
	public DropPlace place { get; private set; } 

	void EnterPlace (DropPlace place) 
	{
		StartMoveAnimation(place.position, enterPlaceTime); 
		this.place = place; 

		place.OnItemEntered(this); 
	}

	void LeavePlace () 
	{
		place.OnItemLeft(); 
		place = null; 
	}



	//  Continious movement  ---------------------------------------- 
	Vector3 velocity; 

	void StartMoveAnimation (Vector3 target, float animationTime) 
	{
		velocity = new Vector3(); 
		StartCoroutine(MoveAnimation(target, animationTime)); 
	}

	IEnumerator MoveAnimation (Vector3 target, float animationTime) 
	{
		float distance = float.MaxValue; 

		while (distance > 0.0001f) 
		{
			transform.position = Vector3.SmoothDamp(
				transform.position, 
				target, 
				ref velocity, 
				animationTime 
			); 

			distance = Vector3.Distance(transform.position, target); 

			yield return null; 
		}
	}



	//  Reset  ------------------------------------------------------ 
	public void ResetMe () 
	{
		transform.position = originalPoint; 
	}

}

}
