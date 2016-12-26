
/*
	根据高度图生成法线贴图;
*/

Shader "HexTerrain/NormalMapFromHeight"
{

	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Radius ("Radius", float) = 5
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

	sampler2D _MainTex;
	float _Radius;
	float4 _MainTex_TexelSize;

	v2f vert (appdata v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.uv;
		return o;
	}
		
	float Clamp1(float value)
	{
		return clamp(value, 0, 1);
	}


	void NromalMapFromHeightMap(float2 uv, out float dX, out float dY)
	{
		fixed4 col = tex2D(_MainTex, uv);

		float wDelta = _MainTex_TexelSize.x * _Radius;
		float hDelta = _MainTex_TexelSize.y * _Radius;

		float topLeft = tex2D(_MainTex, float2(Clamp1(uv.x - wDelta), Clamp1(uv.y + hDelta))).r;
		float top = tex2D(_MainTex, float2(Clamp1(uv.x), Clamp1(uv.y + hDelta))).r;
		float topRight = tex2D(_MainTex, float2(Clamp1(uv.x + wDelta), Clamp1(uv.y + hDelta))).r;
		float left = tex2D(_MainTex, float2(Clamp1(uv.x - wDelta), Clamp1(uv.y))).r;
		float self = col.r;
		float right = tex2D(_MainTex, float2(Clamp1(uv.x + wDelta), Clamp1(uv.y))).r;
		float bottomLeft = tex2D(_MainTex, float2(Clamp1(uv.x - wDelta), Clamp1(uv.y - hDelta))).r;
		float bottom = tex2D(_MainTex, float2(Clamp1(uv.x), Clamp1(uv.y - hDelta))).r;
		float bottomRight = tex2D(_MainTex, float2(Clamp1(uv.x + wDelta), Clamp1(uv.y - hDelta))).r;

		dX = (topRight + 2 * right + bottomRight) - (topLeft + 2 * left + bottomLeft);
		dY = (bottomLeft + 2 * bottom + bottomRight) - (topLeft + 2 * top + topRight);

		dX = (dX + 1) * 0.5;
		dY = (dY + 1) * 0.5;
	}

	fixed4 frag_Unity (v2f i) : SV_Target
	{
		float dX;
		float dY;
		
		NromalMapFromHeightMap(i.uv, dX, dY);

		fixed4 c = fixed4(dX , dX, dY, dY);
		return c;
	}

	fixed4 frag_NromalMap(v2f i) : SV_Target
	{
		float dX;
		float dY;
		
		NromalMapFromHeightMap(i.uv, dX, dY);

		fixed4 c = fixed4(dX , dY, 1, 1);
		return c;
	}

	ENDCG

	//0
	//Unity使用的法线;
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
			#pragma fragment frag_Unity
			ENDCG
		}
	}

	//1
	//蓝色背景的法线;
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
			#pragma fragment frag_NromalMap
			ENDCG
		}
	}

}
