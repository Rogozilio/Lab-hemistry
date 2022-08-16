using System.Collections;
using System.Collections.Generic;
using UnityEngine; 



namespace VirtualLab.PlayerMotion 
{

public abstract class AbstractNavPoints : MonoBehaviour 
{
	public abstract int pointCount { get; } 
    public abstract NavPoint GetPoint (int pointID); 

}

}
