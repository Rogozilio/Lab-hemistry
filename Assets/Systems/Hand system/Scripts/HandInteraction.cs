using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace VirtualLab.HandSystem 
{

public abstract class HandInteraction 
{
    public bool active { get; protected set; } 



    public delegate void EventHandler (); 
    public event EventHandler onStart = delegate {}; 
    public event EventHandler onStop = delegate {}; 



    public virtual void Start (HandItem handItem) 
    {
        active = true; 
        onStart(); 
    }

    public virtual void Update () {} 

    public virtual void Stop () 
    {
        active = false; 
        onStop(); 
    }
}

}