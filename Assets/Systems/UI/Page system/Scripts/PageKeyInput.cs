using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





namespace ERA.SidePanelAsset {

public class PageKeyInput : MonoBehaviour
{
    // parameters 
    [SerializeField] ScrollRect scrollRect; 
    [SerializeField] float scrollSpeed = 1; 
    [SerializeField] Button previousPageButton; 
    [SerializeField] Button nextPageButton; 
    


    void Update()
    {
        UpdateScrolling(); 
        UpdatePageSwitching(); 
    }



    //  Scrolling  -------------------------------------------------- 
    void UpdateScrolling () 
    {
        float input = GetScrollInput(); 
        
        if (input != 0) 
        {
			float height = scrollRect.content.sizeDelta.y; 
            float change = input * scrollSpeed / height * Time.deltaTime; 
			ChangeScrollPosition(change); 
        }
    }

    float GetScrollInput () 
    {
        float input = 0; 
        input += Input.GetKey(KeyCode.DownArrow) ? -1 : 0; 
        input += Input.GetKey(KeyCode.UpArrow)   ?  1 : 0; 
        return input; 
    }

	void ChangeScrollPosition (float change) 
	{
		float position = scrollRect.verticalNormalizedPosition; 

		position += change; 
		position = Mathf.Clamp01(position); 

		scrollRect.verticalNormalizedPosition = position; 
	}



    //  Page switching  --------------------------------------------- 
    void UpdatePageSwitching () 
    {
        if (
            Input.GetKeyDown(KeyCode.LeftArrow) && 
            !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
            previousPageButton.interactable 
        ) {
            previousPageButton.onClick.Invoke(); 
        }

        if (
            Input.GetKeyDown(KeyCode.RightArrow) && 
            !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
            nextPageButton.interactable 
        ) {
            nextPageButton.onClick.Invoke(); 
        }
    }

}

}
