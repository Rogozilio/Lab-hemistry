using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public class FollowWorldObject : MonoBehaviour
{
	[SerializeField] RectTransform objectToMove;
	[SerializeField] Transform _objectToFollow;
	[SerializeField] Vector3 offsetWorld;
	[SerializeField] Vector2 offsetScreen;

	new Camera camera;



	void Awake () 
	{
		camera = Camera.main;
	}

	void Update () 
	{
		if (objectToFollow) MoveToTargetPosition();
	}



	//  Object to follow  -------------------------------------------
	public Transform objectToFollow 
	{
		get => _objectToFollow;
		set 
		{
			_objectToFollow = value;
			if (enabled && objectToFollow) MoveToTargetPosition();
		}
	}



	//  Position  ---------------------------------------------------
	void MoveToTargetPosition () 
    {
		Vector3 worldPos = objectToFollow.position + offsetWorld;

        Vector3 screenPos = camera.WorldToScreenPoint(worldPos); 
		if (screenPos.z < 0) screenPos = Vector3.positiveInfinity;

        screenPos += (Vector3) offsetScreen; 

        objectToMove.anchoredPosition = screenPos; 
    }

}

}
