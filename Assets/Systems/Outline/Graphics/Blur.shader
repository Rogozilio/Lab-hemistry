Shader "Custom/Blur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {} 
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert 
			#pragma fragment frag 
			

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};


			sampler2D _MainTex; 
			float4 _MainTex_TexelSize; 
			UNITY_DECLARE_DEPTH_TEXTURE(_MainTex_Depth); 


			v2f vert (appdata v)
			{
				v2f o;
				
				o.vertex = UnityObjectToClipPos(v.vertex); 
				o.uv = v.uv; 

				return o;
			}



			half4 getPixel (float2 uv, float2 offset, float2 pixelSize) 
			{
				return tex2D(_MainTex, uv + offset * pixelSize); 
			}

			half4 frag (v2f input) : SV_Target
			{
				float2 pixelSize = _MainTex_TexelSize.xy; 
				float2 uv = input.uv; 

				half4 color = tex2D(_MainTex, uv); 
				
				// left 
				color += getPixel(uv, float2(-1, 0), pixelSize); 
				color += getPixel(uv, float2(-2, 0), pixelSize); 
				color += getPixel(uv, float2(-3, 0), pixelSize); 

				// right 
				color += getPixel(uv, float2(1, 0), pixelSize); 
				color += getPixel(uv, float2(2, 0), pixelSize); 
				color += getPixel(uv, float2(3, 0), pixelSize); 

				// up 
				color += getPixel(uv, float2(0, 1), pixelSize); 
				color += getPixel(uv, float2(0, 2), pixelSize); 
				color += getPixel(uv, float2(0, 3), pixelSize); 

				// down 
				color += getPixel(uv, float2(0, -1), pixelSize); 
				color += getPixel(uv, float2(0, -2), pixelSize); 
				color += getPixel(uv, float2(0, -3), pixelSize); 

				return color / 12; 
			}

			ENDCG
		}
	}
}