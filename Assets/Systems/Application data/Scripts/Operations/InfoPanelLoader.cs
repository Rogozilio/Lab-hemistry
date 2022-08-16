using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class InfoPanelLoader : Operation<InfoPanelData> 
{    

	public InfoPanelLoader (OnCompleted onCompleted, OnError onError) 
        : base(onCompleted, onError) {} 



	public void Start () 
	{
		LoadTabs(); 
		LoadPictureView(); 
	}



	//  Tab data  --------------------------------------------------- 
	List<TabData> tabDataList = new List<TabData>(); 

	bool allTabsLoaded => tabDataList.Count == AppData.TAB_COUNT; 

	void LoadTabs () 
	{
		LoadTab(0); 
	}

	void LoadTab (int tabIndex) 
	{
		Tab tab = (Tab) tabIndex; 
		new TabDataLoader(OnTabDataLoaded, GoBackWithError).Start(tab); 
	}

	void OnTabDataLoaded (TabData tabData) 
	{
		tabDataList.Add(tabData); 

		if (!allTabsLoaded) 
		{
			LoadTab(tabDataList.Count); 
			return; 
		}

		if (pictureViewLoaded) CreateInfoPanel(); 
	}



	//  Picture view  ----------------------------------------------- 
	PictureViewData pictureView; 

	bool pictureViewLoaded => pictureView != null; 

	void LoadPictureView () 
	{
		new PictureViewLoader(OnPicViewLoaded, GoBackWithError).Start(); 
	}

	void OnPicViewLoaded (PictureViewData pictureView) 
	{
		this.pictureView = pictureView; 
		if (allTabsLoaded && pictureViewLoaded) CreateInfoPanel(); 
	}



	//  Info panel  ------------------------------------------------- 
	void CreateInfoPanel () 
	{
		InfoPanelData infoPanel = new InfoPanelData(tabDataList, pictureView); 
		GoBack(infoPanel); 
	}

}

}
