// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


/*
	混合高度贴图,且模糊化贴图, r通道 为提高高度, g通道 为降低高度;
*/

Shader "HexTerrain/BlendHeightMap"
{

	Properties
	{
		_HeightMap ("_HeightMap", 2D) = "black" {}
		_HeightAdjustMap ("_HeightAdjustMap", 2D) = "black" {}
		_SmoothWidth ("_SmoothWidth", float) = 5
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

	v2f vert (appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	sampler2D _HeightMap;
	float4 _HeightMap_TexelSize;
	sampler2D _HeightAdjustMap;
	float _SmoothWidth;

	//获取到这个点的修改后高度;
	float GetHeightWith(float2 uv)
	{
		float heigt = tex2D(_HeightMap, uv).r;
		fixed4 adjustCol = tex2D(_HeightAdjustMap, uv);
		heigt += adjustCol.r - adjustCol.b;
		return heigt;
	}

	fixed4 frag (v2f i) : SV_Target
	{
		float wDelta = _HeightMap_TexelSize.x * _SmoothWidth;
		float hDelta = _HeightMap_TexelSize.y * _SmoothWidth;

		half2 p0 = half2(i.uv.x, i.uv.y);
		half2 p1 = half2(i.uv.x, saturate(i.uv.y + hDelta));
		half2 p2 = half2(i.uv.x, saturate(i.uv.y - hDelta));
		half2 p3 = half2(saturate(i.uv.x - wDelta), i.uv.y);
		half2 p4 = half2(saturate(i.uv.x + wDelta), i.uv.y);

		float height = 0;
		height += GetHeightWith(p0);
		height += GetHeightWith(p1);
		height += GetHeightWith(p2);
		height += GetHeightWith(p3);
		height += GetHeightWith(p4);
		height /= 5;

		return fixed4(height, height, height, 1);
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
