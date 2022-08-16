using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VirtualLab.DragAndDropNS; 



namespace VirtualLab {

public class Microscope : MonoBehaviour
{
    [SerializeField] DropPlace samplePoint; 



    //  Sample  ----------------------------------------------------- 
    public Sample sample 
	{
		get => samplePoint.item?.GetComponentInParent<Sample>(); 
	}

}

}
