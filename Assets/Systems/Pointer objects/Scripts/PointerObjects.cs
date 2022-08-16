using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PointerObjectsNS 
{

public class PointerObjects : MonoBehaviour 
{
	new Camera camera; 

	ObjectList objectList = new ObjectList();

	void Awake () 
	{
		camera = Camera.main; 
	}

	void Update () 
	{
		UpdateObjects(); 
	}



	//  Finding objects  -------------------------------------------- 
	void UpdateObjects () 
	{
		objectList.Clear(); 

		RaycastHit [] hits = RaycastAll(); 
		Sort(hits); 
		ExtractObjects(hits); 
	}

	RaycastHit [] RaycastAll () 
	{
		Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition); 
		return Physics.RaycastAll(mouseRay); 
	}

	void Sort (RaycastHit [] hits) 
	{
		bool changesWereMade; 

		do {
			changesWereMade = false; 

			for (int i = 0; i < hits.Length - 1; i++) 
			{
				bool wrongOrder = hits[i].distance > hits[i + 1].distance; 
				if (wrongOrder) 
				{
					RaycastHit t = hits[i]; 
					hits[i] = hits[i + 1]; 
					hits[i + 1] = t; 

					changesWereMade = true; 
				}
			}
		}
		while (changesWereMade); 
	}

	void ExtractObjects (RaycastHit [] hits) 
	{
		foreach (RaycastHit hit in hits) 
		{
			PointerObject pointerObject = new PointerObject(hit); 
			objectList.AddObject(pointerObject); 
		}
	}



	//  Accessing objects  ------------------------------------------ 
	public PointerObject 		GetClosestObject () 
	{
		return objectList.GetClosestObject(); 
	}

	public GameObject 			GetClosestGameObject () 
	{
		return objectList.GetClosestGameObject(); 
	}


	public List<PointerObject> 	GetAllObjects () 
	{
		return objectList.GetAllObjects(); 
	}

	public List<GameObject> 	GetAllGameObjects () 
	{
		return objectList.GetAllGameObjects(); 
	}


	public PointerObject 		GetClosestObject<T> () where T: Component 
	{
		return objectList.GetClosestObject<T>(); 
	}

	public GameObject 			GetClosestGameObject<T> () where T: Component 
	{
		return objectList.GetClosestGameObject<T>(); 
	}

	public T 					GetClosestComponent<T> () where T: Component 
	{
		return objectList.GetClosestComponent<T>(); 
	}


	public List<PointerObject> 	GetAllObjects<T> () where T: Component 
	{
		return objectList.GetAllObjects<T>(); 
	}
	
	public List<GameObject> 	GetAllGameObjects<T> () where T: Component 
	{
		return objectList.GetAllGameObjects<T>(); 
	}

	public List<T> 				GetAllComponents<T> () where T: Component 
	{
		return objectList.GetAllComponents<T>(); 
	}



	
	
}

}
