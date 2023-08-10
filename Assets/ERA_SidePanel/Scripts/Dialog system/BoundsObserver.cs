using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events; 



namespace ERA.SidePanelAsset
{

public class BoundsObserver : MonoBehaviour 
{
    RectTransform rt; 



    void Awake () 
    {
        rt = GetComponent<RectTransform>(); 
        InitParentRT(); 
    }

    void LateUpdate () 
    {
        UpdateParentRT(); 

        Info info = CreateBoundsInfo(); 
        if (info.isOutOfBounds) onOutOfBounds.Invoke(info); 
    }



    //  Events  ----------------------------------------------------- 
    public UnityEvent<Info> onOutOfBounds; 



    //  Parent RT  -------------------------------------------------- 
    RectTransform parentRT; 

    void InitParentRT () 
    {
        parentRT = transform.parent.GetComponent<RectTransform>(); 
    }

    void UpdateParentRT () 
    {
        if (parentRT.transform != transform.parent) 
        {
            InitParentRT(); 
        }
    }



    //  Checking bounds  -------------------------------------------- 
    Info CreateBoundsInfo () 
    {
        Rect me = CreateRect(rt); 
        Rect parent = CreateRect(parentRT); 

        Info info = new Info(); 

        info.outToLeft  = Mathf.Max(parent.min.x - me.min.x, 0); 
        info.outToRight = Mathf.Max(me.max.x - parent.max.x, 0); 
        info.outToDown  = Mathf.Max(parent.min.y - me.min.y, 0); 
        info.outToUp    = Mathf.Max(me.max.y - parent.max.y, 0); 

        info.isOutOfBounds = FindIsOutOfBounds(info); 
        info.toInside = FindToInside(info); 

        return info; 
    }

    bool FindIsOutOfBounds (Info info) 
    {
        return 
            info.outToLeft > 0 || 
            info.outToRight > 0 || 
            info.outToDown > 0 || 
            info.outToUp > 0; 
    }

    Vector2 FindToInside (Info info) 
    {
        Vector2 toInside = new Vector2(); 

        toInside.x += info.outToLeft; 
        toInside.x -= info.outToRight; 
        if (info.outToLeft > 0 && info.outToRight > 0) toInside.x /= 2; 

        toInside.y += info.outToDown; 
        toInside.y -= info.outToUp; 
        if (info.outToUp > 0 && info.outToDown > 0) toInside.y /= 2; 

        return toInside; 
    }

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



    //  Classes  ---------------------------------------------------- 
    public class Info 
    {
        public bool isOutOfBounds; 

        public float outToLeft; 
        public float outToRight; 
        public float outToUp; 
        public float outToDown; 

        public Vector2 toInside; 
    }

}

}
