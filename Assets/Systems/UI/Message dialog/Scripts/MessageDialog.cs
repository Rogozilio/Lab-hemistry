using System.Collections;
using System.Collections.Generic;
using UnityEngine; 



namespace VirtualLab 
{

public class MessageDialog : MonoBehaviour
{
    [SerializeField] GameObject messagePanelPrefab; 



    //  Interface  -------------------------------------------------- 
    public void ShowErrorMessage (string message) 
    {
        ShowMessagePanel("Ошибка", message); 
    }



    //  Message panel  ---------------------------------------------- 
    void ShowMessagePanel (string title, string message) 
    {
        GameObject panelObj = Instantiate(messagePanelPrefab); 
        panelObj.transform.SetParent(transform, false); 

        MessagePanel panel = panelObj.GetComponent<MessagePanel>(); 
        panel.Init(title, message); 
    }
    
}

}
