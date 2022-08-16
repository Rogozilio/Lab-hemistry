using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using VirtualLab.PointerObjectsNS; 
using VirtualLab.OutlineNS; 



namespace VirtualLab.DragAndDropNS 
{

public class PlaceOutliner : MonoBehaviour
{
	[SerializeField] DragAndDrop dragAndDrop; 
	[SerializeField] SelectedObjects selectedObjects; 
	[SerializeField] ObjectOutliner outliner; 

	DragItem pointerItem; 



	void Update () 
	{
		outliner.ClearOutlineAll(); 

		if (dragAndDrop.hasItem) 
		{
			DropPlace pointerPlace = selectedObjects.place; 
			if (pointerPlace) outliner.AddOutline(pointerPlace); 
		}
	}

}

}
