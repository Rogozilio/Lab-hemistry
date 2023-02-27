using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class PictureViewLoader : Operation<PictureViewData> 
{
    

	public PictureViewLoader (OnCompleted onCompleted, OnError onError = null) 
		: base(onCompleted, onError) {} 


	
	//  Operation  -------------------------------------------------- 
	public void Start () 
	{
		LoadPictureInfo(); 
	}

	void InitNewOperation () 
	{
		pictures = new List<PictureData>(); 
	}




	//  Picture info  ----------------------------------------------- 
	void LoadPictureInfo () 
	{
		new JSONLoader<PictureInfo>(OnDataLoaded, GoBackWithError).Start("Pictures/info.json"); 
	}

	void OnDataLoaded (PictureInfo pictureInfo) 
	{
		if (pictureInfo.hasPictures) LoadPictures(pictureInfo); 
		else 						 CreatePictureViewData(); 
	}



	//  Pictures  --------------------------------------------------- 
	List<PictureData> pictures = new List<PictureData>(); 
	OperatoinsTracker tracker = new OperatoinsTracker(); 

	void LoadPictures (PictureInfo info) 
	{
		LoadPicturesForTab(Tab.About, info); 
		LoadPicturesForTab(Tab.Instructions, info); 
		LoadPicturesForTab(Tab.Theory, info); 

		tracker.AllStarted(); 
	}

	void LoadPicturesForTab (Tab tab, PictureInfo info) 
	{
		PictureInfo.Entry [] entries = tab switch 
		{
			Tab.About 		 => info.about, 
			Tab.Instructions => info.instructions, 
			Tab.Theory 		 => info.theory, 
			_ => throw new UnityException() 
		}; 

		foreach (PictureInfo.Entry entry in entries) 
		{
			LoadPicture(tab, entry); 
		}
	}

	void LoadPicture (Tab tab, PictureInfo.Entry entry) 
    {
		tracker.OneStarted(); 

		new SpriteLoader(
			(sprite) => OnPicLoaded(sprite, tab, entry), 
			(_) => OnPicLoadError(tab, entry) 
		)
		.Start("Pictures/" + entry.pictureName); 
    }

	void OnPicLoaded (Sprite sprite, Tab tab, PictureInfo.Entry entry) 
	{
		Picture picture = CreatePicture(sprite, entry); 
		PictureData pictureData = CreatePictureData(picture, tab, entry); 

		pictures.Add(pictureData); 

		tracker.OneFinished(); 
		if (tracker.allFinished) CreatePictureViewData(); 
	}

	void OnPicLoadError (Tab tab, PictureInfo.Entry entry) 
	{
		GoBackWithError(
			"Картинка " + entry.pictureName + 
			" для страницы " + (entry.pageNumber + 1) + 
			" на вкладке " + tab + 
			" не найдена." 
		); 
	}

	Picture CreatePicture (Sprite sprite, PictureInfo.Entry entry) 
	{
		return new Picture(
			entry.pictureName, 
			entry.pictureDescription, 
			sprite 
		); 
	}

	PictureData CreatePictureData (Picture picture, Tab tab, PictureInfo.Entry entry) 
	{
		return new PictureData(
			picture, 
			tab, 
			entry.pageNumber - 1, 
			entry.x, 
			entry.y
		); 
	}




	//  Picture view  ----------------------------------------------- 
	void CreatePictureViewData () 
	{
		PictureViewData pictureViewData = new PictureViewData(pictures); 
		GoBack(pictureViewData); 
	}
	
}

}
