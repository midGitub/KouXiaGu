using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// shader 功能测试;
    /// </summary>
    public sealed class TestBlendShader : MonoBehaviour
    {

        const string savePath = @"Assets\TestTex";

        [SerializeField]
        Texture heightMap;

        [SerializeField]
        Texture heightAdjustMap;

        [SerializeField]
        Shader blendHeight;

        [ContextMenu("输出高度图;")]
        void OutPutHeightMAP()
        {
            Material blendHeightMaterial = new Material(blendHeight);

            blendHeightMaterial.SetTexture("_HeightMap", heightMap);
            blendHeightMaterial.SetTexture("_HeightAdjustMap", heightAdjustMap);

            RenderTexture rt = RenderTexture.GetTemporary(heightMap.width, heightMap.height);
            Graphics.Blit(heightMap, rt, blendHeightMaterial);

            rt.SavePNG(savePath);
            RenderTexture.ReleaseTemporary(rt);

            DestroyImmediate(blendHeightMaterial);
        }

        [SerializeField]
        Texture diffuseMap;

        [SerializeField]
        Texture diffuseBlendTex;

        [SerializeField]
        Shader blendDiffuseShader;

        [ContextMenu("输出漫反射图;")]
        void OutPutDiffuseMap()
        {
            Material blendHeightMaterial = new Material(blendDiffuseShader);

            blendHeightMaterial.SetTexture("_BaseMap", diffuseMap);
            blendHeightMaterial.SetTexture("_SecondMap", diffuseBlendTex);

            RenderTexture rt = RenderTexture.GetTemporary(diffuseMap.width, diffuseMap.height);
            Graphics.Blit(null, rt, blendHeightMaterial);

            rt.SavePNG(savePath);
            RenderTexture.ReleaseTemporary(rt);

            DestroyImmediate(blendHeightMaterial);
        }

        [ContextMenu("烘焙平整高度;")]
        void RanderHeigt()
        {
            //var col = new Color(128f / 255f, 128f / 255f, 128f / 255f, 1f);
            //var col2 = new Color(1, 1, 1, 0f);

            RenderTexture rt = RenderTexture.GetTemporary(512, 512, 24);
            Renderer.CameraRender(rt);
            rt.SavePNG(savePath);
            RenderTexture.ReleaseTemporary(rt);
        }

    }

}
