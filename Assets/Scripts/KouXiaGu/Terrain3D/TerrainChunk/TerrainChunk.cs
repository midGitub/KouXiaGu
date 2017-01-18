using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块实例控制;
    /// </summary>
    public class TerrainChunk
    {

        /// <summary>
        /// 地形块挂载的脚本;
        /// </summary>
        static readonly Type[] TERRAIN_CHUNK_SCRIPTS = new Type[]
            {
                typeof(TerrainMesh), //地形块控制;
                typeof(TerrainTrigger), //地形碰撞器;
            };


        /// <summary>
        /// 实例一个地形块,并指定名称;
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("GameObject/Create Other/TerrainChunk")]
#endif
        static GameObject CraeteTerrainChunk()
        {
            GameObject gameObject = new GameObject("TerrainChunk", TERRAIN_CHUNK_SCRIPTS);
#if UNITY_EDITOR
            gameObject.transform.SetParent(ChunkParent, false);
#endif
            return gameObject;
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
        /// 创建一个空的地图块到;
        /// </summary>
        public TerrainChunk()
        {
            GameObject gameobjct = CraeteTerrainChunk();
            Renderer = gameobjct.GetComponent<TerrainMesh>();
            Trigger = gameobjct.GetComponent<TerrainTrigger>();
        }

        public RectCoord ChunkCoord
        {
            get { return Renderer.Coord; }
            set { Renderer.Coord = value; }
        }

        public TerrainMesh Renderer { get; private set; }
        public TerrainTrigger Trigger { get; private set; }


        /// <summary>
        /// gameObject.SetActive(value);
        /// </summary>
        public void SetActive(bool value)
        {
            Renderer.gameObject.SetActive(value);
        }

        public void Set(RectCoord coord)
        {
            Renderer.Coord = coord;
        }

        public void Set(TerrainTexPack tex)
        {
            Renderer.DiffuseTexture = tex.diffuseMap;
            Renderer.HeightTexture = tex.heightMap;
            Renderer.NormalMap = tex.normalMap;

            Trigger.ResetCollisionMesh(Renderer);
        }

        public void Set(RectCoord coord, TerrainTexPack tex)
        {
            Set(coord);
            Set(tex);
        }

        /// <summary>
        /// 清空所有信息;
        /// </summary>
        public void Clear()
        {
            Renderer.DestroyTextures();
        }

    }

}
