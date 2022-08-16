using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering; 
using UnityEngine.Rendering.Universal; 
using UnityEngine.Experimental.Rendering;



namespace VirtualLab.OutlineNS 
{

public class AbstractShapePass : ScriptableRenderPass 
{
	List<ShaderTagId> shaderTagIdList = new List<ShaderTagId>() { new ShaderTagId("UniversalForward") }; 
	FilteringSettings filteringSettings; 
	RenderStateBlock renderStateBlock; 

	RenderTexture output; 
	Material material;



	public AbstractShapePass (
		int layerMask, 
		Material material, 
		RenderPassEvent renderPassEvent, 
		RenderTexture output
	) {
		this.material = material; 
		this.renderPassEvent = renderPassEvent; 
		this.output = output; 

		filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask); 
		renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing); 
	}



	//  Info  ------------------------------------------------------- 
	bool isReadyToRender 
	{
		get {
			return 
				material != null && 
				output != null; 
		}
	}



	//  Setup  ------------------------------------------------------ 
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		ConfigureTarget(output); 
		ConfigureClear(ClearFlag.All, Color.clear); 
	}



	//  Rendering  -------------------------------------------------- 
	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (isReadyToRender) 
		{
			SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags; 

			DrawingSettings drawingSettings = CreateDrawingSettings(
				shaderTagIdList, 
				ref renderingData, 
				sortingCriteria
			); 
			drawingSettings.overrideMaterial = material; 

			context.DrawRenderers(
				renderingData.cullResults, 
				ref drawingSettings, 
				ref filteringSettings, 
				ref renderStateBlock
			);
		}

	}

}

}
