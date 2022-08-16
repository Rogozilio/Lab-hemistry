using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public abstract class Operation <Output> 
{
    public delegate void OnCompleted (Output data); 
	public delegate void OnError (string message = null); 

    OnCompleted onCompleted; 
	OnError onError; 



    public Operation (OnCompleted onCompleted, OnError onError) 
    {
        this.onCompleted = onCompleted; 
		this.onError = onError; 
    }



	protected void GoBack (Output data) 
	{
		if (onCompleted != null) onCompleted(data); 
	}

	protected void GoBackWithError (string message = null) 
	{
		if (onError != null) onError(message); 
	}

}

}