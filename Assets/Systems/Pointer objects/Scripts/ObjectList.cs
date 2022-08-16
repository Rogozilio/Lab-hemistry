using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PointerObjectsNS 
{

public class ObjectList 
{
    List<PointerObject> objects = new List<PointerObject>(); 


	
	//  Saving objects  -------------------------------------------- 
	public void AddObject (PointerObject obj) 
	{
		objects.Add(obj); 
	}

	public void Clear () 
	{
		this.objects.Clear(); 
	}



	//  Reading objects  -------------------------------------------- 
	public PointerObject 		GetClosestObject () 
	{
		if (objects.Count > 0) return objects[0]; 
		else 				   return null; 
	}

	public GameObject 			GetClosestGameObject () 
	{
		if (objects.Count > 0) return objects[0].gameObject; 
		else 				   return null; 
	}


	public List<PointerObject> 	GetAllObjects () 
	{
		return new List<PointerObject>(objects); 
	}

	public List<GameObject> 	GetAllGameObjects () 
	{
		List<GameObject> gameObjects = new List<GameObject>(); 

		foreach (PointerObject pointerObject in objects) 
		{
			gameObjects.Add(pointerObject.gameObject); 
		}

		return gameObjects; 
	}


	public PointerObject 		GetClosestObject<T> () where T: Component 
	{
		foreach (PointerObject obj in objects) 
		{
			T component = obj.gameObject.GetComponentInParent<T>(); 
			if (component != null) return obj; 
		}

		return null; 
	}

	public GameObject 			GetClosestGameObject<T> () where T: Component 
	{
		foreach (PointerObject obj in objects) 
		{
			T component = obj.gameObject.GetComponentInParent<T>(); 
			if (component != null) return obj.gameObject; 
		}

		return null; 
	}

	public T 					GetClosestComponent<T> () where T: Component 
	{
		foreach (PointerObject obj in objects) 
		{
			T component = obj.gameObject.GetComponentInParent<T>(); 
			if (component != null) return component; 
		}

		return null; 
	}


	public List<PointerObject> 	GetAllObjects<T> () where T: Component 
	{
		List<PointerObject> output = new List<PointerObject>(); 

		foreach (PointerObject obj in objects) 
		{
			T component = obj.GetComponentInParent<T>(); 
			if (component != null) output.Add(obj); 
		}

		return output; 
	}
	
	public List<GameObject> 	GetAllGameObjects<T> () where T: Component 
	{
		List<GameObject> output = new List<GameObject>(); 

		foreach (PointerObject obj in objects) 
		{
			T component = obj.GetComponentInParent<T>(); 
			if (component != null) output.Add(obj.gameObject); 
		}

		return output; 
	}

	public List<T> 				GetAllComponents<T> () where T: Component 
	{
		List<T> output = new List<T>(); 

		foreach (PointerObject obj in objects) 
		{
			T component = obj.GetComponentInParent<T>(); 
			if (component != null) output.Add(component); 
		}

		return output; 
	}



	
	


	//  Tech  ------------------------------------------------------- 
	public override string ToString () 
	{
		string s = ""; 

		foreach (PointerObject obj in objects) 
		{
			s += obj.gameObject.name + "\n"; 
		}

		return s; 
	}

}

}
