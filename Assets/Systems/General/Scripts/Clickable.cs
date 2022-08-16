using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VirtualLab.OutlineNS; 



namespace VirtualLab 
{

public class Clickable : MonoBehaviour, IResettable 
{
	[SerializeField] Outline outline; 
	[SerializeField] bool interactableAtStart = true; 
	[SerializeField] UnityEvent onClick; 




	void Awake () 
	{
		interactable = interactableAtStart; 
	}



	bool _interactable; 
	public bool interactable 
	{
		get => _interactable; 
		set => _interactable = value; 
	}



	void OnMouseUp () 
	{
		if (interactable) onClick.Invoke(); 
	}

	void OnMouseEnter () 
	{
		if (interactable) outline.ShowOutline(); 
	}

	void OnMouseExit () 
	{
		if (interactable) outline.HideOutline(); 
	}



	public void ResetMe () 
	{
		interactable = interactableAtStart; 
	}

}

}
