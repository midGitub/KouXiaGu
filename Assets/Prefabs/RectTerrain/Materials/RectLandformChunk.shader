
/*
	游戏地形显示;
*/

Shader "Landform3D/RectLandformChunk" 
{

		Properties 
		{
			_DiffuseMap ("DiffuseMap", 2D) = "white" {}
			_HeightMap ("HeightMap", 2D) = "black" {}
			_NormalMap ("Normalmap", 2D) = "bump" {}
		}

        SubShader {
            Tags { "RenderType"="Opaque" }
            LOD 300

            CGPROGRAM
            #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap 
            #pragma target 5.0
			#include "Tessellation.cginc"

			uniform float _LandformTess;
            uniform float _LandformDisplacement;

			sampler2D _DiffuseMap;
            sampler2D _HeightMap;
			sampler2D _NormalMap;

			struct appdata 
			{
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

			//固定数量细分;
			float4 tessFixed()
            {
                return _LandformTess;
            }

            void disp (inout appdata v)
            {
                float d = tex2Dlod(_HeightMap, float4(v.texcoord.xy, 0, 0)).r * _LandformDisplacement;
                v.vertex.xyz += v.normal * d;
            }

            struct Input 
			{
                half2 uv_DiffuseMap;
				half2 uv_HeightMap;
				half2 uv_NormalMap;
				INTERNAL_DATA
            };

            void surf (Input IN, inout SurfaceOutput o) 
			{
				half4 diffuseColor = tex2D (_DiffuseMap, IN.uv_DiffuseMap);
				o.Albedo = diffuseColor;
            }

            ENDCG
        }

	FallBack "Diffuse"
}
