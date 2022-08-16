using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.EventSystems; 



namespace VirtualLab 
{

public class FloatingPanel : MonoBehaviour, IBeginDragHandler, IDragHandler 
{
    RectTransform rectTransform; 



    void Awake () 
    {
        rectTransform = GetComponent<RectTransform>(); 
        SaveStartPoint(); 
    }

    void Update () 
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            GoToStartPoint(); 
        }
    }



    //  Events  ----------------------------------------------------- 
    public void OnParentSizeChagned (Vector2 oldSize, Vector2 newSize) 
    {
        Vector2 sizeChange = newSize / oldSize; 

        startPoint *= sizeChange; 
        offset *= sizeChange; 
        position *= sizeChange; 
    }

    public void OnOutOfBounds (BoundsObserver.Info boundsInfo) 
    {
        position += boundsInfo.toInside; 
    }



    //  Start point  ------------------------------------------------ 
    Vector2 startPoint; 

    void SaveStartPoint () 
    {
        startPoint = position; 
    }

    public void GoToStartPoint () 
    {
        position = startPoint; 
    }



    //  Drag  ------------------------------------------------------- 
    public void OnBeginDrag (PointerEventData eventData)
    {
        SaveOffset(eventData.position); 
    }

    public void OnDrag (PointerEventData eventData) 
    {
        if (eventData.dragging) 
        {
            MoveTo(eventData.position); 
        }
    }



    //  Movement  ------------------------------------------------------- 
    Vector2 offset; 

    void SaveOffset (Vector2 mousePos) 
    {
        offset = position - mousePos; 
    }

    void MoveTo (Vector2 mousePos) 
    {
        position = mousePos + offset; 
    }

    Vector2 position 
    {
        get => rectTransform.anchoredPosition; 
        set => rectTransform.anchoredPosition = value; 
    }

}

}
