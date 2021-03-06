﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形块渲染;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class LandformChunkRenderer : MonoBehaviour
    {
        private LandformChunkRenderer()
        {
        }

        public const string tessellationName = "_LandformTess";
        public const string DisplacementName = "_LandformDisplacement";
        private MeshRenderer meshRenderer;
        private event Action<LandformChunkRenderer> onHeightChanged;
        private bool isHeightChanged;

        private Material material
        {
            get { return meshRenderer.material; }
        }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseMap { get; private set; }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture2D HeightMap { get; private set; }

        /// <summary>
        /// 法线贴图;
        /// </summary>
        public Texture2D NormalMap { get; private set; }

        /// <summary>
        /// 当地形块高度发生变化时调用;
        /// </summary>
        public event Action<LandformChunkRenderer> OnHeightChanged
        {
            add { onHeightChanged += value; }
            remove { onHeightChanged -= value; }
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        /// <summary>
        /// 重置参数;
        /// </summary>
        public void Reset()
        {
            Destroy();
        }

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
        public void Destroy()
        {
            SetDiffuseMap(null);
            SetHeightMap(null);
            SetNormalMap(null);
        }

        /// <summary>
        /// 应用贴图,设置完毕后需要手动调用;
        /// </summary>
        public void Apply()
        {
            if (isHeightChanged && onHeightChanged != null)
            {
                onHeightChanged(this);
            }
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,或超出范围,则返回0;
        /// </summary>
        public float GetHeight(Vector2 uv)
        {
            if (HeightMap == null)
                return 0;

            Color pixelColor = HeightMap.GetPixel(uv);
            return pixelColor.r * GetDisplacement();
        }

        public static float GetTessellation()
        {
            float value = Shader.GetGlobalFloat(tessellationName);
            return value;
        }

        public static void SetTessellation(float value)
        {
            Shader.SetGlobalFloat(tessellationName, value);
        }

        public static float GetDisplacement()
        {
            float value = Shader.GetGlobalFloat(DisplacementName);
            return value;
        }

        public static void SetDisplacement(float value)
        {
            Shader.SetGlobalFloat(DisplacementName, value);
        }
    }
}
