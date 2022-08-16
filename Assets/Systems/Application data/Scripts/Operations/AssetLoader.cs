using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Networking; 



namespace VirtualLab.ApplicationData 
{

public class AssetLoader : Operation<byte []> 
{

    public AssetLoader (OnCompleted onCompleted, OnError onError) 
        : base(onCompleted, onError) {} 



    public void Start (string path)
    {
        DataLoader.LoadData(path, OnDataLoaded, OnLoadError); 
    }

    void OnDataLoaded (byte [] data) 
    {
        GoBack(data); 
    }

	void OnLoadError () 
	{
		GoBackWithError(); 
	}

}

}
