using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; 
using VirtualLab.ApplicationData; 



namespace VirtualLab 
{

public class PictureView : MonoBehaviour
{
    // parameters 
    [Header("UI connections")] 
    [SerializeField] GameObject contentPanel; 
    [SerializeField] ScrollRect scrollView; 
    [SerializeField] Text nameComponent; 
    [SerializeField] Image imageComponent; 

    [Header("Buttons")]
    [SerializeField] PageSystem aboutTab; 
    [SerializeField] PageSystem theoryTab; 
    [SerializeField] PageSystem instructionsTab; 
    [SerializeField] GameObject maximizeButtonPrefab; 




    void Awake () 
    {
        InitActivity(); 
    }

    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            HidePicture(); 
        }
    }



    //  Loading data  ----------------------------------------------- 
    public void OnPicturesLoaded (AppData data) 
    {
        foreach (PictureData pictureData in data.infoPanel.pictureView.pictures) 
        {
            CreateButton(pictureData); 
        }
    }

    void CreateButton (PictureData pictureData) 
    {
        GameObject buttonObj = Instantiate(maximizeButtonPrefab); 

        Button button = buttonObj.GetComponent<Button>(); 
        button.onClick.AddListener(
            () => { 
                ShowPicture(pictureData.picture); 
            } 
        ); 

        Page page = FindPage(pictureData.tab, pictureData.pageNumber); 
        page.AddButton(
            button.GetComponent<RectTransform>(), 
            pictureData.buttonX, 
            pictureData.buttonY
        ); 
    }

    Page FindPage (Tab tab, int pageNumber) 
    {
        PageSystem pageSystem = tab switch 
        {
            Tab.About        => aboutTab, 
            Tab.Theory       => theoryTab, 
            Tab.Instructions => instructionsTab, 
            _                => throw new UnityException("Not supported yet") 
        }; 

        try {
            return pageSystem.GetPage(pageNumber); 
        }
        catch 
        {
            throw new UnityException(
                "Не удалось найти страницу с номером " + (pageNumber + 1) + " для картинки" 
            ); 
        }
    }



    //  Events  ----------------------------------------------------- 
    [Space] 
    public UnityEvent onActivated; 
    public UnityEvent onDeactivated; 



    //  Showing picture  -------------------------------------------- 
    public void ShowPicture (Picture picture) 
    {
        nameComponent.text = picture.description; 

        imageComponent.sprite = picture.sprite; 
        imageComponent.rectTransform.ForceUpdateRectTransforms(); 
        
        SetActive(true); 
    }

    public void HidePicture () 
    {
        SetActive(false); 
    }



    //  Activity  --------------------------------------------------- 
    PictureFitter pictureFitter; 

	public bool active => contentPanel.activeSelf; 

    void InitActivity () 
    {
        pictureFitter = imageComponent.GetComponent<PictureFitter>(); 
    }

    void SetActive (bool newActive) 
    {
        if (active == newActive) 
		{
			pictureFitter.UpdateFit(); 
            scrollView.horizontalNormalizedPosition = 0.5f; 
            scrollView.verticalNormalizedPosition = 0.5f; 
		}
        else if (newActive) 
        {
            contentPanel.SetActive(true); 

            pictureFitter.UpdateFit(); 
            scrollView.horizontalNormalizedPosition = 0.5f; 
            scrollView.verticalNormalizedPosition = 0.5f; 

            onActivated.Invoke(); 
        }
        else 
        {
            contentPanel.SetActive(false); 

            onDeactivated.Invoke(); 
        }   
    }

}

}
