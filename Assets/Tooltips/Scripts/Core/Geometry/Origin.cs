using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public class Origin 
{
    MainSettings mainSettings;
    GeometrySettings geometrySettings;



    public Origin (MainSettings mainSettings, GeometrySettings geometrySettings) 
    {
        this.mainSettings = mainSettings;
        this.geometrySettings = geometrySettings;
    }



    //  Data  -------------------------------------------------------
    public Vector2 point { get; private set; }



    //  Life cycle  -------------------------------------------------
    public void Update () 
    {
        if (Application.isPlaying) UpdateFromWorldPoint();
        else                       UpdateFromUI();
    }



    //  Update (from UI)  -------------------------------------------
    void UpdateFromUI () 
    {
        point = geometrySettings.editModePosition;
    }



    //  Update (from world point)  ----------------------------------
    void UpdateFromWorldPoint () 
    {
        Vector3 pointWorld = geometrySettings.worldPoint.position;
        pointWorld = AddOffsetLocalSpace(pointWorld);
        pointWorld = AddOffsetWorldSpace(pointWorld);

        Vector2 pointScreen = WorldToScreen(pointWorld);
        pointScreen = AddOffsetScreenSpace(pointScreen);

        point = pointScreen;
    }

    Vector3 AddOffsetLocalSpace (Vector3 point) 
    {
        Vector3 offset = geometrySettings.worldPoint.TransformVector(geometrySettings.offsetLocalSpace);
        return point + offset;
    }

    Vector3 AddOffsetWorldSpace (Vector3 point) 
    {
        return point + geometrySettings.offsetWorldSpace;
    }

    Vector2 WorldToScreen (Vector3 point) 
    {
        Camera camera = mainSettings.tooltipSystem.camera;
        Vector3 pointScreen = camera.WorldToScreenPoint(point);

        if (pointScreen.z < 0) pointScreen = Vector3.positiveInfinity;
        
        return pointScreen;
    }

    Vector2 AddOffsetScreenSpace (Vector2 point) 
    {
        return point + geometrySettings.offsetScreenSpace;
    }

}

}
