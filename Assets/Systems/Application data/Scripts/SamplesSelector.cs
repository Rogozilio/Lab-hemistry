using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.ApplicationData 
{

public class SamplesSelector 
{
	List<int> [] selectedIDs = new List<int>[3]; 



	public SamplesSelector (SamplesInfo info) 
	{
		CreateIDLists(); 

		SelectIDsForGroup(0, info.group1); 
		SelectIDsForGroup(1, info.group2); 
		SelectIDsForGroup(2, info.group3); 
	}

	void CreateIDLists () 
	{
		for (int i = 0; i < selectedIDs.Length; i++) 
		{
			selectedIDs[i] = new List<int>(); 
		}
	}



	//  Data access  ------------------------------------------------ 
	public int groupCount => selectedIDs.Length; 

	public int [] GetSelectedIDs (int groupIndex) 
	{
		return selectedIDs[groupIndex].ToArray(); 
	}



	//  Selecting IDs  ---------------------------------------------- 
	void SelectIDsForGroup (int groupIndex, int [] group) 
	{
		if (groupIndex == 0 || groupIndex == 2) 
		{
			SelectOneID(groupIndex, group); 
		}
		else if (groupIndex == 1) 
		{
			SelectMultipleIDs(groupIndex, group, 3); 
		}
		else 
			throw new UnityException("Not supported yet"); 
	}

	void SelectOneID (int groupIndex, int [] group) 
	{
		int randomIndex = Random.Range(0, group.Length); 
		int id = group[randomIndex]; 
		selectedIDs[groupIndex].Add(id); 
	}

	void SelectMultipleIDs (int groupIndex, int [] group, int howManyToSelect) 
	{
		List<int> randomIndexes = CreateUniqueRandomNumbers(howManyToSelect, group.Length - 1); 

		for (int i = 0; i < howManyToSelect; i++) 
		{
			int id = group[randomIndexes[i]]; 
			selectedIDs[groupIndex].Add(id); 
		}
	}



	//  Unique random numbers 
	List<int> CreateUniqueRandomNumbers (int howManyToCreate, int maxNumber) 
	{
		CheckIfCanCreateThisManyNumbers(howManyToCreate, maxNumber); 
		return CreateUniqueRandomNumbers_NoChecks(howManyToCreate, maxNumber); 
	}

	void CheckIfCanCreateThisManyNumbers (int howManyToCreate, int maxNumber) 
	{
		int howManyCanBeCreated = maxNumber + 1; 

		if (howManyToCreate > howManyCanBeCreated) 
			throw new UnityException("Can't generate that many unique random numbers"); 
	}

	List<int> CreateUniqueRandomNumbers_NoChecks (int howManyToCreate, int maxNumber) 
	{
		List<int> numbers = new List<int>(); 

		while (numbers.Count < howManyToCreate) 
		{
			int n = Random.Range(0, maxNumber + 1); 
			if (!numbers.Contains(n)) numbers.Add(n); 
		}

		return numbers; 
	}

}

}
