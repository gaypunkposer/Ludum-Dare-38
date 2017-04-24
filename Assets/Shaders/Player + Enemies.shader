Shader "Custom/Player + Enemies" 
{
	Properties 
	{
		_Color ("Line Color", Color) = (1,1,1,1)
		_ColorInt ("Color Intensity", Float) = 1
		_MainTex ("Main Texture", 2D) = "white" {}
		_Thickness("Thickness", Float) = 1
		_BGColor("Background Color", Color) = (0,0,0,0)
		_Speed("Wave Speed", Float) = 5
		_Scroll("Scroll Intensity", Float) = 0.5
		_WaveOffset ("Wave Offset", Float) = 0
	}

		SubShader
		{
			Pass
			{
				Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

				Blend SrcAlpha OneMinusSrcAlpha
				LOD 200

				CGPROGRAM
					#pragma target 4.0
					#include "UnityCG.cginc"
					#include "UCLA GameLab Wireframe Functions.cginc"
					#pragma vertex vert
					#pragma fragment frag
					#pragma geometry geom

					float _ColorInt;
					float _WaveOffset;
				// Vertex Shader
				UCLAGL_v2g vert(appdata_base v)
				{
					UCLAGL_v2g output = UCLAGL_vert(v);
					output.pos.y += clamp(sin(_Time * _Speed + _WaveOffset) / 4, -0.5, 0.5);
					return output;
				}
				
				// Geometry Shader
				[maxvertexcount(3)]
				void geom(triangle UCLAGL_v2g p[3], inout TriangleStream<UCLAGL_g2f> triStream)
				{
					UCLAGL_geom( p, triStream);
				}
				
				// Fragment Shader
				float4 frag(UCLAGL_g2f input) : COLOR
				{	
					float4 col = UCLAGL_frag(input);
					if (col.a < 0.5f) {
						return tex2D(_MainTex, input.uv) + (_BGColor * _ColorInt);
					}
					else col.a = 1.0f;
					
					return col;
				}
			
			ENDCG
		}
	} 
}
