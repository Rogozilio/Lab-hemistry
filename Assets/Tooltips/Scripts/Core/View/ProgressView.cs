using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



namespace ERA.Tooltips.Core
{

public class ProgressView : DataView
{
    [SerializeField] RectTransform barFill;
    [SerializeField] TMP_Text textPart;



    //  Info  -------------------------------------------------------
    public override TooltipData.Type type => TooltipData.Type.Progress;

    new bool isReady => base.isReady && barFill && textPart;

    
    
    //  Life cycle  -------------------------------------------------
    public override void StartMe()
    {
        
    }

    public override void UpdateMe () 
    {
        if (isReady) UpdateParts();
    }



    //  Actions  ----------------------------------------------------
    void UpdateParts () 
    {
        UpdateBarFill();
        UpdateTextPart();
    }

    void UpdateBarFill () 
    {
        float progress = Mathf.Clamp01(data.numberData);

        Vector2 anchor = barFill.anchorMax;
        anchor.x = progress;
        barFill.anchorMax = anchor;
    }

    void UpdateTextPart () 
    {
        textPart.text = data.stringData;
    }

}

}
