Shader "HexTerrain/HeightToAlpha" 
{

	Properties 
	{	
		_MainTex ("_MainTex", 2D)  = "black" {}    
	}

	SubShader 
	{
		Tags {"Queue"="Geometry" "RenderType"="Opaque"}
		LOD 100
	
		ZWrite Off
		
		Pass 
		{  
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t 
			{
				float4 vertex : POSITION;
			};

			struct v2f 
			{
				float4 vertex : SV_POSITION;
                half2 scrPos : TEXCOORD1;
			};
			
            sampler2D _MainTex;
    		
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.scrPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target			
			{      
                float4 color = 1;

                fixed4 height = tex2D(_MainTex, i.scrPos);
                color.a = (height.r + height.g + height.b) / 3;

				return color;				
			}
		ENDCG
		}
	}

}
