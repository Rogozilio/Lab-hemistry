using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

[System.Serializable]
public class MainSettings : Settings
{
    public TooltipSystem tooltipSystem;
    
    
    public string name = "";


    public override bool isReady => 
        tooltipSystem && 
        tooltipSystem.settingsReady;

    public override void Init (MonoBehaviour tooltip) 
    {
        tooltipSystem = tooltip.GetComponentInParent<TooltipSystem>(true);
    }

}

}
