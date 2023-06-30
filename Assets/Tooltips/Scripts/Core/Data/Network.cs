using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



namespace ERA.Tooltips.Core
{

public static class Network 
{

    //  Loading data  -----------------------------------------------
    public static IEnumerator Load (string pathRelative, Result<byte[]> result) 
    {
        string path       = CreatePath(pathRelative);
        var    webRequest = UnityWebRequest.Get(path);
        yield return webRequest.SendWebRequest();
        
        if (IsSuccess(webRequest)) OnLoadingSuccess(webRequest, result);
        else                       OnLoadingError(path, webRequest, result);
    }

    static void OnLoadingSuccess (UnityWebRequest webRequest, Result<byte[]> result) 
    {
        byte [] data = webRequest.downloadHandler.data; 
        if (data == null) data = new byte[0];
        webRequest.Dispose(); 
        result.Success(data);
    }

    static void OnLoadingError (string path, UnityWebRequest webRequest, Result<byte[]> result) 
    {
        string title = "не удалось загрузить данные";
        string details = webRequest.error;
        webRequest.Dispose(); 
        result.Error(title, path, details);
    }



    //  Checking existence  -----------------------------------------
    public static IEnumerator CheckExistence (string pathRelative, Result<bool> result) 
    {
        string path       = CreatePath(pathRelative);
#if UNITY_EDITOR
        var    webRequest = UnityWebRequest.Get(path);
#else
        var    webRequest = UnityWebRequest.Head(path);
#endif
        yield return webRequest.SendWebRequest();

        bool found = IsSuccess(webRequest);
        webRequest.Dispose();
        result.Success(found);
    }



    //  Tech  -------------------------------------------------------
    static string CreatePath (string pathRelative) 
    {
        return Application.streamingAssetsPath + "/" + pathRelative;
    }

    static bool IsSuccess (UnityWebRequest webRequest) 
    {
        return webRequest.result == UnityWebRequest.Result.Success;
    }

}

}
