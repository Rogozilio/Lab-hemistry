using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualLab.OutlineNS; 



namespace VirtualLab.PointerObjectsNS 
{

public class Test_PointerObjects : MonoBehaviour
{
	public enum Mode { SelectOne, SelectAll } 

    [SerializeField] PointerObjects currentObjects; 
	[SerializeField] Mode mode; 

	List<Outline> selectedObjects = new List<Outline>(); 



	void Update () 
	{
		ClearOutline(); 
		ClearSelection(); 

		switch (mode) 
		{
			case Mode.SelectOne: 
				SelectOneObject(); 
				break; 
			case Mode.SelectAll: 
				SelectAllObjects(); 
				break; 
		}

		ShowOutlineOnSelected(); 
	}



	//  Selection  -------------------------------------------------- 
	void SelectOneObject () 
	{
		Outline obj = currentObjects.GetClosestComponent<Outline>(); 

		if (obj != null) 
		{
			selectedObjects.Add(obj); 
		}
	}

	void SelectAllObjects () 
	{
		selectedObjects = currentObjects.GetAllComponents<Outline>(); 
	}

	void ClearSelection () 
	{
		selectedObjects.Clear(); 
	}



	//  Outline  ---------------------------------------------------- 
	void ShowOutlineOnSelected () 
	{
		foreach (Outline obj in selectedObjects) 
		{
			//obj.ShowOutline(); 
		}
	}

	void ClearOutline () 
	{
		foreach (Outline obj in selectedObjects) 
		{
			//obj.HideOutline(); 
		}
	}

}

}
