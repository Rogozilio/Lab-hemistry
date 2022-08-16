using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 





namespace VirtualLab 
{

public class PageControls : MonoBehaviour
{
    [SerializeField] TabSystem tabSystem; 
    [SerializeField] Text textComponent; 
    [SerializeField] Button buttonPrevious; 
    [SerializeField] Button buttonNext; 



    void Awake () 
    {
        InitTabEvents(); 
    }

    void Start () 
    {
        UpdateUI(); 
    }

    void OnDestroy () 
    {
        pageSystem = null; 
        ClearTabEvents(); 
    }



    //  Page system  ------------------------------------------------ 
    PageSystem _pageSystem = null; 
    PageSystem pageSystem 
    {
        get { return _pageSystem; }
        set {
            if (_pageSystem != null) 
            {
                _pageSystem.onPageChanged -= OnPageChanged; 
                _pageSystem.onPageCountChanged -= OnPageCountChanged; 
            }

            _pageSystem = value; 

            if (_pageSystem != null) 
            {
                _pageSystem.onPageChanged += OnPageChanged; 
                _pageSystem.onPageCountChanged += OnPageCountChanged; 
            }
            
            UpdateUI(); 
        }
    }

    void OnPageChanged (int pageNumber) 
    {
        UpdateUI(); 
    }

    void OnPageCountChanged (int count) 
    {
        UpdateUI(); 
    }



    //  Tab system  ------------------------------------------------- 
    void InitTabEvents () 
    {
        tabSystem.onTabChanged += OnTabChanged; 
    }

    void ClearTabEvents () 
    {
        tabSystem.onTabChanged -= OnTabChanged; 
    }

    public void OnTabChanged (GameObject newTab) 
    {
        PageSystem pageSystem = newTab.GetComponentInChildren<PageSystem>(); 
        if (pageSystem == null) throw new UnityException("New tab doesn't contain a page system"); 

        this.pageSystem = pageSystem; 
    }



    //  UI  --------------------------------------------------------- 
    void UpdateUI () 
    {
        if (pageSystem != null) 
        {
            textComponent.text = 
                (pageSystem.currentPage + 1).ToString() + 
                "/" + 
                pageSystem.pageCount.ToString(); 

            buttonPrevious.interactable = pageSystem.currentPage > 0; 
            buttonNext.interactable = pageSystem.currentPage < pageSystem.pageCount - 1; 
        }
        else 
        {
            textComponent.text = "-/-"; 
            buttonPrevious.interactable = false; 
            buttonNext.interactable = false; 
        }
    }



    public void SwitchToNextPage () 
    {
        if (pageSystem.currentPage >= pageSystem.pageCount - 1) 
        {
            Debug.LogWarning("Attempting to move out of page range"); 
            return; 
        }

        pageSystem.currentPage += 1; 
    }

    public void SwitchToPreviousPage () 
    {
        if (pageSystem.currentPage <= 0) 
        {
            Debug.LogWarning("Attempting to move out of page range"); 
            return; 
        }
        
        pageSystem.currentPage -= 1; 
    }

}

}
