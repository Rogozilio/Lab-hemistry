using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

public abstract class View : MonoBehaviour
{
    [SerializeField] protected RectTransform rectTransform;



    //  Life cycle  -------------------------------------------------
    public void Init (Geometry geometry) 
    {
        this.geometry = geometry;
    }

    public abstract void StartMe ();
    public abstract void UpdateMe ();



    //  Parts  ------------------------------------------------------
    protected Geometry geometry;



    //  Info  -------------------------------------------------------
    protected bool isReady => rectTransform && geometry != null;

}

}
