// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "My shaders/Outline"
{
    Properties
    {
        _originalShape ("Original shape", 2D) = "white" {} 
        _blurredShape ("Blurred shape", 2D) = "white" {} 
        _shapeDepth ("Shape depth", 2D) = "white" {} 
		_color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" } 
        LOD 100 

        Pass 
        {
			Blend SrcAlpha OneMinusSrcAlpha  
			ZTest Always 

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            struct appdata
            {
                float4 vertex : POSITION; 
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            }; 

			sampler2D _blurredShape; 
			sampler2D _originalShape; 
			fixed4 _color; 


            v2f vert (appdata v)
            {
                v2f o; 
				o.vertex = UnityObjectToClipPos(v.vertex); 
                o.uv = v.uv; 

                return o; 
            }

            fixed4 frag (v2f i) : SV_Target 
            {
                fixed4 blurred = tex2D(_blurredShape, i.uv); 
                fixed4 original = tex2D(_originalShape, i.uv); 

				fixed4 output = blurred - original; 
				output = max(output, 0); 
				
				output.rgb = _color.rgb; 
				output.a = pow(output.a, 0.5); 

                return output; 
            }

            ENDCG
        }
    }
}
