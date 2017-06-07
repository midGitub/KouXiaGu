//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 地形块贴图信息;
//    /// </summary>
//    public class ChunkTexture
//    {
//        public ChunkTexture()
//        {
//        }

//        public ChunkTexture(ChunkTexture textures)
//        {
//            if (textures != null)
//            {
//                SetTextures(textures);
//            }
//        }

//        /// <summary>
//        /// 漫反射贴图;
//        /// </summary>
//        public Texture2D DiffuseMap { get; protected set; }

//        /// <summary>
//        /// 高度贴图;
//        /// </summary>
//        public Texture2D HeightMap { get; protected set; }

//        /// <summary>
//        /// 道路漫反射贴图;
//        /// </summary>
//        public Texture2D RoadDiffuseMap { get; protected set; }

//        /// <summary>
//        /// 道路高度贴图;
//        /// </summary>
//        public Texture2D RoadHeightMap { get; protected set; }

//        /// <summary>
//        /// 法线贴图;
//        /// </summary>
//        public Texture2D NormalMap { get; protected set; }

//        public virtual void SetDiffuseMap(Texture2D diffuseMap)
//        {
//            DiffuseMap = diffuseMap;
//        }

//        public virtual void SetHeightMap(Texture2D heightMap)
//        {
//            HeightMap = heightMap;
//        }

//        public virtual void SetRoadDiffuseMap(Texture2D roadDiffuseMap)
//        {
//            RoadDiffuseMap = roadDiffuseMap;
//        }

//        public virtual void SetRoadHeightMap(Texture2D roadHeightMap)
//        {
//            RoadHeightMap = roadHeightMap;
//        }

//        public virtual void SetNormalMap(Texture2D normalMap)
//        {
//            NormalMap = normalMap;
//        }

//        /// <summary>
//        /// 设置所有贴图,若传入参数为Null,则返回异常;
//        /// </summary>
//        public virtual void SetTextures(ChunkTexture textures)
//        {
//            if (textures == null)
//                throw new ArgumentNullException("textures");

//            SetDiffuseMap(textures.DiffuseMap);
//            SetHeightMap(textures.HeightMap);
//            SetRoadDiffuseMap(textures.RoadDiffuseMap);
//            SetRoadHeightMap(textures.RoadHeightMap);
//            SetNormalMap(textures.NormalMap);
//        }

//        /// <summary>
//        /// 更新所有贴图,若贴图为 Null,则保持原有贴图;若传入参数为Null,则返回异常;
//        /// </summary>
//        public virtual void UpdateTextures(ChunkTexture textures)
//        {
//            if (textures == null)
//                throw new ArgumentNullException("textures");

//            if (textures.DiffuseMap != null)
//                SetDiffuseMap(textures.DiffuseMap);

//            if (textures.HeightMap != null)
//                SetHeightMap(textures.HeightMap);

//            if (textures.RoadDiffuseMap != null)
//                SetRoadDiffuseMap(textures.RoadDiffuseMap);

//            if (textures.RoadHeightMap != null)
//                SetRoadHeightMap(textures.RoadHeightMap);

//            if (textures.NormalMap != null)
//                SetNormalMap(textures.NormalMap);
//        }

//        /// <summary>
//        /// 销毁所有贴图;
//        /// </summary>
//        public virtual void Destroy()
//        {
//            GameObject.Destroy(DiffuseMap);
//            GameObject.Destroy(HeightMap);
//            GameObject.Destroy(RoadDiffuseMap);
//            GameObject.Destroy(RoadHeightMap);
//            GameObject.Destroy(NormalMap);
//            Clear();
//        }

//        /// <summary>
//        /// 清空贴图引用;
//        /// </summary>
//        public virtual void Clear()
//        {
//            SetDiffuseMap(null);
//            SetHeightMap(null);
//            SetRoadDiffuseMap(null);
//            SetRoadHeightMap(null);
//            SetNormalMap(null);
//        }
//    }

//}
