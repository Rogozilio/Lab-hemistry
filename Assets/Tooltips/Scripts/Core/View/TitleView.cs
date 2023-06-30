using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



namespace ERA.Tooltips.Core
{

public class TitleView : DataView
{
    [SerializeField] TMP_Text textPart;



    //  Info  -------------------------------------------------------
    public override TooltipData.Type type => TooltipData.Type.Title;

    new bool isReady => base.isReady && textPart;



    //  Life cycle  -------------------------------------------------
    public override void StartMe()
    {
        
    }

    public override void UpdateMe () 
    {
        if (isReady) UpdateTextPart();
    }



    //  Actions  ----------------------------------------------------
    void UpdateTextPart () 
    {
        textPart.text = data.stringData;
    }

}

}
