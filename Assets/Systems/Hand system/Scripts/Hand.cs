using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;





namespace VirtualLab.HandSystem 
{

public class Hand : MonoBehaviour 
{
    [SerializeField] HandSurface handSurface; 

    CurrentItem currentItem; 
    HandOver handOver; 
    HandTouch handTouch; 
    HandDrag handDrag; 



    void Awake () 
    {
        currentItem = new CurrentItem(); 
        handOver    = new HandOver(currentItem); 
        handTouch   = new HandTouch (); 
        handDrag    = new HandDrag (handSurface); 

        handDrag.onStart += () => onDragStart.Invoke(); 
        handDrag.onStop  += () => onDragEnd.Invoke(); 
    }

    void Update () 
    {
        currentItem.Update(); 

        StartInteractions(); 

        if (handOver.active)  handOver.Update(); 
        if (handTouch.active) handTouch.Update(); 
        if (handDrag.active)  handDrag.Update(); 
    }




    //  Events  ----------------------------------------------------- 
    public UnityEvent onDragStart; 
    public UnityEvent onDragEnd; 



    //  Interactions  ----------------------------------------------- 
    void StartInteractions () 
    {
        if (!handOver.active && currentItem.handOverItem != null) 
        {
            handOver.Start(currentItem.handOverItem); 
        }

        if (!handTouch.active && currentItem.touchItem != null) 
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                handTouch.Start(currentItem.touchItem); 
            }
        }

        if (!handDrag.active && currentItem.dragItem != null) 
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                handDrag.Start(currentItem.dragItem); 
            }
        }
    }

}

}
