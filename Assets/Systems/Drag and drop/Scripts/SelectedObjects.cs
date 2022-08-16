using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualLab.PointerObjectsNS; 
using VirtualLab.OutlineNS; 



namespace VirtualLab.DragAndDropNS 
{

public class SelectedObjects : MonoBehaviour 
{
	[SerializeField] DragAndDrop dragAndDrop; 
	[SerializeField] PointerObjects pointerObjects; 

	public DragItem item { get; private set; } 
	public Vector3 itemContactPoint { get; private set; } 

	public DropPlace place { get; private set; } 
	public Vector3 placeContactPoint { get; private set; } 



	//  Life cycle  ------------------------------------------------- 
	public void UpdateObjects () 
	{
		ClearSelection(); 

		if (!dragAndDrop.hasItem) SelectPointerItem(); 
		else 					  SelectPointerPlace(); 
	}



	//  Actions  ---------------------------------------------------- 
	void SelectPointerItem () 
	{
		PointerObject pointerObject = pointerObjects.GetClosestObject<DragItem>(); 

		if (pointerObject != null) 
		{
			item = pointerObject.GetComponentInParent<DragItem>(); 
			itemContactPoint = pointerObject.hitInfo.point; 
		}
		else 
		{
			item = null; 
			itemContactPoint = Vector3.zero; 
		}
	}

	void SelectPointerPlace () 
	{
		PointerObject pointerObject = pointerObjects.GetClosestObject<DropPlace>(); 
		
		if (pointerObject != null) 
		{
			place = pointerObject.GetComponentInParent<DropPlace>(); 
			placeContactPoint = pointerObject.hitInfo.point; 
		}
		else 
		{
			place = null; 
			placeContactPoint = Vector3.zero; 
		}
	}

	void ClearSelection () 
	{
		item = null; 
		place = null; 
	}

}

}
