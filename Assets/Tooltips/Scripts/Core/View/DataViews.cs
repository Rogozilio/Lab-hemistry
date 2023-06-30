using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace ERA.Tooltips.Core
{

public class DataViews 
{
    MainSettings mainSettings;
    UISettings uiSettings;
    TooltipDataList dataList;
    Geometry geometry;



    public DataViews (MainSettings mainSettings, UISettings uiSettings, TooltipDataList dataList, Geometry geometry) 
    {
        this.mainSettings = mainSettings;
        this.uiSettings   = uiSettings;
        this.dataList     = dataList;
        this.geometry     = geometry;
    }



    //  Life cycle  -------------------------------------------------
    bool isReady => 
        mainSettings.isReady && 
        uiSettings.isReady && 
        geometry.isReady;

    bool isInsidePrefab 
    {
        get {
#if UNITY_EDITOR
            return UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null;
#else 
            return false;
#endif
        }
    }

    public void Update () 
    {
        if (isInsidePrefab) return;
        if (isReady) 
        {
            SyncViewsWithData();
            foreach (var view in views) view.UpdateMe();
        }
    }

    public void UpdateSettings () 
    {
        
    }



    //  Views  ------------------------------------------------------
    DataView [] views = new DataView[0];
    
    DataView CreateView (TooltipData data, int index) 
    {
        GameObject prefab = mainSettings.tooltipSystem.GetPrefabForData(data.type);
        GameObject go = GameObject.Instantiate(
            prefab, 
            Vector3.zero, 
            Quaternion.identity, 
            uiSettings.dataViewsContainer
        );
        go.transform.SetSiblingIndex(index);

        DataView view = go.GetComponent<DataView>();
        view.Init(geometry);
        view.data = data;

        return view;
    }

    void DestroyView (DataView view) 
    {
        DestroyObject(view.gameObject);
    }

    bool Contains (DataView view) 
    {
        foreach (DataView v in views) 
        {
            if (v == view) return true;
        }

        return false;
    }



    //  Actions  ----------------------------------------------------
    void SyncViewsWithData () 
    {
        DataView [] newViews = new DataView[dataList.Count];

        TransferUsedViews(dataList, views, newViews);
        views = newViews;

        CreateMissingViews(dataList, views);
        DeleteUnusedObjects(uiSettings.dataViewsContainer, views);
    }

    void TransferUsedViews (TooltipDataList dataList, DataView [] views, DataView [] newViews) 
    {
        for (int i = 0; i < dataList.Count; i++) 
        {
            TooltipData data = dataList[i];
            DataView view = i < views.Length ? views[i] : null;

            bool match = view && view.Match(data);
            if (match) newViews[i] = view;
        }
    }

    void CreateMissingViews (TooltipDataList dataList, DataView [] views) 
    {
        for (int i = 0; i < dataList.Count; i++) 
        {
            TooltipData data = dataList[i];
            DataView view = i < views.Length ? views[i] : null;

            if (!view) views[i] = CreateView(data, i);
        }
    }

    void DeleteUnusedObjects (Transform viewsContainer, DataView [] views) 
    {
        var objectsToDelete = new List<GameObject>();

        foreach (Transform kid in uiSettings.dataViewsContainer) 
        {
            DataView view = kid.GetComponent<DataView>();

            bool isUsed = view && Contains(view);
            if (!isUsed) objectsToDelete.Add(kid.gameObject);
        }

        foreach (var obj in objectsToDelete) 
        {
            DestroyObject(obj);
        }
    }



    //  Tech  -------------------------------------------------------
    void DestroyObject (GameObject obj) 
    {
        if (Application.isPlaying) GameObject.Destroy(obj);
        else                       GameObject.DestroyImmediate(obj);
    }

}

}
