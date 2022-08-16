using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace VirtualLab 
{

public class DragItem : HandSystem.AbstractDragItem  
{
    Vector3 startPoint; 



    void Start () 
    {
        startPoint = transform.position; 
    }



    //  Events  ----------------------------------------------------- 
    public UnityEvent onDragStart; 
    public UnityEvent<Vector3> onDrag; 
    public UnityEvent onDragEnd; 



    //  Info  ------------------------------------------------------- 
    public bool isDragging { get; private set; } 



    //  Drag  ------------------------------------------------------- 
    public override void OnDragStart () 
    {
        isDragging = true; 
        onDragStart.Invoke(); 
    }

    public override void OnDrag (Vector3 handPosition) 
    {
        transform.position = handPosition; 
        onDrag.Invoke(handPosition); 
    }

    public override void OnDragEnd ()
    {
        isDragging = false; 
        onDragEnd.Invoke(); 
    }



    //  Actions  ---------------------------------------------------- 
    public void GoToStartPoint () 
    {
        transform.position = startPoint; 
    }

}

}
