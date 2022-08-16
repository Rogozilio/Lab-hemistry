using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

public class DistanceFromLeftSide : MonoBehaviour
{
    [SerializeField] float minDistance = 50; 

    RectTransform rectTransform; 



    void Awake () 
    {
        rectTransform = GetComponent<RectTransform>(); 
        InitOffset(); 
    }

    void LateUpdate () 
    {
        float distance = GetDistanceFromEdge(); 
        float change = Mathf.Max(minDistance - distance, 0); 

        SetOffsetChange(change); 
    }



    //  Distance  --------------------------------------------------- 
    float GetDistanceFromEdge () 
    {
        Vector3 [] corners = new Vector3[4]; 
        rectTransform.GetWorldCorners(corners); 

        return corners[0].x - offset + originalOffset; 
    }



    //  Offset  ----------------------------------------------------- 
    float originalOffset; 

    float offset 
    {
        get => rectTransform.offsetMin.x; 
        set => rectTransform.offsetMin = new Vector2(value, rectTransform.offsetMin.y); 
    }

    void InitOffset () 
    {
        originalOffset = offset; 
    }

    void SetOffsetChange (float change) 
    {
        offset = originalOffset + change; 
    }
    
}

}
