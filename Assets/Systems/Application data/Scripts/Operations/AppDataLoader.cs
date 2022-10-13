using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class AppDataLoader : Operation<AppData>
{

    public AppDataLoader (OnCompleted onCompleted, OnError onError = null) 
        : base(onCompleted, onError) {} 



	//  Operation  -------------------------------------------------- 
	bool allLoaded => infoPanel != null; 

    public void Start () 
    {
        LoadInfoPanel(); 
		LoadLabData(); 
    }



	//  Info panel  ------------------------------------------------- 
	InfoPanelData infoPanel; 

	void LoadInfoPanel () 
	{
		new InfoPanelLoader(OnInfoPanelLoaded, GoBackWithError).Start(); 
	}

	void OnInfoPanelLoaded (InfoPanelData infoPanel) 
	{
		this.infoPanel = infoPanel; 

		if (allLoaded) CreateAppData(); 
	}



	//  Lab data  --------------------------------------------------- 
	LabData labData; 

	void LoadLabData () 
	{
		new LabDataLoader(OnLabDataLoaded, GoBackWithError).Start(); 
	}

	void OnLabDataLoaded (LabData labData) 
	{
		this.labData = labData; 

		if (allLoaded) CreateAppData(); 
	}



	//  App data  --------------------------------------------------- 
	void CreateAppData () 
	{
		AppData appData = new AppData(infoPanel, labData); 
		GoBack(appData); 
	}

}

}
