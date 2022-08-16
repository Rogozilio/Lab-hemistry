using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 
using UnityEngine.UI; 





namespace VirtualLab {

[ExecuteInEditMode] 
public class ImageAspectRatio : UnityEngine.EventSystems.UIBehaviour 
{
    public enum Mode { WidthControlsHeight, HeightControlsWidth } 
    [SerializeField] Mode mode; 



    //  UI events  -------------------------------------------------- 
    protected override void OnRectTransformDimensionsChange ()
    {
        if (!HasConnections()) InitConnections(); 
        if (!HasConnections()) return; 

        UpdateAspectRatio(); 
    }



    //  Connections  ------------------------------------------------ 
    RectTransform rectTransform; 
    Image image;

    bool HasConnections () 
    {
        return image != null && rectTransform != null; 
    }

    void InitConnections () 
    {
        rectTransform = GetComponent<RectTransform>(); 
        image = GetComponent<Image>(); 
    }



    //  Update  ----------------------------------------------------- 
    public void UpdateAspectRatio () 
    {
        if (image.sprite == null) return; 

        float width = image.sprite.rect.width; 
        float height = image.sprite.rect.height; 
        float aspectRatio = width / height; 

        float newWidth; 
        float newHeight; 
        switch (mode) 
        {
            case Mode.WidthControlsHeight: 
                newWidth = rectTransform.sizeDelta.x; 
                newHeight = newWidth / width * height; 
                break; 
            case Mode.HeightControlsWidth: 
                newHeight = rectTransform.sizeDelta.y; 
                newWidth = newHeight / height * width; 
                break; 
            default: 
                throw new UnityException("Not supported yet"); 
        }

        rectTransform.sizeDelta = new Vector2(newWidth, newHeight); 
    }


}

}
