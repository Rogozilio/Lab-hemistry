using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class SampleDataLoader : Operation<SampleData> 
{

    public SampleDataLoader (OnCompleted onCompleted, OnError onError = null) 
		: base(onCompleted, onError) {} 



	public void Start (int groupIndex, int sampleID) 
	{
		InitData(groupIndex, sampleID); 
		LoadSprite(); 
	}



	//  Sample data  ------------------------------------------------ 
	int groupIndex; 
	int sampleID; 

	void InitData (int groupIndex, int sampleID) 
	{
		this.groupIndex = groupIndex; 
		this.sampleID = sampleID; 
	}



	//  Sprite  ----------------------------------------------------- 
	void LoadSprite () 
	{
		string path = CreateSpritePath(); 
		new SpriteLoader(OnSpriteLoaded, OnLoadError).Start(path); 
	}

	void OnSpriteLoaded (Sprite sprite) 
	{
		CreateSampleData(sprite); 
	}

	void OnLoadError (string message) 
	{
		GoBackWithError("Не получилось загрузить изображения для образцов"); 
	}

	string CreateSpritePath () 
	{
		return"Samples/сталь " + sampleID + ".png"; 
	}



	//  Sample data  ------------------------------------------------ 
	void CreateSampleData (Sprite sprite) 
	{
		SampleData data = new SampleData(groupIndex, sampleID, sprite); 
		GoBack(data); 
	}

}

}
