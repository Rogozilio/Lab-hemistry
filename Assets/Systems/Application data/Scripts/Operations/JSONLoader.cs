using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class JSONLoader<Output> : Operation<Output> 
{
	string path; 



    public JSONLoader (OnCompleted onCompleted, OnError onError = null) 
        : base(onCompleted, onError) {} 



    public void Start (string path) 
    {
		this.path = path; 
        new StringLoader(OnDataLoaded, OnLoadError).Start(path); 
    }

    void OnDataLoaded (string data) 
    {
        Output obj = JsonUtility.FromJson<Output>(data); 
        GoBack(obj); 
    }

	void OnLoadError (string message) 
	{
		GoBackWithError("Не получилось загрузить файл: " + path); 
	}

}

}
