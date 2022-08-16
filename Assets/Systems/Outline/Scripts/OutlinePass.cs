using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; 



namespace VirtualLab.OutlineNS 
{

public class OutlinePass : ScriptableRenderPass
{
	RenderTexture originalShape; 
	RenderTexture blurredShape; 
	Material material; 



	public OutlinePass (
		RenderTexture originalShape, 
		RenderTexture blurredShape, 
		Material material, 
		RenderPassEvent renderPassEvent 
	) {
		this.originalShape = originalShape; 
		this.blurredShape = blurredShape; 
		this.material = material; 
		this.renderPassEvent = renderPassEvent; 
	}



	//  Info  ------------------------------------------------------- 
	bool readyToRender 
	{
		get {
			return 
				blurredShape != null && 
				originalShape != null && 
				material != null; 
		}
	}



	//  Setup  ------------------------------------------------------ 
	public override void Configure (CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		material.SetTexture("_originalShape", originalShape); 
		material.SetTexture("_blurredShape", blurredShape); 
	}



	//  Rendering  -------------------------------------------------- 
	public override void Execute (ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (readyToRender)
		{
			CommandBuffer cmd = CommandBufferPool.Get(); 

			cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity); 
			cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, material); 

			context.ExecuteCommandBuffer(cmd); 
			CommandBufferPool.Release(cmd); 
		}
	}

}

}
