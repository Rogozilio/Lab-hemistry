using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Networking; 



namespace VirtualLab.ApplicationData
{

public static class DataLoader 
{
    public delegate void OnCompleted (byte [] data); 
	public delegate void OnError (); 


    public static void LoadData (string relativePath, OnCompleted onCompleted, OnError onError) 
    {
        UnityWebRequest webRequest = CreateWebRequest(relativePath); 

        AsyncOperation operation = webRequest.SendWebRequest(); 
        operation.completed += 
            (operation) => OnDataLoaded(webRequest, onCompleted, onError); 
    }

    static UnityWebRequest CreateWebRequest (string relativePath) 
    {
        string path = Application.streamingAssetsPath + "/" + relativePath; 
        UnityWebRequest webRequest = UnityWebRequest.Get(path); 
        return webRequest; 
    }

    static void OnDataLoaded (UnityWebRequest webRequest, OnCompleted onCompleted, OnError onError) 
    {
        if (webRequest.result == UnityWebRequest.Result.Success) 
        {
			byte [] data = webRequest.downloadHandler.data; 
			webRequest.Dispose(); 

            onCompleted(data); 
        }
        else 
        {
			webRequest.Dispose(); 
            onError(); 
        }
    }

}

}