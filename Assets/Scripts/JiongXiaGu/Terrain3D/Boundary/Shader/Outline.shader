﻿
Shader "Yogi/ImageEffect/Outline"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_StencilMap("StencilMap (RGB)", 2D) = "white" {}
		_BlurMap("BlurMap (RGB)", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
		_Intensity("Intensity", Float) = 1
	}

	SubShader
	{
		Pass
		{
			ZTest Always
			ZWrite Off
			Cull Off
			Fog{ Mode Off }

			CGPROGRAM
			#include "UnityCG.cginc"  
			#pragma vertex vert  
			#pragma fragment frag  
			#pragma fragmentoption ARB_precision_hint_fastest  

			sampler2D _MainTex;
			sampler2D _StencilMap;
			sampler2D _BlurMap;
			fixed3 _OutlineColor;
			fixed _Intensity;

			struct a2v
			{
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				fixed4 vertex : SV_POSITION;
				fixed2 uv : TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 original = tex2D(_MainTex, i.uv);
				fixed4 stencil = tex2D(_StencilMap, i.uv);
				fixed4 blur = tex2D(_BlurMap, i.uv);

				fixed4 current = blur - stencil;
				current.rgb = original.rgb + saturate(current.a) * _Intensity * _OutlineColor;
				current.a = original.a;

				return current;
			}
			ENDCG
		}
	}
	Fallback off
}