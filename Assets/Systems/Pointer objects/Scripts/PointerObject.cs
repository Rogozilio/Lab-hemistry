using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PointerObjectsNS 
{

public class PointerObject 
{
	public GameObject gameObject; 
	public RaycastHit hitInfo; 


	public PointerObject (RaycastHit hitInfo) 
	{
		this.gameObject = hitInfo.collider.gameObject; 
		this.hitInfo = hitInfo; 
	}


	public T GetComponentInParent<T> () 
	{
		return gameObject.GetComponentInParent<T>(); 
	}
}

}
