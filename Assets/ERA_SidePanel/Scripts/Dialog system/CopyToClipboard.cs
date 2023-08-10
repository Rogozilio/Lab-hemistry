using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;



namespace ERA.SidePanelAsset
{

public class CopyToClipboard : MonoBehaviour
{
    [SerializeField] TMP_Text textSource;


    public void Copy () 
    {
        string s = textSource.text;
        s = ClearRichText(s);
        
        GUIUtility.systemCopyBuffer = s;
    }

    string ClearRichText (string s) 
    {
        return Regex.Replace(s, "<.*?>", "");
    }

}

}
