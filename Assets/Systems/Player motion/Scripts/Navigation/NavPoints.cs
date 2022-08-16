using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class NavPoints : AbstractNavPoints 
{
	[SerializeField] NavPoint table; 

	public const int Table = 1; 



	public override int pointCount => 1; 

	public override NavPoint GetPoint (int pointID) 
	{
		switch (pointID) 
		{
			case Table:    return table; 
			default: 	   throw new UnityException("Point ID is out of range"); 
		}
	}

}

}
