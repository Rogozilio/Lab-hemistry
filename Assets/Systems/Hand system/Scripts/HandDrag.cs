using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace VirtualLab.HandSystem 
{

public class HandDrag : HandInteraction 
{
    AbstractDragItem item; 
    HandSurface handSurface; 



    public HandDrag (HandSurface handSurface) 
    {
        this.handSurface = handSurface; 
    }



    public override void Start (HandItem item) 
    {
        this.item = (AbstractDragItem) item; 
        this.item.OnDragStart(); 

        handSurface.SetSurface(item.transform.position); 

        base.Start(item); 
    }

    public override void Update () 
    {
        if (Input.GetMouseButtonUp(0)) 
        {
            Stop(); 
            return; 
        }

        Vector3 handPosition = handSurface.GetPoint(Input.mousePosition); 
        item.OnDrag(handPosition); 
    }

    public override void Stop () 
    {
        item.OnDragEnd(); 
        base.Stop(); 
    }

}

}
