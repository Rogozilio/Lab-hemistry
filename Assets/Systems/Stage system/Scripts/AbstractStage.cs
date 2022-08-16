using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public abstract class AbstractStage : MonoBehaviour 
{
    // life cycle 
    public virtual void StartStage () {} 
    public virtual void StopStage () {} 
    public virtual void Restart () {} 
    
    // events 
    public delegate void EventHandler (AbstractStage stage); 
    public event EventHandler onCompleted = delegate {}; 
    protected void OnCompleted () { onCompleted(this); } 
}
