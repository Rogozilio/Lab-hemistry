using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events; 



namespace VirtualLab.ApplicationData 
{

public class AppInit : MonoBehaviour
{
    [SerializeField] MessageDialog messageDialog; 

    public UnityEvent<AppData> onAppDataLoaded; 


    
    void Start () 
    {
        new AppDataLoader(OnDataLoaded, OnLoadError).Start(); 
    }

    void OnDataLoaded (AppData appData) 
    {
        onAppDataLoaded.Invoke(appData); 
    }

	void OnLoadError (string message) 
	{
		messageDialog.ShowErrorMessage(message); 
	}

}

}
