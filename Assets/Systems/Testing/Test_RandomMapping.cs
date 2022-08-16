using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.Testing 
{

public class Test_RandomMapping : MonoBehaviour 
{
	[SerializeField][TextArea(0, 20)] string _mapping; 
	[Space]
	[SerializeField] int elementCount = 5; 
	[SerializeField] TestAction recreateMapping; 
    
	RandomMapping mapping; 



	void Start () 
	{
		RecreateMapping(); 
	}

	void Update () 
	{
		DoActions(); 
		DisplayData(); 
	}



	//  Updating  --------------------------------------------------- 
	void DoActions () 
	{
		if (recreateMapping.Read()) RecreateMapping(); 
	}

	void DisplayData () 
	{
		_mapping = mapping.ToString(); 
	}



	//  Actions  ---------------------------------------------------- 
	void RecreateMapping () 
	{
		mapping = new RandomMapping(elementCount); 
	}

}

}
