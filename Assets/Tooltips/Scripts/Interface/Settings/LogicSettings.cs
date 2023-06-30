using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

[System.Serializable]
public class LogicSettings : Settings
{    
    [Tooltip("Длительность анимации появления и исчезания выноски")]
    public float fadeTime = 0.15f;
    [Tooltip("Отображается ли выноска всегда, независимо от внешних воздействий")]
    public bool alwaysVisible = false;


    public override bool isReady => true;

    public override void Init (MonoBehaviour tooltip) 
    {
        
    }

}

}
