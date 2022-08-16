using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using VirtualLab.ApplicationData; 



namespace VirtualLab 
{

public class PageSystem : MonoBehaviour
{
    [SerializeField] Tab tab; 
    [SerializeField] Transform pageContainer; 
    [SerializeField] GameObject pagePrefab; 

    List<Page> pages = new List<Page>(); 



    //  Events  ----------------------------------------------------- 
    public delegate void EventHandler(int number); 
    public event EventHandler onPageChanged = delegate {}; 
    public event EventHandler onPageCountChanged = delegate {}; 



    //  Loading data  ----------------------------------------------- 
    public void OnPagesLoaded (AppData appData) 
    {
        CreatePages(appData.infoPanel.GetTabData(tab)); 
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
        GameObject pageObj = Instantiate(
            pagePrefab, 
            new Vector3(0, 0, 0), 
            Quaternion.identity 
        ); 
        pageObj.transform.SetParent(pageContainer); 

        RectTransform pageTransform = pageObj.GetComponent<RectTransform>(); 
        pageTransform.anchorMin = new Vector2(); 
        pageTransform.anchorMax = new Vector2(); 
        pageTransform.anchoredPosition = new Vector2(); 

        Page page = pageObj.GetComponent<Page>(); 
        page.Init(sprite); 

        pages.Add(page); 
    }



    //  Init  ------------------------------------------------------- 
    void InitPages () 
    {
        TurnOffAllPages(); 
        TurnOnFirstPage(); 

        onPageCountChanged(pageCount); 
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
            
            onPageChanged(_currentPage); 
        }
    }

    public Page GetPage (int i) 
    {
        return pages[i].GetComponent<Page>(); 
    }

}

}
