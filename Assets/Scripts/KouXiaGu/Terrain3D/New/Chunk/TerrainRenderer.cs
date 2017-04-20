using System;
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
    public class TerrainRenderer : TerrainChunkTexture
    {

        static TerrainParameter Parameter
        {
            get { return TerrainParameter.Instance; }
        }

        static Shader TerrainShader
        {
            get { return Parameter.TerrainShader; }
        }

        static float Displacement
        {
            get { return Parameter.Displacement; }
        }

        public TerrainRenderer()
        {
        }

        public TerrainRenderer(MeshRenderer renderer)
        {
            Init(renderer);
        }

        public TerrainRenderer(MeshRenderer renderer, TerrainChunkTexture textures)
        {
            Init(renderer);
            SetTextures(textures);
        }

        Material material;
        LinkedListTracker<TerrainRenderer> onHeightMapUpdate;

        /// <summary>
        /// 当高度贴图发生变化时调用;
        /// </summary>
        public IObservable<TerrainRenderer> OnHeightMapUpdate
        {
            get { return onHeightMapUpdate; }
        }

        void Init(MeshRenderer renderer)
        {
            renderer.sharedMaterial = material = new Material(TerrainShader);
            onHeightMapUpdate = new LinkedListTracker<TerrainRenderer>();
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
                onHeightMapUpdate.Track(this);
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
        /// 获取到高度,若不存在高度信息,则返回0;
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
