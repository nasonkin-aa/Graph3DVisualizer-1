﻿Shader "Custom/BillboardShader"
{
	Properties{
		_MainTex("Texture Image", 2D) = "white" {}
		_ScaleX("Scale X", Float) = 1.0
		_ScaleY("Scale Y", Float) = 1.0
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.0
		[Toggle] _IsMonoColor("Is Mono Color", Float) = 0
		_MonoColor("Mono Color", Color) = (255,0,0,0)
		_LineSize("Line Size", int) = 1
	}
		SubShader{
			Tags
			{
				"Queue" = "AlphaTest"
				"RenderType" = "TransparentCutout"
				"IgnoreProjector" = "True"
			//"DisableBatching" = "True" //In order to avoid "flickering"
		}

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float _ScaleX;
			uniform float _ScaleY;
			uniform float _Cutoff;
			uniform int _LineSize;
			uniform fixed4 _MonoColor;
			uniform fixed _IsMonoColor;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 tex : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;
				output.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0)) + float4(input.vertex.x, input.vertex.y, 0.0, 0.0) * float4(_ScaleX, _ScaleY, 1.0, 1.0));
				output.tex = input.tex;
				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
				float4 texColor = tex2D(_MainTex, float2(input.tex.xy));
				if (texColor.a < _Cutoff)
				{
					discard;
				}

				if (_IsMonoColor != 0)
				{
					texColor = _MonoColor;
				}

				return texColor;
			}

			ENDCG
		}
		}
}