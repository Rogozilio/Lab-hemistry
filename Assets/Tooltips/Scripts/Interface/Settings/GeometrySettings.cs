using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.Tooltips.Core
{

[System.Serializable]
public class GeometrySettings : Settings
{
    // Edit mode 
    [Tooltip("Точка основания выноски, испозуется только в режиме редактирования. В режиме игры выноска следует за объектом worldPoint")]
    public Vector2 editModePosition = new Vector2(-900, 500);
    
    //  Tooltip position 
    [Tooltip("Отступ от точки основания выноски до центра выноски, в пикселях")]
    public Vector2 idealOffsetFromOrigin;
    [Tooltip("Максимальное расстояние от основания выноски до центра выноски")]
    public float maxDistanceFromOrigin = 250;

    //  Tooltip origin 
    [Tooltip("Объект в мире, к которому прикреплена эта выноска. Точка основания выноски следует за этим объектом, но только в режиме игры. В режиме редактирования основание выноски находится в точке pointEditMode")]
    public Transform worldPoint;
    
    [Tooltip("Отступ от проекции объекта worldPoint на экран до точки основания выноски, в пикселях")]
    public Vector2 offsetScreenSpace;
    [Tooltip("Отступ от центра объекта worldPoint, в мировом пространстве")]
    public Vector3 offsetWorldSpace;
    [Tooltip("Отступ от центра объекта worldPoint, в пространстве объекта")]
    public Vector3 offsetLocalSpace;

    //  Main 
    [Tooltip("Ширина выноски в пикселях")]
    public float width = 250;


    public override bool isReady => worldPoint;

    public override void Init (MonoBehaviour tooltip) 
    {
        
    }
    
}

}
