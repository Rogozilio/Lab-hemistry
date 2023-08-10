using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems; 



namespace ERA.SidePanelAsset 
{

public class PictureFitter : MonoBehaviour 
{
    [SerializeField] RectTransform container; 
    [SerializeField] GameObject fitAllButton; 
    [SerializeField] GameObject fitWidthButton; 
    [SerializeField] Mode _mode; 

    RectTransform rectTransform; 
    Image image; 



    void Awake () 
    {
        rectTransform = GetComponent<RectTransform>(); 
        image = GetComponent<Image>(); 
    }

    void Update ()
    {
        if (transform.hasChanged || container.hasChanged) 
        {
            UpdateFit(); 

            transform.hasChanged = false; 
            container.hasChanged = false; 
        }
    }



    //  Mode -------------------------------------------------------- 
    public enum Mode { FitAll, FitWidth } 

    public Mode mode 
    {
        get => _mode; 
        set {
            _mode = value; 
            UpdateFit(); 
        }
    }

    public void SetFitMode (int fitMode) 
    {
        this.mode = (Mode) fitMode; 
    }

    public void UpdateFit () 
    {
        if (image != null && image.sprite == null) return; 

        switch (mode) 
        {
            case Mode.FitAll: 
                if (CanFitHeigt()) FitHeight(); 
                else               FitWidth(); 
                break; 
            case Mode.FitWidth: 
                FitWidth(); 
                break; 
        }

        UpdateButtons(); 
    }



    //  Buttons  ---------------------------------------------------- 
    void UpdateButtons () 
    {
        if (mode == Mode.FitAll) 
        {
            fitAllButton.SetActive(false); 
            fitWidthButton.SetActive(true); 
        }
        else 
        {
            fitAllButton.SetActive(true); 
            fitWidthButton.SetActive(false); 
        }
    }
 


    //  Info  ------------------------------------------------------- 
    float imageWidth => image.sprite.rect.width; 
    float imageHeight => image.sprite.rect.height; 
    float containerWidth => container.rect.width; 
    float containerHeight => container.rect.height; 

    bool CanFitHeigt () 
    {
        float height = containerHeight; 
        float width = imageWidth / imageHeight * height; 
        return width <= containerWidth; 
    }



    //  Actions  ---------------------------------------------------- 
    void FitWidth () 
    {
        rectTransform.sizeDelta = new Vector2(
            containerWidth, 
            imageHeight / imageWidth * containerWidth 
        ); 
    }

    void FitHeight () 
    {
        rectTransform.sizeDelta = new Vector2(
            imageWidth / imageHeight * containerHeight, 
            containerHeight 
        ); 
    }

}

}
