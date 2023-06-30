using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public class UIPart 
{
    UISettings uiSettings;
    GeometrySettings geometrySettings;
    Geometry geometry;



    public UIPart (UISettings uiSettings, GeometrySettings geometrySettings, Geometry geometry) 
    {
        this.uiSettings       = uiSettings;
        this.geometrySettings = geometrySettings;
        this.geometry         = geometry;
    }



    //  UI parts  ---------------------------------------------------
    RectTransform mainArea     => uiSettings.mainArea;
    RectTransform lineToOrigin => uiSettings.lineToOrigin;
    RectTransform originDot    => uiSettings.originDot;



    //  Life cycle  -------------------------------------------------
    bool isReady => uiSettings.isReady && geometry.isReady;

    public void Update () 
    {
        if (isReady) 
        {
            UpdateMainArea();
            UpdateLineToOrigin();
            UpdateOriginDot();
        }
    }



    //  Main area  --------------------------------------------------
    void UpdateMainArea () 
    {
        mainArea.anchorMin = Vector2.zero;
        mainArea.anchorMax = Vector2.zero;
        mainArea.anchoredPosition = geometry.position.point;
        mainArea.sizeDelta = new Vector2(
            geometrySettings.width,
            mainArea.sizeDelta.y
        );
    }



    //  Line to origin  ---------------------------------------------
    bool isLineToOriginVisible => !geometry.isOriginInsideRectTransform;

    void UpdateLineToOrigin () 
    {
        if (isLineToOriginVisible) DrawLineToOrigin();
        else                       HideLineToOrigin();
    }

    void DrawLineToOrigin () 
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

        lineToOrigin.gameObject.SetActive(true);
        lineToOrigin.anchorMin = Vector2.zero;
        lineToOrigin.anchorMax = Vector2.zero;
        lineToOrigin.anchoredPosition = midPoint;
        lineToOrigin.rotation = rotation;
        lineToOrigin.sizeDelta = new Vector2(0, distance);
    }

    void HideLineToOrigin () 
    {
        lineToOrigin.gameObject.SetActive(false);
    }



    //  Origin dot  -------------------------------------------------
    bool isOriginDotVisible => !geometry.isOriginInsideRectTransform;

    void UpdateOriginDot () 
    {
        if (isOriginDotVisible) DrawOriginDot();
        else                    HideOriginDot();
    }

    void DrawOriginDot () 
    {
        Vector2 origin = -geometry.position.offsetFromOrigin;

        originDot.gameObject.SetActive(true);
        originDot.anchorMin = new Vector2(0.5f, 0.5f);
        originDot.anchorMax = new Vector2(0.5f, 0.5f);
        originDot.anchoredPosition = origin;
    }

    void HideOriginDot () 
    {
        originDot.gameObject.SetActive(false);
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
