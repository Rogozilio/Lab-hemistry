using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class OperatoinsTracker 
{
	bool allStarted; 
	int activeOperations; 



	//  Init  ------------------------------------------------------- 
	public void Reset () 
	{
		allStarted = false; 
		activeOperations = 0; 
	}



	//  Events  ----------------------------------------------------- 
	public void OneStarted () 
	{
		activeOperations++; 
	}

	public void AllStarted () 
	{
		allStarted = true; 
	}

	public void OneFinished () 
	{
		activeOperations--; 
	}



	//  Info  ------------------------------------------------------- 
	public bool allFinished => allStarted && activeOperations == 0; 


}

}
