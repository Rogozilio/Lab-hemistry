using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA.SidePanelAsset 
{

public class TabKeyInput : MonoBehaviour
{
	[SerializeField] TabSystem tabSystem; 
	[SerializeField] InfoPanelAnimation infoPanel; 



    void Update () 
	{
        if (infoPanel.isAppearingOrVisible) 
        {
            SwitchToNextTab_OnCtrlRight(); 
            SwitchToPreviousTab_OnCtrlLeft(); 
        }
	}

	void SwitchToNextTab_OnCtrlRight () 
	{
		if (
			Input.GetKeyDown(KeyCode.RightArrow)
			&& 
			(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		) {
			tabSystem.SwitchToNextTab(); 
		}
	}

	void SwitchToPreviousTab_OnCtrlLeft () 
	{
		if (
			Input.GetKeyDown(KeyCode.LeftArrow)
			&& 
			(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		) {
			tabSystem.SwitchToPreviousTab(); 
		}
	}

}

}
