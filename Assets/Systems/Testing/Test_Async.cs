using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; 



namespace VirtualLab.Testing 
{

public class Test_Async : MonoBehaviour
{
    
	async void Start () 
	{
		Debug.Log("Starting task"); 
		Task task = AsyncOperation(); 
		Debug.Log("Await"); 
		await task; 
		Debug.Log("Continue"); 
	}



	async Task AsyncOperation () 
	{
		Debug.Log("		| Starting task"); 
		Task task = Task.Delay(2500); 
		Debug.Log("		| Await"); 
		await task; 
		Debug.Log("		| Continue"); 
	}

}

}
