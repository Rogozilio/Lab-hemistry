using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using UnityEngine.EventSystems; 



namespace VirtualLab {

public class TabButton : MonoBehaviour, IPointerClickHandler 
{
	// parameters 
    [SerializeField] GameObject tabArea; 
    [SerializeField] GameObject imageNormal; 
    [SerializeField] GameObject imageSelected; 



    //  Events  ----------------------------------------------------- 
    public delegate void EventHandler (TabButton button); 
    public event EventHandler onSelected = delegate {}; 
    public event EventHandler onDeselected = delegate {}; 

    public void OnPointerClick (PointerEventData eventData) 
	{
		Select(); 
	}



    //  State  ------------------------------------------------------ 
    public bool isSelected { get; private set; } 

    public void Select () 
    {
        if (isSelected) return; 
        
        SetState(true); 
        onSelected(this); 
    }

    public void Deselect () 
    {
        if (!isSelected) return; 
        
        DeselectQuietly(); 
        onDeselected(this); 
    }

    public void DeselectQuietly () 
    {
        SetState(false); 
    }

    void SetState (bool active) 
    {
        isSelected = active; 
        SetButtonImage(active); 

        tabArea.SetActive(active); 
    }



    //  Image  ------------------------------------------------------ 
    void SetButtonImage (bool active) 
    {
        imageNormal.SetActive(!active); 
        imageSelected.SetActive(active); 
    }
    


    //  Tab area  --------------------------------------------------- 
    public GameObject TabArea => tabArea; 

}

}
