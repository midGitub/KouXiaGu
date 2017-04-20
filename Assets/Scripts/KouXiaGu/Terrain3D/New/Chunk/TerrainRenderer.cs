using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形渲染;
    /// </summary>
    [Serializable]
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

        void Init(MeshRenderer renderer)
        {
            renderer.sharedMaterial = material = new Material(TerrainShader);
        }

        public void OnValidate()
        {
            SetDiffuseMap(DiffuseMap);
            SetHeightMap(HeightMap);
            SetNormalMap(NormalMap);
        }

        public override void SetDiffuseMap(Texture2D diffuseMap)
        {
            material.SetTexture("_MainTex", diffuseMap);
            base.SetDiffuseMap(diffuseMap);
        }

        public override void SetHeightMap(Texture2D heightMap)
        {
            material.SetTexture("_HeightTex", heightMap);
            base.SetHeightMap(heightMap);
        }

        public override void SetNormalMap(Texture2D normalMap)
        {
            material.SetTexture("_NormalMap", normalMap);
            base.SetNormalMap(normalMap);
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public void Destroy()
        {
            GameObject.Destroy(DiffuseMap);
            GameObject.Destroy(HeightMap);
            GameObject.Destroy(NormalMap);
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
