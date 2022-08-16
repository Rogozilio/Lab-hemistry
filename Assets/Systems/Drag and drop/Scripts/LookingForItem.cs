using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualLab.PointerObjectsNS; 



namespace VirtualLab.DragAndDropNS 
{

public class LookingForItem : MonoBehaviour
{
	[SerializeField] DragAndDrop dragAndDrop; 
	[SerializeField] SelectedObjects selectedObjects; 



	//  Life cycle  ------------------------------------------------- 
	public void LookForItem () 
	{
		DragItem item = selectedObjects.item; 

		if (item && wantToPickUp) 
		{
			ReturnItem(item, selectedObjects.itemContactPoint); 
		}
	}

	void ReturnItem (DragItem item, Vector3 contactPoint) 
	{
		dragAndDrop.OnItemFound(item, contactPoint); 
	}



	//  Info  ------------------------------------------------------- 
	bool wantToPickUp 
	{
		get => Input.GetMouseButtonDown(0); 
	}

}

}
