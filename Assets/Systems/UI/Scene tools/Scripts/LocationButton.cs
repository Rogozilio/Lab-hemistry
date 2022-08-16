using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

public class LocationButton : MonoBehaviour 
{
	public enum State { Available, Active, NotAvailable } 

	[SerializeField] SwitchMany imageSwitch; 
	[SerializeField] State _state; 



	void OnValidate () 
	{
		UpdateUI(); 
	}



	public State state 
	{
		get => _state; 
		set {
			_state = value; 
			UpdateUI(); 
		}
	}

	void UpdateUI () 
	{
		if (Application.isPlaying || imageSwitch != null) 
		{
			imageSwitch.selectedObject = (int) state; 
		}
	}

}

}
