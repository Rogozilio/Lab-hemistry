using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

[System.Serializable]
public class UISettings : Settings
{
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;

    public RectTransform mainArea;
    public RectTransform lineToOrigin;
    public RectTransform originDot;

    public Transform dataViewsContainer;


    public override bool isReady => 
        canvasGroup && 
        mainArea && 
        lineToOrigin && 
        originDot && 
        dataViewsContainer;

    public override void Init (MonoBehaviour tooltip) 
    {
        rectTransform = tooltip.GetComponent<RectTransform>();
        canvasGroup   = tooltip.GetComponent<CanvasGroup>();

        mainArea     = tooltip.GetComponent<RectTransform>();
        lineToOrigin = tooltip.transform.Find("Graphics/Line to origin").GetComponent<RectTransform>();
        originDot    = tooltip.transform.Find("Graphics/Origin dot").GetComponent<RectTransform>();

        dataViewsContainer = tooltip.transform.Find("Data views");
    }

}

}