
/*
	游戏地形显示;
*/

Shader "Landform3D/LandformChunk" 
{

       Properties {
            _DiffuseMap ("DiffuseMap", 2D) = "white" {}
            _HeightMap ("HeightMap", 2D) = "black" {}
			_RoadDiffuseMap ("RoadDiffuseMap", 2D) = "black" {}
			_RoadHeightMap ("RoadHeightMap", 2D) = "black" {}
			_NormalMap ("Normalmap", 2D) = "bump" {}

			_SnowColor ("Snow Color", Color) = (1.0,1.0,1.0,1.0)
			_SnowDirection ("Snow Direction", Vector) = (0, 1, 0)
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
			sampler2D _RoadDiffuseMap;
			sampler2D _RoadHeightMap;
			sampler2D _NormalMap;

			uniform float _TerrainSnow;
			float4 _SnowColor;
			float4 _SnowDirection;

			uniform sampler2D _LandformGridLineMap;
			uniform half3 _LandformGridLineColor;

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
				half2 uv_RoadDiffuseMap;
				half2 uv_RoadHeightMap;
				half2 uv_NormalMap;
				half2 uv_LandformGridLineMap;
				half2 uv_SnowTex;
				float3 worldNormal;
				INTERNAL_DATA
            };

            void surf (Input IN, inout SurfaceOutput o) 
			{
				//half4 c = tex2D (_DiffuseMap, IN.uv_DiffuseMap) * _Color;
				//o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));

				//if (dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz) > lerp(1, -1, _TerrainSnow/10000)) 
				//{
				//	o.Albedo = _SnowColor.rgb;
				//} 
				//else 
				//{
				//	o.Albedo = c.rgb;
				//}
				
				half4 diffuseColor = tex2D (_DiffuseMap, IN.uv_DiffuseMap);
				half4 roadDiffuseColor = tex2D (_RoadDiffuseMap, IN.uv_RoadDiffuseMap);
				half3 result = lerp(diffuseColor, roadDiffuseColor, roadDiffuseColor.a).rgb;

				half3 gridLineColor = tex2D (_LandformGridLineMap, IN.uv_LandformGridLineMap);

				o.Albedo = lerp(result, _LandformGridLineColor, gridLineColor.r).rgb;
            }

            ENDCG
        }

	FallBack "Diffuse"
}
