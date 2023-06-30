using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public class Geometry 
{
    UISettings uiSettings;
    GeometrySettings geometrySettings;



    public Geometry (MainSettings mainSettings, UISettings uiSettings, GeometrySettings geometrySettings) 
    {
        this.uiSettings       = uiSettings;
        this.geometrySettings = geometrySettings;

        this.origin   = new Origin(mainSettings, geometrySettings);
        this.position = new Position(uiSettings, geometrySettings, origin);
    }



    //  Data  -------------------------------------------------------
    public Position position { get; private set; }
    public Origin   origin   { get; private set; }



    //  Life cycle  -------------------------------------------------
    public bool isReady => geometrySettings.isReady;
    
    public void Update ()
    {
        origin.Update();
        position.Update();
    }



    //  Rect transform  ---------------------------------------------
    public RectTransform rectTransform => uiSettings.rectTransform;

    public Rect rectWorld 
    {
        get {
            Vector3 [] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Vector3 min = corners[0];
            Vector3 max = corners[3];
            return new Rect(min, max - min);
        }
    }

    public bool isOriginInsideRectTransform => IsInsideRectTransform(origin.point);

    public bool IsInsideRectTransform (Vector2 point) 
    {
        if (rectTransform.rotation.z == 0) return IsInsideRectTransform_Straight(point);
        else                               return IsInsideRectTransform_Rotated(point);
    }

    bool IsInsideRectTransform_Straight (Vector2 point) 
    {
        Vector2 pointLocal = point - position.point;
        return rectTransform.rect.Contains(pointLocal);
    }

    bool IsInsideRectTransform_Rotated (Vector2 point) 
    {
        Vector3 [] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector2 a1 = corners[0];
        Vector2 a2 = corners[1];
        Vector2 a3 = corners[2];

        Vector2 b1 = corners[0];
        Vector2 b2 = corners[2];
        Vector2 b3 = corners[3];

        return 
            IsInsideTriangle(a1, a2, a3, point) ||
            IsInsideTriangle(b1, b2, b3, point);
    }

    bool IsInsideTriangle (Vector2 a, Vector2 b, Vector2 c, Vector2 point) 
    {
        Vector2 ab = b - a;
        Vector2 bc = c - b;
        Vector2 ca = a - c;

        Vector2 ap = point - a;
        Vector2 bp = point - b;
        Vector2 cp = point - c;

        float sideAB = ab.x * ap.y - ab.y * ap.x;
        float sideBC = bc.x * bp.y - bc.y * bp.x;
        float sideCA = ca.x * cp.y - ca.y * cp.x;

        return 
            sideAB <= 0 && 
            sideBC <= 0 && 
            sideCA <= 0;
    }



    //  Tech  -------------------------------------------------------
    public Side originSide 
    {
        get {
            Vector2 o = origin.point;
            Vector2 p = position.point;

            if (p == o) return Side.Center;

            Rect rect = rectWorld;

            if (o.x >= rect.xMin && o.x <= rect.xMax) 
            {
                if (o.y > p.y) return Side.Top;
                else                       return Side.Bottom;
            }
            else if (o.y >= rect.yMin && o.y <= rect.yMax) 
            {
                if (o.x > p.x) return Side.Right;
                else                       return Side.Left;
            }
            else 
            {
                Corner corner = originCorner;
                Vector2 cornerPos = GetCornerPosition(corner);
                Vector2 cornerToOrigin = origin.point - cornerPos;
                float xy = Mathf.Abs(cornerToOrigin.x / cornerToOrigin.y);

                switch (corner) 
                {
                    case Corner.BottomLeft: 
                        if (xy >= 1) return Side.Left;
                        else         return Side.Bottom;
                    case Corner.TopLeft: 
                        if (xy >= 1) return Side.Left;
                        else         return Side.Top;
                    case Corner.TopRight: 
                        if (xy >= 1) return Side.Right;
                        else         return Side.Top;
                    case Corner.BottomRight: 
                        if (xy >= 1) return Side.Right;
                        else         return Side.Bottom;
                    default:         return Side.Center;
                }
            }
        }
    }

    public Corner originCorner 
    {
        get {
            Vector2 p = origin.point - position.point;

            if (p.x >  0 && p.y >  0) return Corner.TopRight;
            if (p.x <= 0 && p.y >  0) return Corner.TopLeft;
            if (p.x >  0 && p.y <= 0) return Corner.BottomRight;
            if (p.x <= 0 && p.y <= 0) return Corner.BottomLeft;
            else                      return Corner.None;
        }
    }

    public Vector2 GetCornerPosition (Corner corner) 
    {
        Vector3 [] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        switch (corner) 
        {
            case Corner.BottomLeft:  return corners[0];
            case Corner.TopLeft:     return corners[1];
            case Corner.TopRight:    return corners[2];
            case Corner.BottomRight: return corners[3];
            default:                 return Vector2.zero;
        }
    }

}

}
