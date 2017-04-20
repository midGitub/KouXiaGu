using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

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

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseMap { get; protected set; }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture2D HeightMap { get; protected set; }

        /// <summary>
        /// 法线贴图;
        /// </summary>
        public Texture2D NormalMap { get; protected set; }

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
        public virtual void SetTextures()
        {
            SetDiffuseMap(DiffuseMap);
            SetHeightMap(HeightMap);
            SetNormalMap(NormalMap);
        }

        /// <summary>
        /// 设置所有贴图;
        /// </summary>
        public virtual void SetTextures(TerrainChunkTexture textures)
        {
            SetDiffuseMap(textures.DiffuseMap);
            SetHeightMap(textures.HeightMap);
            SetNormalMap(textures.NormalMap);
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public virtual void Destroy()
        {
            GameObject.Destroy(DiffuseMap);
            GameObject.Destroy(HeightMap);
            GameObject.Destroy(NormalMap);
            Clear();
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

}
