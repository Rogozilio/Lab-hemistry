using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events; 
using UnityEngine.EventSystems; 



namespace VirtualLab 
{

public class MouseTrigger : MonoBehaviour
{
    public UnityEvent onMouseDown; 
    public UnityEvent onMouseUp; 
    public UnityEvent onClick; 



    void OnMouseDown () 
    {
        onMouseDown.Invoke(); 
    }

    void OnMouseUp () 
    {
        onMouseUp.Invoke(); 
    }

    void OnMouseUpAsButton () 
    {
        onClick.Invoke(); 
    }

}

}
