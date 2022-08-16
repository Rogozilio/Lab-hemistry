using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

public class RandomMapping 
{
	int [] mapping; 


	public RandomMapping (int elementCount) 
	{
		DoChecks(elementCount); 

		CreateDirectMapping(elementCount); 
		MixMapping(); 
	}

	void DoChecks (int elementCount) 
	{
		if (elementCount <= 0) throw new UnityException("Element count for random mapping is too low"); 
	}



	//  Creating mapping  ------------------------------------------- 
	void CreateDirectMapping (int elementCount) 
	{
		mapping = new int[elementCount]; 

		for (int i = 0; i < mapping.Length; i++) 
		{
			mapping[i] = i; 
		}
	}

	void MixMapping () 
	{
		for (int i = 0; i < mapping.Length; i++) 
		{
			int i2 = Random.Range(0, mapping.Length); 
			SwapElements(i, i2); 
		}
	}

	void SwapElements (int i1, int i2) 
	{
		int temp = mapping[i1]; 
		mapping[i1] = mapping[i2]; 
		mapping[i2] = temp; 
	}



	//  Interface  -------------------------------------------------- 
	public int Map (int input) 
	{
		return mapping[input]; 
	}



	//  Tech  ------------------------------------------------------- 
	public override string ToString () 
	{
		string s = ""; 

		for (int i = 0; i < mapping.Length; i++) 
		{
			s += i + " -> " + Map(i) + "\n"; 
		}

		return s; 
	}

}

}
