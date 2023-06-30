using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public abstract class Settings 
{
    public abstract bool isReady { get; }
    public abstract void Init (MonoBehaviour tooltip);
}

}