using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Rendering; 
using UnityEngine.Rendering.Universal; 



namespace VirtualLab.OutlineNS 
{

public class Outline : MonoBehaviour
{
	[SerializeField] bool autoOutlineLayer = true; 
	[SerializeField] OutlineFeatureSettings outlineSettings; 
	[SerializeField] int _outlineLayer; 

	int originalLayer; 



	//  Info  ------------------------------------------------------- 
	bool outlineActive => gameObject.layer == outlineLayer; 

	int outlineLayer 
	{
		get {
			if (autoOutlineLayer) return outlineSettings.outlineLayer; 
			else 				  return _outlineLayer; 
		}
	}



	//  Outline  ---------------------------------------------------- 
	public void ShowOutline () 
	{
		if (outlineActive) return; 

		originalLayer = gameObject.layer; 
		gameObject.SetLayerRecursively(outlineLayer); 
	}

	public void HideOutline () 
	{
		if (!outlineActive) return; 

		gameObject.SetLayerRecursively(originalLayer); 
	}

}

}
