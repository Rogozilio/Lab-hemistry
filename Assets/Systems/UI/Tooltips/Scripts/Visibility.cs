using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using VirtualLab.Animation; 



namespace VirtualLab.Tooltips 
{

public class Visibility 
{
	GameObject tooltipObject; 
    CanvasGroup canvasGroup; 

	LinearAnimation_AB  animation = new LinearAnimation_AB (); 



	public Visibility (GameObject tooltipObject, float fadeTime) 
	{
		this.tooltipObject = tooltipObject; 
		this.canvasGroup = tooltipObject.GetComponent<CanvasGroup>(); 

		InitAnimation(fadeTime); 
		TurnInvisibileOnInit(); 
	}

	void InitAnimation (float fadeTime) 
	{
		animation.value = 0; 
		animation.A = 0; 
		animation.B = 1; 
		// animation.targetIsA = true; 
		animation.animationTime = fadeTime; 
	}

	void TurnInvisibileOnInit () 
	{
		canvasGroup.alpha = 0; 
		tooltipObject.SetActive(false); 
	}



	//  State  ------------------------------------------------------ 
	public bool visible 
	{
		get => animation.value != 0; 
	}



	//  Updating  --------------------------------------------------- 
	public void Update () 
	{
		animation.Update(); 

		UpdateTransparency(); 
		ShowOrHideObject(); 
	}

	void UpdateTransparency () 
	{
		canvasGroup.alpha = animation.value; 
	}

	void ShowOrHideObject () 
	{
		if (animation.value != 0) tooltipObject.SetActive(true); 
		else 					  tooltipObject.SetActive(false); 
	}



	//  Setting visibility  ----------------------------------------- 
	public void SetVisible (bool visible) 
	{
		if (visible) SetShowAnimation(); 
		else 		 SetHideAnimation(); 
	}

	void SetShowAnimation () 
	{
		tooltipObject.SetActive(true); 
		animation.GoToB(); 
	}

	void SetHideAnimation () 
	{
		animation.GoToA(); 
	}

}

}
