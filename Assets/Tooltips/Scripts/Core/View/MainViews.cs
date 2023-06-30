using System.Collections;
using System.Collections.Generic;
using ERA.Tooltips.Core;
using UnityEngine;



namespace VirtualLab.Tooltips.Core
{

public class MainViews : ILifeCycle
{
    MainViewsSettings settings;
    Geometry geometry;



    public MainViews (MainViewsSettings settings, Geometry geometry) 
    {
        this.settings = settings;
        this.geometry = geometry;

        UpdateSettings();
    }



    //  Data  -------------------------------------------------------
    MainArea     mainArea     => settings.mainArea;
    LineToOrigin lineToOrigin => settings.lineToOrigin;
    OriginDot    originDot    => settings.originDot;



    //  Life cycle  -------------------------------------------------
    bool isReady => settings.isReady && geometry.isReady;

    public void StartMe () 
    {
        if (isReady) 
        {
            mainArea    .StartMe();
            lineToOrigin.StartMe();
            originDot   .StartMe();
        }
    }

    public void UpdateMe () 
    {
        if (isReady) 
        {
            mainArea    .UpdateMe();
            lineToOrigin.UpdateMe();
            originDot   .UpdateMe();
        }
    }

    public void UpdateSettings () 
    {
        if (settings.isReady) 
        {
            mainArea    .Init(geometry);
            lineToOrigin.Init(geometry);
            originDot   .Init(geometry);
        }
    }

}

}
