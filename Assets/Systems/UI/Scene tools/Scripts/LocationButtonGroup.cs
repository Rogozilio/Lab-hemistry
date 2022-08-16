using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using VirtualLab.PlayerMotion; 



namespace VirtualLab 
{

public class LocationButtonGroup : MonoBehaviour
{
	[SerializeField] PlayerMotion.PlayerMotion playerMotion; 
    [SerializeField] List<LocationButton> buttons; 



	void Update () 
	{
		UpdateButtons(); 
	}



	void UpdateButtons () 
	{
		int currentPoint = playerMotion.semanticLocation; 
		
		for (int i = 0; i < buttons.Count; i++) 
		{
			int buttonPoint = i + 1; 
			UpdateButton(buttons[i], buttonPoint, currentPoint); 
		}
	}

	void UpdateButton (LocationButton button, int buttonPoint, int currentPoint) 
	{
		if (currentPoint == buttonPoint) 
		{
			button.state = LocationButton.State.Active; 
		}
		else 
		{
			button.state = LocationButton.State.Available; 
		}
	}

}

}
