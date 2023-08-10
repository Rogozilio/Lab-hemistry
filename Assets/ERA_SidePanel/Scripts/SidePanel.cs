using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ERA.SidePanelAsset;



namespace ERA
{

public class SidePanel : MonoBehaviour
{
    [SerializeField] DialogSystem dialogSystem;
    [SerializeField] UnityEvent onDataLoaded;



    void Awake () 
    {
        InitConnections();
    }

    void Start () 
    {
        StartCoroutine(LoadData());
    }



    //  Connections  ------------------------------------------------
    TabSystem tabSystem;
    PageSystem about;
    PageSystem theory;
    PageSystem instructions;
    PictureView pictureView;

    void InitConnections () 
    {
        tabSystem    = GetComponentInChildren<TabSystem>();
        about        = transform.Find("Content area/Info panel/Tabs/Tab areas/About")       .GetComponent<PageSystem>();
        theory       = transform.Find("Content area/Info panel/Tabs/Tab areas/Theory")      .GetComponent<PageSystem>();
        instructions = transform.Find("Content area/Info panel/Tabs/Tab areas/Instructions").GetComponent<PageSystem>();
        pictureView  = transform.Find("Content area/Picture view")                          .GetComponent<PictureView>();
    }



    //  Loading data  -----------------------------------------------
    IEnumerator LoadData () 
    {
        var result = new Result<SidePanelData>();
        var loading = StartCoroutine(SidePanelLoader.Load(this, result));
        yield return loading;

        if (result.success) OnDataLoaded(result.data);
        else                OnError(result.message);
    }

    void OnDataLoaded (SidePanelData data) 
    {
        tabSystem   .OnDataLoaded(data);
        about       .OnPagesLoaded(data);
        instructions.OnPagesLoaded(data);
        theory      .OnPagesLoaded(data);
        pictureView .OnPicturesLoaded(data);
        
        onDataLoaded.Invoke();
    }

    void OnError (string message) 
    {
        dialogSystem.ShowErrorDialog(message);
    }

}

}