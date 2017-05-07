// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


/*
	混合材质贴图;
*/

Shader "HexTerrain/BlendDiffuseMap"
{

	Properties
	{
		_BaseMap ("_BaseMap", 2D) = "black" {}
		_SecondMap ("_SecondMap", 2D) = "black" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

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

	sampler2D _BaseMap;
	sampler2D _SecondMap;

	v2f vert (appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	fixed4 frag (v2f i) : SV_Target
	{
		fixed4 col = tex2D(_BaseMap, i.uv);
		fixed4 secondColor = tex2D(_SecondMap, i.uv);

		col = lerp(col, secondColor, secondColor.a);

		return col;
	}

	ENDCG

	//0
	SubShader
	{
		Pass
		{
            ZTest Always  
            Cull Off  
            ZWrite Off  
            Fog{ Mode Off }  

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}

}
