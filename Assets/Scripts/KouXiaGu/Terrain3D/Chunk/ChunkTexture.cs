﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块贴图信息;
    /// </summary>
    public class ChunkTexture
    {
        public ChunkTexture()
        {
        }

        public ChunkTexture(ChunkTexture textures)
        {
            if (textures != null)
            {
                SetTextures(textures);
            }
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
        /// 设置所有贴图,若传入参数为Null,则返回异常;
        /// </summary>
        public virtual void SetTextures(ChunkTexture textures)
        {
            if (textures == null)
                throw new ArgumentNullException("textures");

            SetDiffuseMap(textures.DiffuseMap);
            SetHeightMap(textures.HeightMap);
            SetNormalMap(textures.NormalMap);
        }

        /// <summary>
        /// 更新所有贴图,若贴图为 Null,则保持原有贴图;若传入参数为Null,则返回异常;
        /// </summary>
        public virtual void UpdateTextures(ChunkTexture textures)
        {
            if (textures == null)
                throw new ArgumentNullException("textures");

            if (textures.DiffuseMap != null)
                SetDiffuseMap(textures.DiffuseMap);

            if (textures.HeightMap != null)
                SetHeightMap(textures.HeightMap);

            if (textures.NormalMap != null)
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
