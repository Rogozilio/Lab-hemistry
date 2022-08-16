using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace VirtualLab.DragAndDropNS 
{

public class DropPlace : MonoBehaviour, IResettable 
{
	[SerializeField] Transform anchor; 
	[SerializeField] UnityEvent onItemEntered; 
	[SerializeField] UnityEvent onItemLeft; 



	//  Position  --------------------------------------------------- 
	public Vector3 position => anchor.position; 



	//  Item  ------------------------------------------------------- 
    public DragItem item { get; private set; } 

	public bool CanRecieveItem () 
	{
		return item == null; 
	}

	public void OnItemEntered (DragItem item) 
	{
		this.item = item; 
		onItemEntered.Invoke(); 
	}

	public void OnItemLeft () 
	{
		item = null; 
		onItemLeft.Invoke(); 
	}



	//  Reset  ------------------------------------------------------ 
	public void ResetMe () 
	{
		item = null; 
	}

}

}

