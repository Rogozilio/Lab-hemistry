using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace VirtualLab {

public class Page : MonoBehaviour
{

    public void Init (Sprite pagePic) 
    {
        Image imageComponent = GetComponent<Image>(); 
        imageComponent.sprite = pagePic; 

        ImageAspectRatio aspectRatio = GetComponent<ImageAspectRatio>(); 
        aspectRatio.UpdateAspectRatio(); 
    }

    public void AddButton (RectTransform button, int x, int y) 
    {
        Image image = GetComponent<Image>(); 
        float width = image.sprite.rect.width; 
        float height = image.sprite.rect.height; 

        Vector2 position = new Vector2(
            x / width, 
            (height - y) / height 
        ); 

        button.SetParent(transform, false); 
        button.anchorMin = position; 
        button.anchorMax = position; 
    }

}

}
