using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.Tooltips
{

[System.Serializable]
public class OriginSettings 
{
    [Tooltip("Объект в мире, к которому прикреплена эта выноска. Точка основания выноски следует за этим объектом, но только в режиме игры. В режиме редактирования основание выноски находится в точке pointEditMode")]
    public Transform worldPoint;
    [Tooltip("Точка основания выноски, испозуется только в режиме редактирования. В режиме игры выноска следует за объектом worldPoint")]
    public Vector2 pointEditMode;
    [Tooltip("Отступ от проекции объекта worldPoint на экран до точки основания выноски, в пикселях")]
    public Vector2 offsetScreenSpace;
    [Tooltip("Отступ от центра объекта worldPoint, в мировом пространстве")]
    public Vector3 offsetWorldSpace;
    [Tooltip("Отступ от центра объекта worldPoint, в пространстве объекта")]
    public Vector3 offsetLocalSpace;
}

}
