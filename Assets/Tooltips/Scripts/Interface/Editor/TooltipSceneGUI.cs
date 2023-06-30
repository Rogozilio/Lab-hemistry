using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace ERA.Tooltips.Core
{

public class TooltipSceneGUI 
{
    Color TOOLTIP_COLOR = Color.blue;



    //  Life cycle  -------------------------------------------------
    public void DrawGUI (Tooltip tooltip) 
    {
        InitTooltipData(tooltip);
        DrawTooltipOutline();
    }



    //  Tooltip data  -----------------------------------------------
    MainSettings main;
    UISettings ui;
    GeometrySettings geometry;
    LogicSettings visibility;

    Camera  camera;
    Vector3 cameraPoint;

    void InitTooltipData (Tooltip tooltip) 
    {
        main       = tooltip.__mainSettings;
        ui         = tooltip.__uiSettings;
        geometry   = tooltip.__geometrySettings;
        visibility = tooltip.__visibilitySettings;

        camera      = main.tooltipSystem.camera;
        cameraPoint = camera.transform.position;
    }



    //  Tooltip outline  --------------------------------------------
    void DrawTooltipOutline () 
    {
        Vector3 origin            = GetTooltipOrigin();
        Vector3 [] tooltipCorners = GetTooltipCorners(origin);

        DrawOriginDot(origin, cameraPoint);
        DrawTooltipRectangle(tooltipCorners);
        DrawLineToOrigin(origin, tooltipCorners);
    }

    void DrawOriginDot (Vector3 origin, Vector3 cameraPosition) 
    {
        SetMatrix(origin, cameraPosition);

        Handles.color = TOOLTIP_COLOR;
        Handles.DrawWireCube(Vector3.zero, 0.025f * Vector3.one);

        ClearMatrix();
    }

    void DrawTooltipRectangle (Vector3 [] corners) 
    {
        Handles.color = TOOLTIP_COLOR;
        Handles.DrawLine(corners[0], corners[1]);
        Handles.DrawLine(corners[1], corners[2]);
        Handles.DrawLine(corners[2], corners[3]);
        Handles.DrawLine(corners[3], corners[0]);
    }

    void DrawLineToOrigin (Vector3 origin, Vector3 [] corners) 
    {
        Vector3 closestCorner = FindClosestPoint(origin, corners);

        Handles.color = TOOLTIP_COLOR;
        Handles.DrawLine(origin, closestCorner);
    }

    

    //  Geometry  ---------------------------------------------------
    Vector3 GetTooltipOrigin () 
    {
        Vector3 origin = geometry.worldPoint.position;

        Vector3 offsetLocal = geometry.worldPoint.TransformVector(geometry.offsetLocalSpace);
        Vector3 offsetWorld = geometry.offsetWorldSpace;
        Vector3 offsetScreen = ScreenToWorldPoint(geometry.worldPoint.position, geometry.offsetScreenSpace) - origin;

        origin += offsetLocal;
        origin += offsetWorld;
        origin += offsetScreen;

        return origin;
    }

    Vector3 [] GetTooltipCorners (Vector3 origin) 
    {
        Vector3 position = ScreenToWorldPoint(origin, geometry.idealOffsetFromOrigin);

        Vector2 size = ui.mainArea.sizeDelta;
        Vector2 extentsScreenX = new Vector2(size.x / 2, 0);
        Vector2 extentsScreenY = new Vector2(0, size.y / 2);

        Vector3 extentsWorldX  = ScreenToWorldPoint(origin, extentsScreenX) - origin;
        Vector3 extentsWorldY  = ScreenToWorldPoint(origin, extentsScreenY) - origin;

        Vector3 a = position - extentsWorldX - extentsWorldY;
        Vector3 b = position - extentsWorldX + extentsWorldY;
        Vector3 c = position + extentsWorldX + extentsWorldY;
        Vector3 d = position + extentsWorldX - extentsWorldY;

        return new Vector3[] {a, b, c, d};
    }

    Vector3 FindClosestPoint (Vector3 origin, Vector3 [] points) 
    {
        Vector3 closest     = points[0];
        float   minDistance = Vector3.Distance(origin, closest);

        for (int i = 1; i < points.Length; i++) 
        {
            float distance = Vector3.Distance(origin, points[i]);

            if (distance < minDistance) 
            {
                closest     = points[i];
                minDistance = distance;
            }
        }

        return closest;
    }

    Vector3 ScreenToWorldPoint (Vector3 worldPoint, Vector2 screenOffset) 
    {
        // set camera to look at origin point 
        Quaternion originalRotation = camera.transform.rotation;
        Vector3 cameraToOrigin = worldPoint - cameraPoint;
        camera.transform.rotation = Quaternion.LookRotation(cameraToOrigin);

        // find screen point in 3D 
        float distance = Vector3.Distance(cameraPoint, worldPoint);
        Vector3 screenZero = new Vector3(camera.scaledPixelWidth / 2, camera.scaledPixelHeight / 2, distance);
        Vector3 screenPoint = screenZero + (Vector3) screenOffset;

        // get world point 
        Vector3 pointWorld = camera.ScreenToWorldPoint(screenPoint);

        // set camera to original rotation 
        camera.transform.rotation = originalRotation;

        return pointWorld;
    }



    //  Drawing tech  -----------------------------------------------
    void SetMatrix (Vector3 originPoint, Vector3 lookAtPoint) 
    {
        Vector3 lookDirection = lookAtPoint - originPoint;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);

        Handles.matrix = 
            Matrix4x4.Translate(originPoint) * 
            Matrix4x4.Rotate(rotation);
    }

    void ClearMatrix () 
    {
        Handles.matrix = Matrix4x4.identity;
    }

}

}