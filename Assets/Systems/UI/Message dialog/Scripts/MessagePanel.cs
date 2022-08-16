using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace VirtualLab 
{

public class MessagePanel : MonoBehaviour 
{
    [SerializeField] Text titleComponent; 
    [SerializeField] Text messageComponent; 



    public void Init (string title, string message) 
    {
        titleComponent.text = title; 
        messageComponent.text = message; 
    }

    public void Hide () 
    {
        Destroy(gameObject); 
    }

}

}
