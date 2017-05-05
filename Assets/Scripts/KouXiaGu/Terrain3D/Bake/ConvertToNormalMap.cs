using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.ImageEffects;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 高度贴图转法线贴图;
    /// </summary>
    [Serializable]
    class ConvertToNormalMap
    {
        ConvertToNormalMap()
        {
        }

        [SerializeField]
        Shader normalMapShader;
        /// <summary>
        /// 法线图取样半径;
        /// </summary>
        [SerializeField]
        float normalMapRadius = 2;

        [SerializeField]
        bool isBlur = true;
        [SerializeField, Range(0, 10)]
        float blurSize;
        const int downsample = 0;
        [SerializeField, Range(1, 6)]
        int blurIterations;

        Material _normalMapMaterial;
        BakeCamera bakeCamera;

        public Shader NormalMapShader
        {
            get { return normalMapShader; }
            set { normalMapShader = value; }
        }

        public bool IsBlur
        {
            get { return isBlur; }
            set { isBlur = value; }
        }

        Material normalMapMaterial
        {
            get { return _normalMapMaterial ?? (_normalMapMaterial = new Material(normalMapShader)); }
        }

        /// <summary>
        /// 根据高度图获取到法线图;
        /// </summary>
        public RenderTexture Rander(Texture heightMap)
        {
            var rt = NormalMapFormHeight(heightMap);

            if (isBlur)
            {
                var blur = Blur(rt);
                RenderTexture.ReleaseTemporary(rt);
                rt = blur;
            }

            return rt;
        }

        /// <summary>
        /// 根据高度图获取到法线贴图;
        /// </summary>
        RenderTexture NormalMapFormHeight(Texture heightMap)
        {
            RenderTexture normalMapRT = null;
            try
            {
                normalMapRT = RenderTexture.GetTemporary(heightMap.width, heightMap.height, 0, RenderTextureFormat.ARGB32);

                heightMap.filterMode = FilterMode.Bilinear;
                normalMapRT.filterMode = FilterMode.Bilinear;

                normalMapMaterial.SetFloat("_Radius", normalMapRadius);

                Graphics.Blit(heightMap, normalMapRT, normalMapMaterial, 0);
                return normalMapRT;
            }
            catch (Exception ex)
            {
                if (normalMapRT != null)
                    RenderTexture.ReleaseTemporary(normalMapRT);
                throw ex;
            }
        }

        RenderTexture Blur(RenderTexture rt)
        {
            var blurRT = ImageEffect.BlurOptimized(rt, blurSize, downsample, blurIterations, ImageEffect.BlurType.StandardGauss);
            return blurRT;
        }

        /// <summary>
        /// 获取到法线贴图;
        /// </summary>
        public Texture2D GetTexture(RenderTexture rt)
        {
            return bakeCamera.GetHeightTexture(rt, TextureFormat.ARGB32);
        }

    }

}
