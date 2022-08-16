using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using VirtualLab.PointerObjectsNS; 
using VirtualLab.OutlineNS; 



namespace VirtualLab.DragAndDropNS 
{

public class ItemOutliner : MonoBehaviour
{
	[SerializeField] DragAndDrop dragAndDrop; 
	[SerializeField] SelectedObjects selectedObjects; 
	[SerializeField] ObjectOutliner outliner; 

	DragItem pointerItem; 



	void Update () 
	{
		outliner.ClearOutlineAll(); 

		if (!dragAndDrop.hasItem) 
		{
			DragItem pointerItem = selectedObjects.item; 
			if (pointerItem) outliner.AddOutline(pointerItem); 
		}
	}

}

}
