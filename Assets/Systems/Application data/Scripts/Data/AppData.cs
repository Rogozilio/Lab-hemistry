using System.Collections;
using System.Collections.Generic;
using ERA.SidePanelAsset;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

//public enum Tab { About, Theory, Instructions } 



public class AppData 
{
    public static readonly int TAB_COUNT = System.Enum.GetNames(typeof(Tab)).Length; 
    public static readonly string [] TAB_NAMES = { 
        "About", 
        "Theory", 
        "Instructions" 
    }; 

	public InfoPanelData infoPanel { get; private set; }
	public LabData labData; 



	public AppData (InfoPanelData infoPanel, LabData labData) 
	{
		this.infoPanel = infoPanel; 
		this.labData = labData; 
	}

}

}
