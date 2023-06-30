using System.Collections;
using System.Collections.Generic;
using ERA.Tooltips.Core;
using UnityEngine;



namespace VirtualLab.Tooltips.Core
{

public class MainArea : View
{
    
    //  Life cycle  -------------------------------------------------
    protected new bool isReady => base.isReady && geometry.isReady;

    public override void StartMe () 
    {
        if (isReady) UpdateTransform();
    }

    public override void UpdateMe () 
    {
        if (isReady) UpdateTransform();
    }



    //  Actions  ----------------------------------------------------
    void UpdateTransform () 
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.anchoredPosition = geometry.position.point;
    }

}

}
