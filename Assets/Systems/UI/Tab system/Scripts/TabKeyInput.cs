using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab 
{

public class TabKeyInput : MonoBehaviour
{
	[SerializeField] TabSystem tabSystem; 
	[SerializeField] InfoPanelAnimation infoPanel; 



    void Update () 
	{
		switch (infoPanel.stateLastFrame) 
		{
			case InfoPanelAnimation.State.Visible: 
			case InfoPanelAnimation.State.Appearing: 
				SwitchToNextTab_OnTab(); 
				SwitchToPreviousTab_OnShiftTab(); 
				break; 
		}
	}

	void SwitchToNextTab_OnTab () 
	{
		if (
			Input.GetKeyDown(KeyCode.Tab) 
			&& 
			!( Input.GetKey(KeyCode.LeftShift) || 
			   Input.GetKey(KeyCode.RightShift) )
		) {
			tabSystem.SwitchToNextTab(); 
		}
	}

	void SwitchToPreviousTab_OnShiftTab () 
	{
		if (
			Input.GetKeyDown(KeyCode.Tab) 
			&& 
			( Input.GetKey(KeyCode.LeftShift) || 
			  Input.GetKey(KeyCode.RightShift) )
		) {
			tabSystem.SwitchToPreviousTab(); 
		}
	}

}

}
