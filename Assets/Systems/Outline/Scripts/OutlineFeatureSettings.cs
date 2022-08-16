using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.OutlineNS 
{

[CreateAssetMenu(
	menuName = "Outline feature settings", 
	fileName = "Outline feature settings", 
	order = 0 
)]
public class OutlineFeatureSettings : ScriptableObject 
{
	public LayerMask layerMask; 
	[Space] 
	public Shader renderShader; 
	public Shader blurShader; 
	public Shader outlineShader; 
	[Space] 
	public Color outlineColor = Color.white; 



	void OnValidate () 
	{
		onDataChanged(); 
	}



	//  Events  ----------------------------------------------------- 
	public delegate void EventHandler (); 
	public event EventHandler onDataChanged = delegate {}; 



	//  Info  ------------------------------------------------------- 
	public bool isReady 
	{
		get {
			return 
				renderShader != null && 
				blurShader != null && 
				outlineShader != null; 
		}
	}

	public int outlineLayer 
	{
		get {
			if (layerMask == 0) return 0; 

			int currentLayer = 1; 

			for (int i = 0; i < 32; i++) 
			{
				if ((layerMask & currentLayer) != 0) return i; 
				currentLayer <<= 1; 
			}
			
			throw new UnityException(); 
		}
	}

}

}
