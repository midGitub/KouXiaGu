using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using UnityEngine.Rendering;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形渲染;
    /// </summary>
    public class LandformRenderer
    {
        public LandformRenderer(MeshRenderer renderer)
        {
            renderer.sharedMaterial = material = new Material(LandformShader);
            renderer.shadowCastingMode = shadowCastingMode;
            Apply();
        }

        Material material;
        event Action<LandformRenderer> onHeightChanged;
        bool isHeightChanged;
        static readonly ShadowCastingMode shadowCastingMode = ShadowCastingMode.TwoSided;

        static LandformSettings Parameter
        {
            get { return LandformSettings.Instance; }
        }

        static Shader LandformShader
        {
            get { return Parameter.LandformShader; }
        }

        static float Displacement
        {
            get { return Parameter.Displacement; }
        }

        /// <summary>
        /// 当地形块高度发生变化时调用;
        /// </summary>
        public event Action<LandformRenderer> OnHeightChanged
        {
            add { onHeightChanged += value; }
            remove { onHeightChanged -= value; }
        }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseMap { get; protected set; }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture2D HeightMap { get; protected set; }

        /// <summary>
        /// 道路漫反射贴图;
        /// </summary>
        public Texture2D RoadDiffuseMap { get; protected set; }

        ///// <summary>
        ///// 道路高度贴图;
        ///// </summary>
        //public Texture2D RoadHeightMap { get; protected set; }

        /// <summary>
        /// 法线贴图;
        /// </summary>
        public Texture2D NormalMap { get; protected set; }

        /// <summary>
        /// 设置到贴图,并且销毁旧贴图;
        /// </summary>
        public void SetDiffuseMap(Texture2D diffuseMap)
        {
            if (DiffuseMap != diffuseMap)
            {
                if (DiffuseMap != null)
                {
                    GameObject.Destroy(DiffuseMap);
                }
                material.SetTexture("_DiffuseMap", diffuseMap);
                DiffuseMap = diffuseMap;
            }
        }

        /// <summary>
        /// 设置到贴图,并且销毁旧贴图;
        /// </summary>
        public void SetHeightMap(Texture2D heightMap)
        {
            if (HeightMap != heightMap)
            {
                if (HeightMap != null)
                {
                    GameObject.Destroy(HeightMap);
                }
                material.SetTexture("_HeightMap", heightMap);
                HeightMap = heightMap;
                isHeightChanged = true;
            }
        }

        /// <summary>
        /// 设置到贴图,并且销毁旧贴图;
        /// </summary>
        public void SetRoadDiffuseMap(Texture2D roadDiffuseMap)
        {
            if (RoadDiffuseMap != roadDiffuseMap)
            {
                if (RoadDiffuseMap != null)
                {
                    GameObject.Destroy(RoadDiffuseMap);
                }
                material.SetTexture("_RoadDiffuseMap", roadDiffuseMap);
                RoadDiffuseMap = roadDiffuseMap;
            }
        }

        ///// <summary>
        ///// 设置到贴图,并且销毁旧贴图;
        ///// </summary>
        //public void SetRoadHeightMap(Texture2D roadHeightMap)
        //{
        //    if (RoadHeightMap != roadHeightMap)
        //    {
        //        if (RoadHeightMap != null)
        //        {
        //            GameObject.Destroy(RoadHeightMap);
        //        }
        //        material.SetTexture("_RoadHeightMap", roadHeightMap);
        //        RoadHeightMap = roadHeightMap;
        //        isHeightChanged = true;
        //    }
        //}

        /// <summary>
        /// 设置到贴图,并且销毁旧贴图;
        /// </summary>
        public void SetNormalMap(Texture2D normalMap)
        {
            if (NormalMap != normalMap)
            {
                if (NormalMap != null)
                {
                    GameObject.Destroy(NormalMap);
                }
                material.SetTexture("_NormalMap", normalMap);
                NormalMap = normalMap;
            }
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public virtual void Destroy()
        {
            SetDiffuseMap(null);
            SetHeightMap(null);
            SetRoadDiffuseMap(null);
            //SetRoadHeightMap(null);
            SetNormalMap(null);
        }

        /// <summary>
        /// 应用到贴图,设置完毕后需要手动调用;
        /// </summary>
        public void Apply()
        {
            if (isHeightChanged && onHeightChanged != null)
                onHeightChanged(this);
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,或超出范围,则返回0;
        /// </summary>
        public float GetHeight(Vector2 uv)
        {
            if (HeightMap == null)
                return 0;

            Color pixelColor = HeightMap.GetPixel(uv);
            return pixelColor.r * Displacement;
        }
    }
}
