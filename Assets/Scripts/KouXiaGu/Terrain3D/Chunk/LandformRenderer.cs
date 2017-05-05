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
    public class LandformRenderer : ChunkTexture
    {
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

        void Init(MeshRenderer renderer)
        {
            renderer.sharedMaterial = material = new Material(LandformShader);
            renderer.shadowCastingMode = shadowCastingMode;
        }

        public override void SetDiffuseMap(Texture2D diffuseMap)
        {
            if (DiffuseMap != diffuseMap)
            {
                material.SetTexture("_DiffuseMap", diffuseMap);
                base.SetDiffuseMap(diffuseMap);
            }
        }

        public override void SetHeightMap(Texture2D heightMap)
        {
            if (HeightMap != heightMap)
            {
                material.SetTexture("_HeightMap", heightMap);
                base.SetHeightMap(heightMap);
                isHeightChanged = true;
            }
        }

        public override void SetRoadDiffuseMap(Texture2D roadDiffuseMap)
        {
            if (RoadDiffuseMap != roadDiffuseMap)
            {
                material.SetTexture("_RoadDiffuseMap", roadDiffuseMap);
                base.SetRoadDiffuseMap(roadDiffuseMap);
            }
        }

        public override void SetRoadHeightMap(Texture2D roadHeightMap)
        {
            if (RoadHeightMap != roadHeightMap)
            {
                material.SetTexture("_RoadHeightMap", roadHeightMap);
                base.SetRoadHeightMap(roadHeightMap);
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
