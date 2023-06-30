using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace ERA.Tooltips
{

#if UNITY_EDITOR
public static class TechGUI 
{

    //  Parameters  -------------------------------------------------
    const int MARGIN = 5;



    //  Background  -------------------------------------------------
    public static Rect CreateContentArea (Rect area) 
    {
        return new Rect(
            area.x + MARGIN,
            area.y + MARGIN,
            area.width  - 2 * MARGIN,
            area.height - 2 * MARGIN
        );
    }

    public static void DrawBackgroundRect (Rect area) 
    {
        Color outlineColor = new Color(0.5f, 0.5f, 0.5f);

        float x1 = area.xMin;
        float x2 = area.xMax;
        float y1 = area.yMin;
        float y2 = area.yMax;

        float width = x2 - x1;
        float height = y2 - y1;

        Rect ab = new Rect(x1, y1, 1, height);
        Rect cd = new Rect(x2, y1, 1, height);
        Rect bc = new Rect(x1, y2, width, 1);
        Rect ca = new Rect(x1, y1, width, 1);

        EditorGUI.DrawRect(ab, outlineColor);
        EditorGUI.DrawRect(cd, outlineColor);
        EditorGUI.DrawRect(bc, outlineColor);
        EditorGUI.DrawRect(ca, outlineColor);
    }

}
#endif

}
