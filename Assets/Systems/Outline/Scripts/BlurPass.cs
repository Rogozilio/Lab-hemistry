using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Rendering; 
using UnityEngine.Rendering.Universal; 
using UnityEngine.Experimental.Rendering; 



namespace VirtualLab.OutlineNS 
{

public class BlurPass : ScriptableRenderPass 
{
	RenderTexture source; 
	RenderTexture destination; 
	Material material; 



	public BlurPass (
		RenderTexture source, 
		RenderTexture destination, 
		Material material, 
		RenderPassEvent renderPassEvent 
	) {
		this.source = source; 
		this.destination = destination; 
		this.material = material; 
		this.renderPassEvent = renderPassEvent; 
	}



	//  Info  ------------------------------------------------------- 
	bool isReadyToRender 
	{
		get {
			return 
				source && 
				destination && 
				material; 
		}
	}



	//  Setup  ------------------------------------------------------ 
	public override void Configure (CommandBuffer cmd, RenderTextureDescriptor cameraDesc)
	{
		material.SetTexture("_MainTex", source); 
		ConfigureTarget(destination); 
	}



	//  Rendering  -------------------------------------------------- 
    public override void Execute (ScriptableRenderContext context, ref RenderingData renderingData)
    {
		CommandBuffer cmd = CommandBufferPool.Get("BlurPass"); 

		cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity); 
		cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, material); 

		context.ExecuteCommandBuffer(cmd); 
		CommandBufferPool.Release(cmd); 
	}

}

}
