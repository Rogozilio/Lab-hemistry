using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.HandSystem 
{

public abstract class HandItem : MonoBehaviour
{
    public virtual bool active { get; set; } = true; 
}

public abstract class AbstractHandOverItem : HandItem 
{
    public virtual void OnHandOverStart () {} 
    public virtual void OnHandOver () {} 
    public virtual void OnHandOverEnd () {} 
}

public abstract class AbstractTouchItem : HandItem 
{
    public virtual void OnTouchStart () {} 
    public virtual void OnTouch () {} 
    public virtual void OnTouchEnd () {} 
}

public abstract class AbstractDragItem : HandItem 
{
    public virtual void OnDragStart () {} 
    public virtual void OnDrag (Vector3 position) {} 
    public virtual void OnDragEnd () {} 
}

}
