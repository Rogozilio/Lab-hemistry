using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.OutlineNS 
{

public class ObjectOutliner : MonoBehaviour 
{
    List<OutlineOld> objectsWithOutline = new List<OutlineOld>(); 



	//  Creating outline  ------------------------------------------- 
	public void AddOutline (GameObject obj) 
	{
		OutlineOld outlineOld = obj.GetComponent<OutlineOld>(); 
		AddOutline(outlineOld); 
	}

	public void AddOutline (Component component) 
	{
		OutlineOld outlineOld = component.GetComponent<OutlineOld>(); 
		AddOutline(outlineOld); 
	}

	public void AddOutline (OutlineOld obj) 
	{
		if (objectsWithOutline.Contains(obj)) return; 

		obj.ShowOutline(); 
		objectsWithOutline.Add(obj); 
	}



	//  Clearing outline  ------------------------------------------- 
	public void ClearOutline (GameObject obj) 
	{
		OutlineOld outlineOld = obj.GetComponent<OutlineOld>(); 
		ClearOutline(outlineOld); 
	}

	public void ClearOutline (Component component) 
	{
		OutlineOld outlineOld = component.GetComponent<OutlineOld>(); 
		ClearOutline(outlineOld); 
	}

	public void ClearOutline (OutlineOld obj) 
	{
		obj.HideOutline(); 
		objectsWithOutline.Remove(obj); 
	}

	public void ClearOutlineAll () 
	{
		foreach (OutlineOld obj in objectsWithOutline) 
		{
			obj.HideOutline(); 
		}

		objectsWithOutline.Clear(); 
	}
}

}
