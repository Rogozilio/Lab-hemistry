using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ERA.Tooltips.Core;



namespace ERA
{

/// <summary>
///     Система для информационных выносок. Содержит список выносок,
///     загружает данные для выносок из json файла, затем передает эти данные выноскам. <br/>
///     Каждая выноска использует эту систему для доступа к камере (для расчета положения на экране) и других целей. <br/>
/// </summary>
public class TooltipSystem : MonoBehaviour
{
    [Tooltip("Камера, используемая для расчета положения выносок на экране")]
    [SerializeField] Camera _camera;
    [Tooltip("Префаб для графического объекта типа Title")]
    public GameObject titleViewPrefab;
    [Tooltip("Префаб для графического объекта типа Text")]
    public GameObject textViewPrefab;
    [Tooltip("Префаб для графического объекта типа Progress")]
    public GameObject progressViewPrefab;



    void Awake () 
    {
        FindTooltips();
        StartCoroutine(InitTooltips());
    }



    //  Data  -------------------------------------------------------
    List<Tooltip> tooltips = new List<Tooltip>();

    void FindTooltips () 
    {
        GetComponentsInChildren<Tooltip>(true, tooltips);
    }



    //  State  ------------------------------------------------------
    public bool settingsReady =>
        _camera && 
        titleViewPrefab && 
        textViewPrefab && 
        progressViewPrefab;



    //  Init data  --------------------------------------------------
    IEnumerator InitTooltips () 
    {
        // load tooltips info 
        var result = new Result<TooltipsInfo>();
        var loading = TooltipsInfoLoader.Load(this, result);
        yield return StartCoroutine(loading);

        // check results 
        if (!result.success) yield break;

        // init tooltips 
        GiveDataToTooltips(result.data);
    }

    void GiveDataToTooltips (TooltipsInfo tooltipsInfo) 
    {
        foreach (var info in tooltipsInfo.tooltips) 
        {
            var tooltip = tooltips.Find(
                (tooltip) => tooltip.name == info.name
            );

            if (tooltip != null) GiveDataToTooltip(tooltip, info);
        }
    }

    void GiveDataToTooltip (Tooltip tooltip, TooltipsInfo.Tooltip info) 
    {
        tooltip.dataList.Clear();

        foreach (var d in info.data) 
        {
            TooltipData data = new TooltipData(d);
            tooltip.dataList.Add(data);
        }
    }



    //  Geometry  ---------------------------------------------------
    public new Camera camera => _camera;



    //  Prefabs  ----------------------------------------------------
    public GameObject GetPrefabForData (TooltipData.Type type) 
    {
        switch (type) 
        {
            case TooltipData.Type.Title:    return titleViewPrefab;
            case TooltipData.Type.Text:     return textViewPrefab;
            case TooltipData.Type.Progress: return progressViewPrefab;
            default:                 throw new UnityException("Data.Type " + type + " not found when creating data view");
        }
    }

}

}
