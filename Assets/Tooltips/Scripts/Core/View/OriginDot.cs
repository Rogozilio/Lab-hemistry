using System.Collections;
using System.Collections.Generic;
using ERA.Tooltips.Core;
using UnityEngine;



namespace VirtualLab.Tooltips.Core
{

public class OriginDot : View
{

    //  Info  -------------------------------------------------------
    bool isVisible => !geometry.isOriginInsideRectTransform;



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
        if (isVisible) GoToOrigin();
        else           Hide();
    }

    void GoToOrigin () 
    {
        Vector2 origin = -geometry.position.offsetFromOrigin;

        rectTransform.gameObject.SetActive(true);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = origin;
    }

    void Hide () 
    {
        rectTransform.gameObject.SetActive(false);
    }

}

}
