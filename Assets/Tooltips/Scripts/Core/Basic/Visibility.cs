using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public class Visibility 
{
    UISettings uiSettings;
    LogicSettings logicSettings;



    public Visibility (UISettings uiSettings, LogicSettings logicSettings) 
    {
        this.uiSettings    = uiSettings;
        this.logicSettings = logicSettings;
    }



    //  Data  -------------------------------------------------------
    public bool isVisible      => visibility > 0;
    public bool isFullyVisible => visibility == 1;

    float visibility = 0;
    float target = 0;
    float velocity = 0;



    //  Life cycle  -------------------------------------------------
    bool isReady => uiSettings.isReady && logicSettings.isReady;

    public void Update () 
    {
        if (Application.isPlaying) 
        {
            if (logicSettings.alwaysVisible) ShowInstantly();
            else                             UpdateAnimation();
        }
        else ShowInstantly();
    }



    //  Actions  ----------------------------------------------------
    public void Show () 
    {
        if (target == 1) return;
        target = 1;
        velocity = 0;
    }

    public void Hide () 
    {
        if (target == 0) return;
        target = 0;
        velocity = 0;
    }

    public void ShowInstantly () 
    {
        target = 1;
        visibility = 1;
        velocity = 0;
        UpdateObject();
    }

    public void HideInstantly () 
    {
        target = 0;
        visibility = 0;
        velocity = 0;
        UpdateObject();
    }

    void UpdateAnimation () 
    {
        MoveToTarget();
        CompleteIfAlmostThere();
        UpdateObject();
    }

    void MoveToTarget () 
    {
        visibility = Mathf.SmoothDamp(
            visibility,
            target,
            ref velocity,
            logicSettings.fadeTime
        );
    }

    void CompleteIfAlmostThere () 
    {
        bool almostReachedOne = target == 1 && 1 - visibility < 0.001f;
        if (almostReachedOne) 
        {
            visibility = 1;
            velocity = 0;
        }

        bool almostReachedZero = target == 0 && visibility < 0.001f;
        if (almostReachedZero) 
        {
            visibility = 0;
            velocity = 0;
        }
    }

    void UpdateObject () 
    {
        if (isReady) 
        {
            uiSettings.canvasGroup.alpha = visibility;
        }
    }

}

}
