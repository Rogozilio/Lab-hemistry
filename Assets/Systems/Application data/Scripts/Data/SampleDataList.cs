using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class SampleDataList 
{

	public SampleDataList (List<SampleData> samples) 
	{
		this._samples = samples; 
	}



	//  Samples  ---------------------------------------------------- 
	List<SampleData> _samples = new List<SampleData>(); 

	public IReadOnlyList<SampleData> samples => _samples; 



	//  Tech  ------------------------------------------------------- 
	public override string ToString () 
	{
		string s = "Sample data list: \n"; 

		foreach (SampleData sample in _samples) 
		{
			s += sample + "\n"; 
		}

		return s; 
	}

}

}
