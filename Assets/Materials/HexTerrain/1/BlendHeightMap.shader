
/*
	混合高度贴图,且模糊化贴图;
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
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
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
		heigt += tex2D(_HeightAdjustMap, uv).r - 0.5;
		return heigt;
	}

	fixed4 frag (v2f i) : SV_Target
	{
		float wDelta = _HeightMap_TexelSize.x * _SmoothWidth;
		float hDelta = _HeightMap_TexelSize.y * _SmoothWidth;

		float2 p0 = float2(i.uv.x, i.uv.y);
		float2 p1 = float2(i.uv.x, saturate(i.uv.y + hDelta));
		float2 p2 = float2(i.uv.x, saturate(i.uv.y - hDelta));
		float2 p3 = float2(saturate(i.uv.x - wDelta), i.uv.y);
		float2 p4 = float2(saturate(i.uv.x + wDelta), i.uv.y);

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
