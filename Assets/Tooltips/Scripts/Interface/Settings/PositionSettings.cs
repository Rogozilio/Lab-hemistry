using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.Tooltips
{

[System.Serializable]
public class PositionSettings 
{
    [Tooltip("Отступ от точки основания выноски до центра выноски, в пикселях")]
    public Vector2 offsetFromOrigin;
}

}
