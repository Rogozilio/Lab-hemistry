using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using VirtualLab.ApplicationData; 



namespace VirtualLab 
{

public class Samples : MonoBehaviour
{
	[SerializeField] List<Sample> samples; 
    


	//  Lab data  --------------------------------------------------- 
	public void InitLabData (AppData appData) 
	{
		InitSamples(appData.labData.samples); 
	}



	//  Samples  ---------------------------------------------------- 
	RandomMapping mapping; 

	void InitSamples (SampleDataList sampleDataList) 
	{
		mapping = new RandomMapping(samples.Count); 

		for (int i = 0; i < samples.Count; i++) 
		{
			AssignDataToSample(sampleDataList, i); 
		}
	}

	void AssignDataToSample (SampleDataList sampleDataList, int sampleIndex) 
	{
		int dataIndex = mapping.Map(sampleIndex); 
		SampleData data = sampleDataList.samples[dataIndex]; 

		samples[sampleIndex].InitLabData(data); 
	}


}

}
