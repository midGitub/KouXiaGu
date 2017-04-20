using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [Serializable]
    public class TerrainChunkTexture
    {
        public TerrainChunkTexture()
        {
        }

        public TerrainChunkTexture(TerrainChunkTexture textures)
        {
            SetTextures(textures);
        }

        public TerrainChunkTexture(Texture2D diffuseMap, Texture2D heightMap, Texture2D normalMap)
        {
            SetDiffuseMap(diffuseMap);
            SetHeightMap(heightMap);
            SetNormalMap(normalMap);
        }

        [SerializeField]
        Texture2D diffuseMap;

        [SerializeField]
        Texture2D heightMap;

        [SerializeField]
        Texture2D normalMap;

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseMap
        {
            get { return diffuseMap; }
            protected set { diffuseMap = value; }
        }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture2D HeightMap
        {
            get { return heightMap; }
            protected set { heightMap = value; }
        }

        /// <summary>
        /// 法线贴图;
        /// </summary>
        public Texture2D NormalMap
        {
            get { return normalMap; }
            protected set { normalMap = value; }
        }

        /// <summary>
        /// 设置漫反射贴图;
        /// </summary>
        public virtual void SetDiffuseMap(Texture2D diffuseMap)
        {
            DiffuseMap = diffuseMap;
        }

        /// <summary>
        /// 设置高度贴图;
        /// </summary>
        public virtual void SetHeightMap(Texture2D heightMap)
        {
            HeightMap = heightMap;
        }

        /// <summary>
        /// 设置法线贴图;
        /// </summary>
        public virtual void SetNormalMap(Texture2D normalMap)
        {
            NormalMap = normalMap;
        }

        /// <summary>
        /// 重设所有贴图;
        /// </summary>
        public void SetTextures()
        {
            SetDiffuseMap(DiffuseMap);
            SetHeightMap(HeightMap);
            SetNormalMap(NormalMap);
        }

        /// <summary>
        /// 设置所有贴图;
        /// </summary>
        public void SetTextures(TerrainChunkTexture textures)
        {
            SetDiffuseMap(textures.diffuseMap);
            SetHeightMap(textures.heightMap);
            SetNormalMap(textures.normalMap);
        }

        /// <summary>
        /// 清空贴图引用;
        /// </summary>
        public virtual void Clear()
        {
            SetDiffuseMap(null);
            SetHeightMap(null);
            SetNormalMap(null);
        }
    }


    /// <summary>
    /// 地形渲染;
    /// </summary>
    [Serializable]
    public class TerrainRenderer : TerrainChunkTexture
    {

        static Shader TerrainShader
        {
            get { return TerrainParameter.Instance.TerrainShader; }
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

    }

}
