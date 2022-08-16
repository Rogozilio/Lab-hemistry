using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.HandSystem 
{

public class HandOver : HandInteraction 
{
    CurrentItem currentItem; 
    AbstractHandOverItem item; 



    public HandOver (CurrentItem currentItem) 
    {
        this.currentItem = currentItem; 
    }



    public void OnItemLost (HandItem item) 
    {
        Stop(); 
    }



    public override void Start (HandItem item) 
    {
        this.item = (AbstractHandOverItem) item; 
        this.item.OnHandOverStart(); 
        
        currentItem.onItemLost += OnItemLost; 
        base.Start(item); 
    }

    public override void Update () 
    {
        item.OnHandOver(); 
    }

    public override void Stop () 
    {
        item.OnHandOverEnd(); 

        currentItem.onItemLost -= OnItemLost; 
        base.Stop(); 
    }

}

}
