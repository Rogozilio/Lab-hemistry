using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.PlayerMotion 
{

public class CameraZoom : MonoBehaviour
{
    [SerializeField] float minFOV = 10; 
    [SerializeField] float maxFOV = 60; 
	[SerializeField] float zoomSpeed = 50; 
	[SerializeField] float resetTime = 1; 

	new Camera camera; 



	void Awake () 
	{
		camera = Camera.main; 
	}

	void Update () 
	{
		if (Input.GetKey(KeyCode.R)) ZoomIn(); 
		if (Input.GetKey(KeyCode.F)) ZoomOut(); 
	}



	//  Info  ------------------------------------------------------- 
	public bool CanZoomIn => camera.fieldOfView > minFOV; 
	public bool CanZoomOut => camera.fieldOfView < maxFOV; 



	//  Zoom  ------------------------------------------------------- 
	public void ZoomIn () 
	{
		if (resetting) return; 

		ChangeFOV(- zoomSpeed); 
	}

	public void ZoomOut () 
	{
		if (resetting) return; 

		ChangeFOV(zoomSpeed); 
	}

	void ChangeFOV (float speed) 
	{
		float fov = camera.fieldOfView; 

		float change = speed * Time.deltaTime; 
		fov += change; 
		fov = Mathf.Clamp(fov, minFOV, maxFOV); 

		camera.fieldOfView = fov; 
	}



	//  Reset  ------------------------------------------------------ 
	bool resetting; 

	public void Reset () 
	{
		StartCoroutine(ResetProcess()); 
	}

	IEnumerator ResetProcess () 
	{
		resetting = true; 
		
		float resetSpeed = (maxFOV - camera.fieldOfView) / resetTime; 

		while (camera.fieldOfView < maxFOV) 
		{
			ChangeFOV(resetSpeed); 
			yield return null; 
		}

		resetting = false; 
	}

	public void ZoomInPoint(float minFOV = 10)
	{
		StartCoroutine(ZoomInProcess(minFOV)); 
	}
	
	IEnumerator ZoomInProcess(float minFOV) 
	{
		resetting = true; 
		
		float resetSpeed = (camera.fieldOfView - minFOV) / resetTime; 

		while (camera.fieldOfView > minFOV) 
		{
			ChangeFOV(-resetSpeed); 
			yield return null; 
		}
		
		resetting = false; 
	}

}

}
