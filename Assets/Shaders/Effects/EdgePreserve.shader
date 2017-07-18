/*
    EdgePreserve
    保留图片不同颜色区域的边缘;
    计划用于: 区域边界线;
*/

Shader "Effects/EdgePreserve"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_StencilMap("StencilMap", 2D) = "Black" {}
        _OutLineWidth ("OutLineWidth", float) = 10
        _Transparency ("Transparency", Range(0, 1)) = 0.2
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
                fixed2 uv : TEXCOORD0;
                fixed2 up : TEXCOORD1;
                fixed2 down : TEXCOORD2;
                fixed2 left : TEXCOORD3;
                fixed2 right : TEXCOORD4;
                fixed2 upLeft : TEXCOORD5;
                fixed2 upRight : TEXCOORD6;
                fixed2 downLeft : TEXCOORD7;
                fixed2 downRight : TEXCOORD8;
			};

            const fixed4 zero = fixed4(0, 0, 0, 0);
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
			sampler2D _StencilMap;
            float _OutLineWidth;
            float _Transparency;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

                fixed2 offsets = _OutLineWidth * _MainTex_TexelSize.xy;
                o.up = v.uv + fixed2(0, offsets.y);
                o.down = v.uv + fixed2(0, -offsets.y);
                o.left = v.uv + fixed2(-offsets.x, 0);
                o.right = v.uv + fixed2(offsets.x, 0);
                o.upLeft = v.uv + fixed2(-offsets.x, offsets.y);
                o.upRight = v.uv + fixed2(offsets.x, offsets.y);
                o.downLeft = v.uv + fixed2(-offsets.x, -offsets.y);
                o.downRight = v.uv + fixed2(offsets.x, -offsets.y);
				return o;
			}
			
            bool IsClose(fixed3 n1, fixed3 n2)
            {
                const fixed minValue = 0.01;

                n1 -= n2;
                n1 = abs(n1);
                return n1.r <= minValue && n1.g <= minValue && n1.b <= minValue;
            }

            fixed OverlayBlendMode(fixed basePixel, fixed blendPixel) 
            {  
                if (basePixel < 0.5) {  
                    return (2.0 * basePixel * blendPixel);  
                } else {  
                    return (1.0 - 2.0 * (1.0 - basePixel) * (1.0 - blendPixel));  
                }  
            }  

            fixed4 OverlayBlendMode(fixed4 basePixel, fixed4 blendPixel)
            {
                blendPixel.r = OverlayBlendMode(basePixel.r, blendPixel.r);
                blendPixel.b = OverlayBlendMode(basePixel.b, blendPixel.b);
                blendPixel.g = OverlayBlendMode(basePixel.g, blendPixel.g);
                return blendPixel;
            }


			fixed4 frag (v2f i) : SV_Target
			{
                fixed4 stencilColor = tex2D(_StencilMap, i.uv);

                if(!IsClose(stencilColor, zero))
                {
                    fixed4 offsetColor;

                    offsetColor = tex2D(_StencilMap, i.up);
                    if(!IsClose(offsetColor, stencilColor))
                        return stencilColor;

                    offsetColor = tex2D(_StencilMap, i.down);
                    if(!IsClose(offsetColor, stencilColor))
                        return stencilColor;

                    offsetColor = tex2D(_StencilMap, i.left);
                    if(!IsClose(offsetColor, stencilColor))
                        return stencilColor;

                    offsetColor = tex2D(_StencilMap, i.right);
                    if(!IsClose(offsetColor, stencilColor))
                        return stencilColor;

                    offsetColor = tex2D(_StencilMap, i.upLeft);
                    if(!IsClose(offsetColor, stencilColor))
                        return stencilColor;

                    offsetColor = tex2D(_StencilMap, i.upRight);
                    if(!IsClose(offsetColor, stencilColor))
                        return stencilColor;

                    offsetColor = tex2D(_StencilMap, i.downLeft);
                    if(!IsClose(offsetColor, stencilColor))
                        return stencilColor;

                    offsetColor = tex2D(_StencilMap, i.downRight);
                    if(!IsClose(offsetColor, stencilColor))
                        return stencilColor;

                    fixed4 originalColor = tex2D(_MainTex, i.uv);
                    return originalColor + (stencilColor * _Transparency);
                }
                else
                {
                       fixed4 originalColor = tex2D(_MainTex, i.uv);
                       return originalColor;
                }
			}
			ENDCG
		}
	}
}