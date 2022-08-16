using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.DragAndDropNS 
{

public abstract class Surface : MonoBehaviour 
{
	public abstract void Setup (List<Vector3> pointsWorld); 
	public abstract Vector3 GetPointWorld (Vector3 pointScreen); 
}

}
