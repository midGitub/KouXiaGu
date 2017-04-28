﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形渲染;
    /// </summary>
    public class LandformRenderer : ChunkTexture
    {

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

        public LandformRenderer()
        {
        }

        public LandformRenderer(MeshRenderer renderer)
        {
            Init(renderer);
            Apply();
        }

        public LandformRenderer(MeshRenderer renderer, ChunkTexture textures)
            : base(textures)
        {
            Init(renderer);
            Apply();
        }

        Material material;
        event Action<LandformRenderer> onHeightChanged;
        bool isHeightChanged;

        /// <summary>
        /// 当地形块高度发生变化时调用;
        /// </summary>
        public event Action<LandformRenderer> OnHeightChanged
        {
            add { onHeightChanged += value; }
            remove { onHeightChanged -= value; }
        }

        void Init(MeshRenderer renderer)
        {
            renderer.sharedMaterial = material = new Material(LandformShader);
        }

        public override void SetDiffuseMap(Texture2D diffuseMap)
        {
            if (DiffuseMap != diffuseMap)
            {
                material.SetTexture("_MainTex", diffuseMap);
                base.SetDiffuseMap(diffuseMap);
            }
        }

        public override void SetHeightMap(Texture2D heightMap)
        {
            if (HeightMap != heightMap)
            {
                material.SetTexture("_HeightTex", heightMap);
                base.SetHeightMap(heightMap);
                isHeightChanged = true;
            }
        }

        public override void SetNormalMap(Texture2D normalMap)
        {
            if (NormalMap != normalMap)
            {
                material.SetTexture("_NormalMap", normalMap);
                base.SetNormalMap(normalMap);
            }
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