using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public class TerrainChunkPool
    {

        #region 地形块实例管理(静态)

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        static Dictionary<RectCoord, TerrainChunk> activatedChunks = new Dictionary<RectCoord, TerrainChunk>();

        /// <summary>
        /// 休眠的地形块;
        /// </summary>
        static Queue<TerrainChunk> restingChunks = new Queue<TerrainChunk>();

        /// <summary>
        /// 激活在场景的地形块数目;
        /// </summary>
        public static int ActivatedChunkCount
        {
            get { return activatedChunks.Count; }
        }

        /// <summary>
        /// 重置的\休眠的地形块数目;
        /// </summary>
        public static int RestingChunkCount
        {
            get { return restingChunks.Count; }
        }

        /// <summary>
        /// 创建地形块到场景;
        /// </summary>
        public static TerrainChunk Create(RectCoord coord, Texture2D diffuse, Texture2D height, Texture2D normal)
        {
            if (activatedChunks.ContainsKey(coord))
                throw new ArgumentException("地形块已经创建到场景;");

            TerrainChunk terrainChunk = GetTerrainChunk(coord.ToString());

            terrainChunk.SetChunk(coord, diffuse, height, normal);

            activatedChunks.Add(coord, terrainChunk);

            return terrainChunk;
        }

        /// <summary>
        /// 创建地形块到场景,若已经存在,则更新其贴图;
        /// </summary>
        public static TerrainChunk CreateOrUpdate(RectCoord coord, Texture2D diffuse, Texture2D height, Texture2D normal)
        {
            if (diffuse == null || height == null)
                throw new NullReferenceException("空的贴图!");

            TerrainChunk chunk;

            if (TryGetChunk(coord, out chunk))
            {
                chunk.SetChunk(coord, diffuse, height, normal);
            }
            else
            {
                chunk = Create(coord, diffuse, height, normal);
            }
            return chunk;
        }

        ///// <summary>
        ///// 设置参数到地形块;
        ///// </summary>
        //static void SetChunk(TerrainChunk chunk, RectCoord coord, Texture2D diffuse, Texture2D height, Texture2D normal)
        //{
        //    chunk.Coord = coord;
        //    chunk.DiffuseTexture = diffuse;
        //    chunk.HeightTexture = height;
        //    chunk.NormalMap = normal;
        //}

        /// <summary>
        /// 移除这个地形块;
        /// </summary>
        public static bool Destroy(RectCoord coord)
        {
            TerrainChunk terrainChunk;
            if (activatedChunks.TryGetValue(coord, out terrainChunk))
            {
                terrainChunk.DestroyTextures();

                ReleaseTerrainChunk(terrainChunk);
                activatedChunks.Remove(coord);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除这个地形块,但是返回其贴图信息;
        /// </summary>
        public static bool Destroy(RectCoord coord, out Texture2D diffuse, out Texture2D height)
        {
            TerrainChunk terrainChunk;
            if (activatedChunks.TryGetValue(coord, out terrainChunk))
            {
                diffuse = terrainChunk.DiffuseTexture;
                height = terrainChunk.HeightTexture;
                terrainChunk.ClearTextures();

                ReleaseTerrainChunk(terrainChunk);
                activatedChunks.Remove(coord);
                return true;
            }
            diffuse = default(Texture2D);
            height = default(Texture2D);
            return false;
        }

        /// <summary>
        /// 移除所有的地图块;
        /// </summary>
        public static void DestroyAll()
        {
            RectCoord[] coords = activatedChunks.Keys.ToArray();
            foreach (var coord in coords)
            {
                Destroy(coord);
            }
        }

        /// <summary>
        /// 获取到这个地形块实例;
        /// </summary>
        public static bool TryGetChunk(RectCoord coord, out TerrainChunk chunk)
        {
            return activatedChunks.TryGetValue(coord, out chunk);
        }

        /// <summary>
        /// 确认是否已经实例化这个地形块;
        /// </summary>
        public static bool Contains(RectCoord coord)
        {
            return activatedChunks.ContainsKey(coord);
        }

        /// <summary>
        /// 从池内获取到或者实例化一个;
        /// </summary>
        static TerrainChunk GetTerrainChunk(string name)
        {
            TerrainChunk terrainChunk;
            if (restingChunks.Count > 0)
            {
                terrainChunk = restingChunks.Dequeue();
                terrainChunk.gameObject.SetActive(true);
            }
            else
            {
                GameObject gameObject = new GameObject(name, typeof(TerrainChunk));
                terrainChunk = gameObject.GetComponent<TerrainChunk>();
#if UNITY_EDITOR
                terrainChunk.transform.SetParent(ChunkParent, false);
#endif
            }
            return terrainChunk;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 放置地形块的父节点;
        /// </summary>
        static Transform chunkParent;
        static Transform ChunkParent
        {
            get { return chunkParent ?? (chunkParent = new GameObject("TerrainChunks").transform); }
        }
#endif

        /// <summary>
        /// 将地形块放回池内,备下次使用;
        /// </summary>
        static void ReleaseTerrainChunk(TerrainChunk terrainChunk)
        {
            terrainChunk.gameObject.SetActive(false);
            restingChunks.Enqueue(terrainChunk);
        }

        #endregion

    }

}
