using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public class Position 
{
    UISettings uiSettings;
    GeometrySettings geometrySettings;
    Origin origin;



    public Position (UISettings uiSettings, GeometrySettings geometrySettings, Origin origin) 
    {
        this.uiSettings       = uiSettings;
        this.geometrySettings = geometrySettings;
        this.origin           = origin;
    }



    //  Data  -------------------------------------------------------
    public Vector2 offsetFromOrigin { get; private set; }
    public Vector2 point            { get; private set; }



    //  Life cycle  -------------------------------------------------
    public void Update () 
    {
        if (Application.isPlaying) UpdatePlayMode();
        else                       UpdateEditMode();
    }

    void UpdateEditMode () 
    {
        Rect parent  = CreateRect((RectTransform) uiSettings.mainArea.parent);
        Rect tooltip = CreateRect(uiSettings.mainArea);

        Vector2 idealPosition = origin.point + geometrySettings.idealOffsetFromOrigin;
        tooltip.center = idealPosition;

        Vector2 originToPointUnclamped = idealPosition - origin.point;
        Vector2 originToPoint          = Vector2.ClampMagnitude(originToPointUnclamped, geometrySettings.maxDistanceFromOrigin);

        point = origin.point + originToPoint;
        offsetFromOrigin = originToPoint;
    }

    void UpdatePlayMode () 
    {
        Rect parent  = CreateRect((RectTransform) uiSettings.mainArea.parent);
        Rect tooltip = CreateRect(uiSettings.mainArea);

        Vector2 idealPosition = origin.point + geometrySettings.idealOffsetFromOrigin;
        tooltip.center = idealPosition;

        OutOfBounds idealOutOfBounds = FindOutOfBounds(parent, tooltip);
        Vector2 idealToInside = idealOutOfBounds.toInside;

        Vector2 pointInBounds = idealPosition + idealToInside;
        
        Vector2 originToPointInBounds = pointInBounds - origin.point;
        Vector2 originToPoint         = Vector2.ClampMagnitude(originToPointInBounds, geometrySettings.maxDistanceFromOrigin);

        point = origin.point + originToPoint;
        offsetFromOrigin = originToPoint;
    }



    //  Geometry  ---------------------------------------------------
    Rect CreateRect (RectTransform rt) 
    {
        Vector3 [] corners = new Vector3[4]; 
        rt.GetWorldCorners(corners); 

        Bounds bounds = new Bounds(corners[0], Vector3.zero); 
        bounds.Encapsulate(corners[0]); 
        bounds.Encapsulate(corners[1]); 
        bounds.Encapsulate(corners[2]); 
        bounds.Encapsulate(corners[3]); 
        
        Rect rect = new Rect(bounds.min, bounds.size); 
        return rect; 
    }

    OutOfBounds FindOutOfBounds (Rect parent, Rect kid) 
    {
        OutOfBounds offset;
        offset.left   = Mathf.Max(parent.min.x - kid.min.x, 0); 
        offset.right  = Mathf.Max(kid.max.x - parent.max.x, 0); 
        offset.bottom = Mathf.Max(parent.min.y - kid.min.y, 0); 
        offset.top    = Mathf.Max(kid.max.y - parent.max.y, 0); 
        return offset;
    }

}

}
