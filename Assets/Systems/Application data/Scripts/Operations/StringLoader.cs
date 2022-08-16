using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class StringLoader : Operation<string> 
{
    
    public StringLoader (OnCompleted onCompleted, OnError onError = null) 
        : base(onCompleted, onError) {} 



    public void Start (string path) 
    {
        new AssetLoader(OnDataLoaded, OnLoadError).Start(path); 
    }

    void OnDataLoaded (byte [] bytes) 
    {
        string s = System.Text.Encoding.UTF8.GetString(bytes); 
        GoBack(s); 
    }

	void OnLoadError (string message) 
	{
		GoBackWithError("Не получилось загрузить строку"); 
	}

}

}
