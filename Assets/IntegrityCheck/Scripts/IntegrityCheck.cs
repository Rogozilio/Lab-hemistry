using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace VirtualLab.IntegrityCheck
{

public class IntegrityCheck : MonoBehaviour
{
    [SerializeField] ScreenSwitch screens;
    [SerializeField] string nextSceneName = "Main";
    [Space]
    [SerializeField] bool doCheckInEditor = false;
    [SerializeField] string editorURL = "tpu.ru";
    [SerializeField] bool startNextSceneInEditor = true;

    const string KEY = "H022Zt3Di@VI0y!1U1U4yPCdgQKMQ0007%ed";



    void Start () 
    {
#if UNITY_EDITOR
        if (doCheckInEditor) StartCoroutine(CheckIntegrity());
        else                 OnSuccess();
#else 
        StartCoroutine(CheckIntegrity());
#endif
    }



    //  Checking  ---------------------------------------------------
    IEnumerator CheckIntegrity () 
    {
        // load control url 
        var result = new Result<string>();
        var loading = StringLoader.Load(this, "url.txt", result);
        yield return StartCoroutine(loading);

        if (!result.success) 
        {
            OnError();
            yield break;
        }

        // decode control url 
        string controlURL = result.data;
        controlURL = Encryption.Decode(controlURL, KEY);

        // get current url 
        string currentURL = GetCurrentURL();

        // compare urls 
        if (CompareURL(currentURL, controlURL)) OnSuccess();
        else                                    OnError();
    }

    void OnSuccess () 
    {
#if UNITY_EDITOR
        if (startNextSceneInEditor) SceneManager.LoadScene(nextSceneName);
#else 
        SceneManager.LoadScene(nextSceneName);
#endif
    }

    void OnError () 
    {
        screens.SetScreen("Error");
    }



    //  URL  --------------------------------------------------------
    string GetCurrentURL () 
    {
#if UNITY_EDITOR
        return editorURL;
#else 
        return Application.absoluteURL;
#endif 
    }

    bool CompareURL (string a, string b) 
    {
        string mainA = GetMainPart(a);
        string mainB = GetMainPart(b);
        return mainA == mainB;
    }

    string GetMainPart (string url) 
    {
        return new System.Uri(url).Host;
    }

}

}
