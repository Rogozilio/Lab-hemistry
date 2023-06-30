using System.Collections;
using System.Collections.Generic;
using ERA.Tooltips.Core;
using UnityEngine;



namespace VirtualLab.Tooltips.Core
{

public class LineToOrigin : View
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
        if (isVisible) MakeLine();
        else           Hide();
    }

    void MakeLine () 
    {
        Vector2 origin = geometry.origin.point;
        Vector2 anchor = geometry.GetCornerPosition(geometry.originCorner);
        Vector2 anchorToOrigin = anchor - origin;

        Vector2 midPoint;
        Quaternion rotation;
        float distance;

        if (IsGood(origin)) 
        {
            midPoint = (origin + anchor) / 2 - geometry.rectWorld.min;
            rotation = Quaternion.FromToRotation(Vector2.up, anchor - origin);
            distance = Vector2.Distance(origin, anchor);
        }
        else 
        {
            midPoint = origin;
            rotation = Quaternion.identity;
            distance = 0;
        }

        rectTransform.gameObject.SetActive(true);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.anchoredPosition = midPoint;
        rectTransform.rotation = rotation;
        rectTransform.sizeDelta = new Vector2(0, distance);
    }

    void Hide () 
    {
        rectTransform.gameObject.SetActive(false);
    }



    //  Tech  -------------------------------------------------------
    bool IsGood (Vector2 v) 
    {
        return 
            v != Vector2.zero && 
            v.x != Mathf.Infinity && 
            v.x != Mathf.NegativeInfinity && 
            v.y != Mathf.Infinity && 
            v.y != Mathf.NegativeInfinity;
    }

}

}
