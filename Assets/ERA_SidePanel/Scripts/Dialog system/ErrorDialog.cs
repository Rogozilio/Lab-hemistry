using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



namespace ERA.SidePanelAsset
{

public class ErrorDialog : MonoBehaviour
{
    [SerializeField] TMP_Text messageArea;


    public string message 
    {
        get => messageArea.text;
        set => messageArea.text = value;
    }

    public void Hide () 
    {
        Destroy(gameObject);
    }

}

}