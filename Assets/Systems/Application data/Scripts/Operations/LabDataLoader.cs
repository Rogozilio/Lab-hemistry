using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class LabDataLoader : Operation<LabData> 
{
	
	public LabDataLoader (OnCompleted onCompleted, OnError onError = null) 
		: base(onCompleted, onError) {} 



	public void Start () 
	{
		new SampleDataListLoader(OnDataLoaded, GoBackWithError).Start(); 
	}

	void OnDataLoaded (SampleDataList sampleDataList) 
	{
		LabData labData = new LabData(sampleDataList); 
		GoBack(labData); 
	}

}

}
