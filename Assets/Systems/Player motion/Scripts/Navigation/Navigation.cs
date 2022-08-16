using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class Navigation : MonoBehaviour
{
    [SerializeField] AbstractNavPoints navPoints; 



	//  Init  ------------------------------------------------------- 
	public void Init (int location) 
	{
		this.location = location; 
	}



	//  Current location  ------------------------------------------- 
	public int location { get; set; } 

	public int GetNextPointID () 
	{
		if (location < navPoints.pointCount) 
		{
			return location + 1; 
		}
		else 
		{
			return 1; 
		}
	}

	public int GetPreviousPointID () 
	{
		if (location > 1) 
		{
			return location - 1; 
		}
		else 
		{
			return navPoints.pointCount; 
		}
	}

	public ViewPoint MoveToPoint (int pointID) 
	{
		NavPoint navPoint = navPoints.GetPoint(pointID); 

		if (!navPoint.available) throw new UnityException("Trying to move to an unavailable point"); 

		location = pointID; 
		return (ViewPoint) navPoint; 
	}



	//  Point  ------------------------------------------------------ 
	public int pointCount => navPoints.pointCount; 

	public bool IsPointAvailable (int pointID) 
	{
		NavPoint point = navPoints.GetPoint(pointID); 
		return point.available; 
	}

	public void SetPointAvailable (int pointID, bool available) 
	{
		NavPoint point = navPoints.GetPoint(pointID); 
		point.available = available; 
	}

	public ViewPoint GetViewPoint (int poitnID) 
	{
		NavPoint point = navPoints.GetPoint(poitnID); 
		return (ViewPoint) point; 
	}

}

}
