using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.OutlineNS 
{

public class ObjectOutliner : MonoBehaviour 
{
    List<Outline> objectsWithOutline = new List<Outline>(); 



	//  Creating outline  ------------------------------------------- 
	public void AddOutline (GameObject obj) 
	{
		Outline outline = obj.GetComponent<Outline>(); 
		AddOutline(outline); 
	}

	public void AddOutline (Component component) 
	{
		Outline outline = component.GetComponent<Outline>(); 
		AddOutline(outline); 
	}

	public void AddOutline (Outline obj) 
	{
		if (objectsWithOutline.Contains(obj)) return; 

		obj.ShowOutline(); 
		objectsWithOutline.Add(obj); 
	}



	//  Clearing outline  ------------------------------------------- 
	public void ClearOutline (GameObject obj) 
	{
		Outline outline = obj.GetComponent<Outline>(); 
		ClearOutline(outline); 
	}

	public void ClearOutline (Component component) 
	{
		Outline outline = component.GetComponent<Outline>(); 
		ClearOutline(outline); 
	}

	public void ClearOutline (Outline obj) 
	{
		obj.HideOutline(); 
		objectsWithOutline.Remove(obj); 
	}

	public void ClearOutlineAll () 
	{
		foreach (Outline obj in objectsWithOutline) 
		{
			obj.HideOutline(); 
		}

		objectsWithOutline.Clear(); 
	}
}

}
