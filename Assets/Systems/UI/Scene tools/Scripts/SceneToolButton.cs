using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;





namespace VirtualLab {

public class SceneToolButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler  
{
    // data 
    bool isPressed; 



    void Update()
    {
        if (isPressed) 
        {
            onPressed.Invoke(); 
        }
    }



    //  Events  ----------------------------------------------------- 
    public UnityEvent onPressed; 

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true; 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false; 
    }

}

}
