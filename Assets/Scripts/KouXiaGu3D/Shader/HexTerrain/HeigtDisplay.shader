Shader "HexTerrain/HeigtDisplay" 
{

	/*
		显示地形高度;
	*/

	Properties 
	{
		_HeightTex ("Albedo (ARGB)", 2D) = "black" {}
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

		sampler2D _HeightTex;

		struct Input {
			float2 uv_HeightTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_HeightTex, IN.uv_HeightTex);
			
			o.Albedo = c.a;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
