using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.DragAndDropNS 
{

public class DropPlaceList : MonoBehaviour
{
	[SerializeField] List<DropPlace> dropPlaces; 

	public IReadOnlyList<DropPlace> places => dropPlaces; 
}

}
