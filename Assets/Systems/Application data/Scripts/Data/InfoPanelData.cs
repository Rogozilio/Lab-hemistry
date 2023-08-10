using System.Collections;
using System.Collections.Generic;
using ERA.SidePanelAsset;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class InfoPanelData 
{
	public PictureViewData pictureView; 



	public InfoPanelData (List<TabData> tabs, PictureViewData pictureView) 
	{
		this.tabs = tabs; 
		this.pictureView = pictureView; 
	}



	//  Tabs  ------------------------------------------------------- 
    List<TabData> tabs; 

	public TabData GetTabData (Tab tab) 
	{
		return tabs[(int) tab]; 
	}

}

}
