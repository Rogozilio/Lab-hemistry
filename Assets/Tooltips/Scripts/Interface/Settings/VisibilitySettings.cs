using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.Tooltips
{

[System.Serializable]
public class VisibilitySettings 
{
    [Tooltip("CanvasGroup для этой выноски, отвечающий для прозрачность всей выноски")]
    public CanvasGroup canvasGroup;
    [Tooltip("Длительность анимации появления и исчезания выноски")]
    public float fadeTime = 0.15f;
    [Tooltip("Отображается ли выноска всегда, независимо от внешних воздействий")]
    public bool alwaysVisible = false;

    public bool isReady => canvasGroup;
}

}
