using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace VirtualLab.HandSystem 
{

class HandTouch : HandInteraction
{
    AbstractTouchItem item; 
    


    public override void Start (HandItem item) 
    {
        this.item = (AbstractTouchItem) item; 
        this.item.OnTouchStart(); 

        base.Start(item); 
    }

    public override void Update () 
    {
        if (Input.GetMouseButtonUp(0)) 
        {
            Stop(); 
            return; 
        }
        
        item.OnTouch(); 
    }

    public override void Stop () 
    {
        item.OnTouchEnd(); 
        base.Stop(); 
    }

}

}
