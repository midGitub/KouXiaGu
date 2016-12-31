using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 将地形和装饰混合;
    /// </summary>
    [Serializable]
    public class DecorateBlend
    {

        [SerializeField]
        Shader blendHeightShader;

        [SerializeField]
        Shader blenddiffuseShader;


        Material blendHeightMaterial;

        Material blendDiffuseMaterial;

        public void Awake()
        {
            blendHeightMaterial = new Material(blendHeightShader);
            blendDiffuseMaterial = new Material(blenddiffuseShader);
        }

        public RenderTexture BlendHeight(Texture heightTex, Texture adjustTex)
        {
            blendHeightMaterial.SetTexture("_HeightMap", heightTex);
            blendHeightMaterial.SetTexture("_HeightAdjustMap", adjustTex);

            RenderTexture rt = RenderTexture.GetTemporary(heightTex.width, heightTex.height);
            Graphics.Blit(null, rt, blendHeightMaterial, 0);
            return rt;
        }

        public RenderTexture BlendDiffuse(Texture diffuseTex, Texture adjustTex)
        {
            blendDiffuseMaterial.SetTexture("_BaseMap", diffuseTex);
            blendDiffuseMaterial.SetTexture("_SecondMap", adjustTex);

            RenderTexture rt = RenderTexture.GetTemporary(diffuseTex.width, diffuseTex.height);
            Graphics.Blit(null, rt, blendDiffuseMaterial, 0);
            return rt;
        }

    }

}
