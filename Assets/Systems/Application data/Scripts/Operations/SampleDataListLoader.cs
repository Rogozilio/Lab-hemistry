using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class SampleDataListLoader : Operation<SampleDataList> 
{

	public SampleDataListLoader (OnCompleted onCompleted, OnError onError = null) 
		: base(onCompleted, onError) {} 




	public void Start () 
	{
		LoadSampleInfo(); 
	}



	//  Info  ------------------------------------------------------- 
	void LoadSampleInfo () 
	{
		new JSONLoader<SamplesInfo>(OnSampleInfoLoaded, OnLoadError).Start("Samples/info.json"); 
	}

	void OnSampleInfoLoaded (SamplesInfo info) 
	{
		SamplesSelector selector = new SamplesSelector(info); 
		LoadSamples(selector); 
	}



	//  Samples  ---------------------------------------------------- 
	List<SampleData> samples = new List<SampleData>(); 
	OperatoinsTracker tracker = new OperatoinsTracker(); 

	void LoadSamples (SamplesSelector selector) 
	{
		for (int i = 0; i < selector.groupCount; i++) 
		{
			foreach (int sampleID in selector.GetSelectedIDs(i)) 
			{
				LoadSample(i, sampleID); 
			}
		}

		tracker.AllStarted(); 
	}

	void LoadSample (int groupIndex, int sampleID) 
	{
		tracker.OneStarted(); 

		new SampleDataLoader(OnSampleLoaded, OnLoadError)
			.Start(groupIndex, sampleID); 
	}

	void OnSampleLoaded (SampleData data) 
	{
		samples.Add(data); 

		tracker.OneFinished(); 
		if (tracker.allFinished) CreateSampleDataList(); 
	}



	//  Sample data  ------------------------------------------------ 
	void CreateSampleDataList () 
	{
		SampleDataList data = new SampleDataList(samples); 
		GoBack(data); 
	}



	//  Tech  ------------------------------------------------------- 
	void OnLoadError (string message) 
	{
		GoBackWithError("Не получилось загрузить образцы"); 
	}

}

}
