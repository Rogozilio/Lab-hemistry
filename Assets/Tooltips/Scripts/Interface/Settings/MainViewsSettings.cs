using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.Tooltips.Core
{

[System.Serializable]
public class MainViewsSettings 
{
    [Tooltip("Прямоугольник выноски")]
    public Core.MainArea mainArea;
    [Tooltip("Линия от прямоугольника до основания выноски")]
    public Core.LineToOrigin lineToOrigin;
    [Tooltip("Круг в основании выноски")]
    public Core.OriginDot originDot;

    public bool isReady => mainArea && lineToOrigin && originDot;
}

}
