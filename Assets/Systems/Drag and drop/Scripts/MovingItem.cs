using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.DragAndDropNS 
{

public class MovingItem : MonoBehaviour
{
	[SerializeField] DragAndDrop dragAndDrop; 
	[SerializeField] SelectedObjects selectedObjects; 
	[SerializeField] Surface surface; 
	[SerializeField] DropPlaceList dropPlaceList; 

	new Camera camera; 



	void Awake () 
	{
		camera = Camera.main; 
	}



	//  Life cycle  ------------------------------------------------- 
	public void PickUpItem (DragItem item, Vector3 contactPoint) 
	{
		SaveItem(item); 
		item.StartDrag(contactPoint); 

		if (wantToDrop) DropItem(); 
	}

	public void MoveItem () 
	{
		SetupSurface(); 
		MoveItemAlongSurface(); 

		if (wantToDrop) DropItem(); 
	}

	void DropItem () 
	{
		DropItemSomewhere(); 
		ClearItem(); 

		dragAndDrop.OnItemDropped(); 
	}



	//  Input  ------------------------------------------------------ 
	bool wantToDrop 
	{
		get => Input.GetMouseButtonUp(0); 
	}



	//  Current item  ----------------------------------------------- 
	DragItem item; 
	Vector3 itemOrigin; 

	void SaveItem (DragItem item) 
	{
		this.item = item; 
		itemOrigin = item.transform.position; 
	}

	void ClearItem () 
	{
		item = null; 
	}



	//  Surface  ---------------------------------------------------- 
	void SetupSurface () 
	{
		List<Vector3> points = new List<Vector3>(); 

		AddItemPoint(points); 
		AddPlacePoints(points); 

		surface.Setup(points); 
	}

	void AddItemPoint (List<Vector3> points) 
	{
		points.Add(itemOrigin); 
	}

	void AddPlacePoints (List<Vector3> points) 
	{
		foreach (DropPlace place in dropPlaceList.places) 
		{
			points.Add(place.position); 
		}
	}

	Vector3 GetCurrentPoint () 
	{
		return surface.GetPointWorld(Input.mousePosition); 
	}



	//  Movement ---------------------------------------------------- 
	void MoveItemAlongSurface () 
	{
		Vector3 point = GetCurrentPoint(); 
		item.MoveTo(point); 
	}

	void DropItemSomewhere () 
	{
		DropPlace nearbyPlace = selectedObjects.place; 

		if (nearbyPlace && nearbyPlace.CanRecieveItem()) 
		{
			item.DropToPlace(nearbyPlace); 
		}
		else 
		{
			item.DropToAir(); 
		}
	}

}

}
