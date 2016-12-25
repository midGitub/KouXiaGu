Shader "HexTerrain/DiffuseBlend" {
Properties {
	_MainTex ("Base", 2D) = "black" {}
    _Blend ("Mixer", 2D) = "black" {}
}


SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
    	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;                
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
            sampler2D _Blend;  
            
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                fixed4 col = tex2D(_MainTex, i.texcoord);
                fixed4 mixer = tex2D(_Blend, i.texcoord);

				col.a = mixer.r;

				return col;
			}
		ENDCG
	}
}

}
