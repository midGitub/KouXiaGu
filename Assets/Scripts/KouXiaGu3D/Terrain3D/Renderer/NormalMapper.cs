using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.ImageEffects;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据高度图生成法线贴图;
    /// </summary>
    [Serializable]
    public class NormalMapper
    {
        protected NormalMapper() { }

        [SerializeField]
        Shader normalMapShader;
        /// <summary>
        /// 法线图取样半径;
        /// </summary>
        [SerializeField]
        float normalMapRadius = 5;

        [SerializeField]
        bool isBlur = true;
        [SerializeField, Range(0, 10)]
        float blurRadius = 1;
        [SerializeField, Range(0,5)]
        int downSample = 0;
        [SerializeField, Range(0, 8)]
        int iteration = 2;

        Material _normalMapMaterial;

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

        RenderTexture Blur(Texture texture)
        {
            var blur = ImageEffect.GaussianBlur(texture, blurRadius, downSample, iteration);
            return blur;
        }

    }

}
