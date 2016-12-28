
/*
	游戏地形显示;
*/

Shader "HexTerrain/TerrainSnow" 
{
       Properties {
            _MainTex ("Base (RGB)", 2D) = "white" {}
            _HeightTex ("HeightMap", 2D) = "black" {}
			_NormalMap ("Normalmap", 2D) = "bump" {}

			_SnowColor ("Snow Color", Color) = (1.0,1.0,1.0,1.0)
			_SnowDirection ("Snow Direction", Vector) = (0,1,0)

            _Color ("Color", color) = (1,1,1,0)
        }
        SubShader {
            Tags { "RenderType"="Opaque" }
            LOD 300

            CGPROGRAM
            #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:vert tessellate:tessFixed nolightmap 
            #pragma target 5.0
            #include "Tessellation.cginc"
			#include "UnityCG.cginc"

			uniform float _TerrainTess;
            uniform float _TerrainDisplacement;
			uniform float _TerrainSnow;

			sampler2D _MainTex;
            sampler2D _HeightTex;
			sampler2D _NormalMap;
            fixed4 _Color;

			float4 _SnowColor;
			float4 _SnowDirection;

			inline float4 LightingCustomDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten)
			{
				float difLight = dot (s.Normal, lightDir);
				float hLambert = difLight * 0.5 + 0.5;
				float4 col;
				col.rgb = s.Albedo * _LightColor0.rgb * (hLambert * atten * 2);
				col.a = s.Alpha;
				return col;
			}

			struct appdata 
			{
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

			float4 tessFixed()
            {
                return _TerrainTess;
            }

            void vert (inout appdata v)
            {
                float d = tex2Dlod(_HeightTex, float4(v.texcoord.xy, 0, 0)).r * _TerrainDisplacement;
                v.vertex.xyz += v.normal * d;
            }

            struct Input 
			{
                float2 uv_MainTex;
				float2 uv_NormalMap;
				float2 uv_SnowTex;
				float3 worldNormal;
				INTERNAL_DATA
            };

            void surf (Input IN, inout SurfaceOutput o) 
			{
                half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

				o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));

				if (dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz) > lerp(1, -1, _TerrainSnow/10000)) 
				{
					o.Albedo = _SnowColor.rgb;
				} 
				else 
				{
					o.Albedo = c.rgb;
				}
            }
 
            ENDCG
        }
        FallBack "Diffuse"
}
