using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

public class SwitchMany : MonoBehaviour 
{
	[SerializeField] List<GameObject> objects; 
	[SerializeField] int _selectedObject; 



	void OnValidate () 
	{
		selectedObject = _selectedObject; 
	}



	//  State  ------------------------------------------------------ 
	public int selectedObject 
	{
		get => _selectedObject; 
		set {
			_selectedObject = Mathf.Clamp(value, 0, objects.Count - 1); 
			UpdateUI(); 
		}
	}



	//  UI  --------------------------------------------------------- 
	void UpdateUI () 
	{
		for (int i = 0; i < objects.Count; i++) 
		{
			objects[i].SetActive(i == selectedObject); 
		}
	}

}

}

