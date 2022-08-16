using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualLab.Tooltips; 
using VirtualLab.ApplicationData; 



namespace VirtualLab {

public class Sample : MonoBehaviour 
{

    //  Lab data  --------------------------------------------------- 
    public SampleData data { get; private set; } 

	public void InitLabData (SampleData data) 
	{
		this.data = data; 
	}

}

}