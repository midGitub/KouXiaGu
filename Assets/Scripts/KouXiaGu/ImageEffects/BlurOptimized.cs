using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.ImageEffects
{

    [Serializable]
    public class BlurOptimized
    {
        BlurOptimized()
        {
        }

        [SerializeField]
        Shader blurOptimizedShader;

        Material _blurMaterial;

        Material blurMaterial
        {
            get { return _blurMaterial ?? (_blurMaterial = new Material(blurOptimizedShader)); }
        }

        public enum BlurType
        {
            StandardGauss = 0,
            SgxGauss = 1,
        }

        public RenderTexture Render(
            RenderTexture source,
            float blurSize,
            int downsample,
            int blurIterations,
            BlurType blurType)
        {
            int rtW = source.width >> downsample;
            int rtH = source.height >> downsample;

            // downsample
            RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
            Render(source, rt, blurSize, downsample, blurIterations, blurType);
            return rt;
        }

        public void Render(
            RenderTexture source,
            RenderTexture destination,
            float blurSize,
            int downsample,
            int blurIterations,
            BlurType blurType)
        {
            float widthMod = 1.0f / (1.0f * (1 << downsample));

            blurMaterial.SetVector("_Parameter", new Vector4(blurSize * widthMod, -blurSize * widthMod, 0.0f, 0.0f));
            source.filterMode = FilterMode.Bilinear;

            int rtW = source.width >> downsample;
            int rtH = source.height >> downsample;

            // downsample
            RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);

            rt.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, rt, blurMaterial, 0);

            var passOffs = blurType == BlurType.StandardGauss ? 0 : 2;

            for (int i = 0; i < blurIterations; i++)
            {
                float iterationOffs = (i * 1.0f);
                blurMaterial.SetVector("_Parameter", new Vector4(blurSize * widthMod + iterationOffs, -blurSize * widthMod - iterationOffs, 0.0f, 0.0f));

                // vertical blur
                RenderTexture rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rt, rt2, blurMaterial, 1 + passOffs);
                RenderTexture.ReleaseTemporary(rt);
                rt = rt2;

                // horizontal blur
                rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rt, rt2, blurMaterial, 2 + passOffs);
                RenderTexture.ReleaseTemporary(rt);
                rt = rt2;
            }

            Graphics.Blit(rt, destination);

            RenderTexture.ReleaseTemporary(rt);
        }

    }

}
