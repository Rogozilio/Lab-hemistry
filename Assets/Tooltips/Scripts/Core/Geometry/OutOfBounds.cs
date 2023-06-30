using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public struct OutOfBounds 
{
    public float left;
    public float right;
    public float bottom;
    public float top;


    public OutOfBounds (float left, float right, float bottom, float top)
    {
        this.left   = left;
        this.right  = right;
        this.bottom = bottom;
        this.top    = top;
    }


    public Vector2 toInside 
    {
        get {
            Vector2 toInside = new Vector2(); 

            toInside.x += left; 
            toInside.x -= right; 
            if (left > 0 && right > 0) toInside.x /= 2; 

            toInside.y += bottom; 
            toInside.y -= top; 
            if (top > 0 && bottom > 0) toInside.y /= 2; 

            return toInside; 
        }
    }
}

}