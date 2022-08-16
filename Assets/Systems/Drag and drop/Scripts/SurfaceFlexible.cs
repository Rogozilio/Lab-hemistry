using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.DragAndDropNS 
{

public class SurfaceFlexible : Surface 
{
	[SerializeField][Range(0.01f, 10)] float objectGravity = 1; 

	new Camera camera; 

	List<Vector3> points = new List<Vector3>(); 



	void Awake () 
	{
		camera = Camera.main; 
	}



	//  Setup  ------------------------------------------------------ 
	public override void Setup (List<Vector3> pointsWorld) 
	{
		Clear(); 
		AddPoints(pointsWorld); 
	}

	void AddPoints (List<Vector3> pointsWorld) 
	{
		foreach (Vector3 pointWorld in pointsWorld) 
		{
			AddPoint(pointWorld); 
		}
	}

	void AddPoint (Vector3 pointWorld) 
	{
		Vector3 pointScreen = camera.WorldToScreenPoint(pointWorld); 

		if (CullFilter(pointScreen)) 
		{
			points.Add(pointScreen); 
		}
	}

	bool CullFilter (Vector3 pointScreen) 
	{
		Rect screen = new Rect(
			0, 
			0, 
			camera.pixelWidth, 
			camera.pixelHeight
		); 

		return 
			screen.Contains(pointScreen) && 
			pointScreen.z > 0; 
	}

	void Clear () 
	{
		points.Clear(); 
	}



	//  Getting point  ---------------------------------------------- 
	public override Vector3 GetPointWorld (Vector3 pointScreen)
	{
		pointScreen.z = GetWeightedAverage(pointScreen, objectGravity); 
		return Camera.main.ScreenToWorldPoint(pointScreen); 
	}



	//  Sorting  ---------------------------------------------------- 
	void SortByDistane (Vector3 position) 
	{
		points.Sort(
			(x, y) => 
			{
				float toX = Distance(position, x); 
				float toY = Distance(position, y); 

				if (toX < toY) 		return -1; 
				else if (toX > toY) return 1; 
				else 				return 0; 
			}
		); 
	}



	//  Average  ---------------------------------------------------- 
	float average = 1; 

	float GetWeightedAverage (Vector3 point, float objectGravity) 
	{
		if (points.Count == 0) return average; 

		SortByDistane(point); 
		
		float [] distances = CreateDistances(point); 
		float [] weights = CreateWeights(distances, objectGravity); 

		average = WeightedAverage(weights); 
		return average; 
	}

	float [] CreateDistances (Vector3 point) 
	{
		float [] distances = new float[points.Count]; 

		for (int i = 0; i < distances.Length; i++) 
		{
			distances[i] = Distance(point, points[i]); 
		}

		return distances; 
	}

	float [] CreateWeights (float [] distances, float objectGravity) 
	{
		float [] weights = new float[distances.Length]; 
		float maxDistance = distances[distances.Length - 1]; 

		for (int i = 0; i < weights.Length; i++) 
		{
			weights[i] = CreateWeight(distances[i], maxDistance, objectGravity); 
		}

		return weights; 
	}

	float CreateWeight (float distance, float maxDistance, float objectGravity) 
	{
		if (maxDistance == 0) return 1; 
		if (distance == 0) return 90000; 

		return 1 / Mathf.Pow(distance, objectGravity); 
	}

	float WeightedAverage (float [] weights) 
	{
		float value = 0; 
		float weight = 0; 

		for (int i = 0; i < weights.Length; i++) 
		{
			value += points[i].z * weights[i]; 
			weight += weights[i]; 
		}

		return value / weight; 
	}



	//  Tech  ------------------------------------------------------- 
	float Distance (Vector3 one, Vector3 two) 
	{
		return Vector2.Distance(one, two); 
	}
	
}

}
