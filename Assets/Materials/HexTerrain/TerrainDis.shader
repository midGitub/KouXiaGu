
/*
	根据 高度灰度图 生成 法线图, 并且把 高度放置在 阿尔法通道上;
*/

Shader "HexTerrain/TerrainDis" 
{

	Properties
	{
		_MainTex ("HeightMap", 2D) = "gray" {}
		_MainTexWidth("HeightMap Width", float) = 1024
		_MainTexHeight("HeightMap Height", float) = 1024
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _MainTexWidth, _MainTexHeight;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float wDelta = 1 / _MainTexWidth;
			float hDelta = 1 / _MainTexHeight;
			float sobel = 1;

			float topLeft = tex2D(_MainTex, float2(IN.uv_MainTex.x - wDelta, IN.uv_MainTex.y + hDelta)).r;
			float top = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y + hDelta)).r;
            float topRight = tex2D(_MainTex, float2(IN.uv_MainTex.x + wDelta, IN.uv_MainTex.y + hDelta)).r;
            float left = tex2D(_MainTex, float2(IN.uv_MainTex.x - wDelta, IN.uv_MainTex.y)).r;
            float self = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y)).r;
            float right = tex2D(_MainTex, float2(IN.uv_MainTex.x + wDelta, IN.uv_MainTex.y)).r;
            float bottomLeft = tex2D(_MainTex, float2(IN.uv_MainTex.x - wDelta, IN.uv_MainTex.y - hDelta)).r;
            float bottom = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y - hDelta)).r;
            float bottomRight = tex2D(_MainTex, float2(IN.uv_MainTex.x + wDelta, IN.uv_MainTex.y - hDelta)).r;

			float dX = (topRight + sobel * right + bottomRight) - (topLeft + sobel * left + bottomLeft);
            float dY = (bottomLeft + sobel * bottom + bottomRight) - (topLeft + sobel * top + topRight);
            float dZ = 1;
			float a = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y)).r;

			dX = (dX + 1) * 0.5;
			dY = (dY + 1) * 0.5;
			
			fixed4 c = fixed4(dX , dY, dZ, a);

			o.Albedo = c;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
