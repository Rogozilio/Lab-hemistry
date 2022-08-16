using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualLab.Animation; 



namespace VirtualLab.UITooltips 
{

[RequireComponent(typeof(CanvasGroup))] 
public class UITooltip : MonoBehaviour 
{
	[SerializeField] float timeBeforeAppearing = 0.5f; 
	[SerializeField] float fadeTime = 0.25f; 
	[SerializeField] Vector2 offset = new Vector2(); 

	RectTransform rt; 
	CanvasGroup canvasGroup; 

	AppearDelay appearDelay; 
	new Animation animation; 
	AlphaDriver alphaDriver; 



	void Awake () 
	{
		InitConnections(); 
		InitParts(); 
		HideObject(); 
	}

	void Update () 
	{
		if (appearDelay.active) WaitBeforeAppearing(); 
		else 					UpdateAnimation(); 
	}



	//  Init  ------------------------------------------------------- 
	void InitConnections () 
	{
		rt = GetComponent<RectTransform>(); 
		canvasGroup = GetComponent<CanvasGroup>(); 
	}

	void InitParts () 
	{
		appearDelay = new AppearDelay(timeBeforeAppearing); 
		animation = new Animation(fadeTime); 
		alphaDriver = new AlphaDriver(canvasGroup, animation); 
	}



	//  Showing and hiding  ----------------------------------------- 
	public void Show (Vector2 screenPos) 
	{
		gameObject.SetActive(true); 
		SetPosition(screenPos); 

		appearDelay.Start(); 
		animation.Prepare(); 
		alphaDriver.UpdateFromAnimation(); 
	}

	public void Hide () 
	{
		appearDelay.Stop(); 
		animation.StartHiding(); 
	}



	//  Actions  ---------------------------------------------------- 
	void WaitBeforeAppearing () 
	{
		appearDelay.Update(); 
	}

	void UpdateAnimation () 
	{
		animation.Update(); 
		alphaDriver.UpdateFromAnimation(); 

		if (alphaDriver.invisible) HideObject(); 
	}

	void HideObject () 
	{
		gameObject.SetActive(false); 
	}



	//  Position  --------------------------------------------------- 
	public void SetPosition (Vector2 screenPos) 
	{
		rt.anchoredPosition = screenPos + offset; 
	}



	//  Classes  ---------------------------------------------------- 
	class AppearDelay 
	{
		float appearDelay; 
		float timeLeft; 

		public AppearDelay (float appearDelay) 
		{
			this.appearDelay = appearDelay; 
			timeLeft = appearDelay; 
		}

		public bool active => timeLeft > 0; 

		public void Start () 
		{
			timeLeft = appearDelay; 
		}

		public void Update () 
		{
			timeLeft -= Time.deltaTime; 
		}

		public void Stop () 
		{
			timeLeft = 0; 
		}
	}

	class Animation 
	{
		LinearAnimation_01 animation01 = new LinearAnimation_01(); 

		public Animation (float fadeTime) 
		{
			animation01.value = 0; 
			animation01.animationTime = fadeTime; 
		}

		public float alpha => animation01.value; 

		public void Prepare () 
		{
			animation01.value = 0; 
			animation01.GoTo1(); 
		}

		public void StartShowing () 
		{
			animation01.GoTo1(); 
		}

		public void StartHiding () 
		{
			animation01.GoTo0(); 
		}

		public void Update () 
		{
			animation01.Update(); 
		}

	}

	class AlphaDriver 
	{
		CanvasGroup canvasGroup; 
		Animation animation; 

		public AlphaDriver (CanvasGroup canvasGroup, Animation animation) 
		{
			this.canvasGroup = canvasGroup; 
			this.animation = animation; 

			canvasGroup.alpha = 0; 
		}

		public bool invisible => canvasGroup.alpha == 0; 

		public void UpdateFromAnimation () 
		{
			canvasGroup.alpha = animation.alpha; 
		}
	}

}

}
