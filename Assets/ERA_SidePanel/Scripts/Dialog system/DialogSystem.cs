using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset
{

public class DialogSystem : MonoBehaviour
{
    [SerializeField] GameObject errorDialogPrefab;



    //  Actions  ----------------------------------------------------
    public void ShowErrorDialog (string message) 
    {
        var obj = Instantiate(errorDialogPrefab, Vector3.zero, Quaternion.identity, transform);

        var rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector3.zero;

        var dialog = obj.GetComponent<ErrorDialog>();
        dialog.message = message;
    }

}

}