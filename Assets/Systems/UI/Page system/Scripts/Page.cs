using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace ERA.SidePanelAsset {

public class Page : MonoBehaviour
{
	[SerializeField] Image imagePart;


    public void Init (Sprite pagePic) 
    {
        imagePart.sprite = pagePic; 
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
