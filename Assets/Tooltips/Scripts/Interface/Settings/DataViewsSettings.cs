using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace VirtualLab.Tooltips
{

[System.Serializable]
public class DataViewsSettings 
{
    [Tooltip("Объект, в котором содержатся графические объекты данных выноски")]
    public Transform dataViewsContainer;
    [Tooltip("Префаб для графического объекта типа Title")]
    public GameObject titleViewPrefab;
    [Tooltip("Префаб для графического объекта типа Text")]
    public GameObject textViewPrefab;
    [Tooltip("Префаб для графического объекта типа Progress")]
    public GameObject progressViewPrefab;


    public bool isReady => 
        dataViewsContainer && 
        titleViewPrefab && 
        textViewPrefab && 
        progressViewPrefab;


    public GameObject GetDataPrefab (Data.Type type) 
    {
        switch (type) 
        {
            case Data.Type.Title:    return titleViewPrefab;
            case Data.Type.Text:     return textViewPrefab;
            case Data.Type.Progress: return progressViewPrefab;
            default:                 throw new UnityException("Data.Type " + type + " not found when creating data view");
        }
    }
}

}
