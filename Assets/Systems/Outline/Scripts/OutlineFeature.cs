using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; 



namespace VirtualLab.OutlineNS 
{

public class OutlineFeature : ScriptableRendererFeature 
{
	[SerializeField] RenderPassEvent insertAt = RenderPassEvent.AfterRenderingPostProcessing; 
	[SerializeField] OutlineFeatureSettings settings; 



	//  Life cycle  ------------------------------------------------- 
    public override void Create () 
    {
		if (!settingsReady) return; 

		InitSettingsEvents(); 

		CreateTextures(); 
		CreateMaterials(); 
		CreatePasses(); 
	}

    public override void AddRenderPasses (ScriptableRenderer renderer, ref RenderingData renderingData) 
    {
		if (!settingsReady) return; 

		renderer.EnqueuePass(renderPass); 
		renderer.EnqueuePass(blurPass); 
		renderer.EnqueuePass(outlinePass); 
	}

	protected override void Dispose (bool disposing) 
	{
		ReleaseTextures(); 
		ClearSettingsEvents(); 
	}



	//  Settings  --------------------------------------------------- 
	bool settingsReady 
	{
		get => settings != null && settings.isReady; 
	}

	void InitSettingsEvents () 
	{
		settings.onDataChanged -= Create; 
		settings.onDataChanged += Create; 
	}

	void ClearSettingsEvents () 
	{
		settings.onDataChanged -= Create; 
	}



	//  Textures  --------------------------------------------------- 
	RenderTexture originalShape; 
	RenderTexture blurredShape; 

	bool hasTextures => originalShape != null; 

	void CreateTextures () 
	{
		if (hasTextures) ReleaseTextures(); 

		var descriptor = CreateTextureDescriptor(); 

		originalShape = new RenderTexture(descriptor); 
		originalShape.name = "Original shape"; 

		blurredShape = new RenderTexture(descriptor); 
		blurredShape.name = "Blurred shape"; 
	}

	RenderTextureDescriptor CreateTextureDescriptor () 
	{
		return new RenderTextureDescriptor(Screen.width, Screen.height); 
	}

	void ReleaseTextures () 
	{
		if (originalShape != null) originalShape.Release(); 
		if (originalShape != null) blurredShape.Release(); 

		originalShape = null; 
		blurredShape = null; 
	}



	//  Materials  -------------------------------------------------- 
	Material renderMaterial; 
	Material blurMaterial; 
	Material outlineMaterial; 

	void CreateMaterials () 
	{
		renderMaterial 	= new Material(settings.renderShader); 
		blurMaterial 	= new Material(settings.blurShader); 
		outlineMaterial = new Material(settings.outlineShader); 

		outlineMaterial.SetColor("_color", settings.outlineColor); 
	}



	//  Passes  ----------------------------------------------------- 
	AbstractShapePass renderPass; 
	BlurPass blurPass; 
	OutlinePass outlinePass; 

	void CreatePasses () 
	{
		renderPass = new AbstractShapePass(
			settings.layerMask, 
			renderMaterial, 
			insertAt, 
			originalShape 
		); 

		blurPass = new BlurPass(
			originalShape, 
			blurredShape, 
			blurMaterial, 
			insertAt 
		); 

		outlinePass = new OutlinePass(
			originalShape, 
			blurredShape, 
			outlineMaterial, 
			insertAt 
		); 
	}

}

}
