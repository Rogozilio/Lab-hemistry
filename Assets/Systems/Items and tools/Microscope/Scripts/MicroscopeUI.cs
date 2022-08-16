using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;





namespace VirtualLab {

public class MicroscopeUI : MonoBehaviour
{
    [SerializeField] Microscope microscope; 
    [SerializeField] Image sampleImage; 



    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Hide(); 
        }
    }



    //  Visiblity  -------------------------------------------------- 
    public void Show () 
    {
        sampleImage.sprite = microscope.sample.data.sprite; 
        gameObject.SetActive(true); 
    }

    public void Hide () 
    {
        gameObject.SetActive(false); 
    }



    //  Saving  ----------------------------------------------------- 
    public void SaveImage () 
    {
        #if UNITY_EDITOR 
            Debug.Log("Saving image"); 
        #else 
            string url = microscope.sample.data.imageReportPath; 
            Application.OpenURL(url); 
        #endif 
    }

}

}
