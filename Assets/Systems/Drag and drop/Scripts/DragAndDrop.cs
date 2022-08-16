using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.DragAndDropNS
{

public class DragAndDrop : MonoBehaviour
{
	[SerializeField] LookingForItem lookingForItem; 
	[SerializeField] MovingItem movingItem; 
	[SerializeField] SelectedObjects selectedObjects; 

	public DragItem item { get; private set; } 



	void Update ()
	{
		selectedObjects.UpdateObjects(); 

		if (!hasItem) lookingForItem.LookForItem(); 
		else 		  movingItem.MoveItem(); 
	}



	//  Info  ------------------------------------------------------- 
	public bool hasItem => item != null; 



	//  Callbacks  -------------------------------------------------- 
	public void OnItemFound (DragItem item, Vector3 contactPoint) 
	{
		this.item = item; 
		movingItem.PickUpItem(item, contactPoint); 
	}

	public void OnItemDropped () 
	{
		this.item = null; 
	}


}

}
