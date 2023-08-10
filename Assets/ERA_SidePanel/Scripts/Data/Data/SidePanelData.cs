using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

public class SidePanelData 
{
    // public static readonly int TAB_COUNT = System.Enum.GetNames(typeof(Tab)).Length; 
    // public static readonly string [] TAB_NAMES = { 
    //     "About", 
    //     "Theory", 
    //     "Instructions" 
    // }; 

	public PictureViewData pictureView; 
    List<TabData> tabs; 


	public SidePanelData (List<TabData> tabs, PictureViewData pictureView) 
	{
		this.tabs = tabs; 
		this.pictureView = pictureView; 
	}


	public TabData GetTabData (Tab tab) 
	{
		return tabs[(int) tab]; 
	}


    public override string ToString () 
    {
        string s = "--  Side panel data  --\n";

        foreach (var tab in tabs) 
        {
            s += tab.ToString() + "\n";
        }

        s += "Pictures count: " + pictureView.pictures.Count + "\n";

        return s;
    }

}

}
