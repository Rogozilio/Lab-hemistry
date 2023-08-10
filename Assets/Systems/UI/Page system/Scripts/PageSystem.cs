using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ERA.SidePanelAsset;


namespace ERA.SidePanelAsset 
{

public class PageSystem : MonoBehaviour
{
    [SerializeField] Tab tab; 
    [SerializeField] Transform pageContainer; 
    [SerializeField] GameObject pagePrefab; 

    List<Page> pages = new List<Page>(); 



    //  Events  ----------------------------------------------------- 
	[Space]
    public UnityEvent<int> onPageChanged;
    public UnityEvent<int> onPageCountChanged;



    //  Loading data  ----------------------------------------------- 
    public void OnPagesLoaded (SidePanelData data) 
    {
        CreatePages(data.GetTabData(tab)); 
        InitPages(); 
    }

    void CreatePages (TabData data) 
    {
        foreach (Sprite sprite in data.pages) 
        {
            CreatePage(sprite); 
        }
    }

    void CreatePage (Sprite sprite) 
    {
        GameObject pageObj = Instantiate(pagePrefab); 
        pageObj.transform.SetParent(pageContainer); 

        RectTransform pageTransform = pageObj.GetComponent<RectTransform>(); 
        pageTransform.offsetMin = new Vector2(); 
        pageTransform.offsetMax = new Vector2(); 

        Page page = pageObj.GetComponent<Page>(); 
        page.Init(sprite); 

        pages.Add(page); 
    }



    //  Init  ------------------------------------------------------- 
    void InitPages () 
    {
        TurnOffAllPages(); 
        TurnOnFirstPage(); 

        onPageCountChanged.Invoke(pageCount); 
    }

    void TurnOffAllPages () 
    {
        for (int i = 0; i < pages.Count; i++) 
        {
            pages[i].gameObject.SetActive(false); 
        }
    }

    void TurnOnFirstPage () 
    {
        if (pages.Count > 0) 
        {
            _currentPage = 0; 
            pages[0].gameObject.SetActive(true); 
        }
    }



    //  Pages  ------------------------------------------------------ 
    public int pageCount => pages.Count; 

    int _currentPage = -1; 
    public int currentPage 
    {
        get { return _currentPage; }
        set {
            pages[_currentPage].gameObject.SetActive(false); 

            _currentPage = value; 
            pages[_currentPage].gameObject.SetActive(true); 
            
            onPageChanged.Invoke(_currentPage); 
        }
    }

    public Page GetPage (int i) 
    {
        return pages[i].GetComponent<Page>(); 
    }

}

}
