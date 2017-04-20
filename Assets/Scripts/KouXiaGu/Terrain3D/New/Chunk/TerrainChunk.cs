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
    /// 地形块脚本;
    /// </summary>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class TerrainChunk : MonoBehaviour
    {

        #region Static;

        /// <summary>
        /// 地形块挂载的脚本;
        /// </summary>
        static readonly Type[] ChunkScripts = new Type[]
            {
                typeof(MeshFilter),
                typeof(TerrainChunk),
            };


#if UNITY_EDITOR
        [MenuItem("GameObject/Create Other/TerrainChunk")]
#endif
        static void _CraeteTerrainChunk()
        {
            new GameObject("TerrainChunk", ChunkScripts);
        }

        /// <summary>
        /// 实例一个地形块,并指定名称;
        /// </summary>
        static GameObject CraeteTerrainChunk()
        {
            GameObject gameObject = new GameObject("TerrainChunk", ChunkScripts);
            gameObject.transform.SetParent(GetChunkParent(), false);
            return gameObject;
        }

        /// <summary>
        /// 放置地形块的父节点;
        /// </summary>
        static Transform chunkParent;
        static Transform GetChunkParent()
        {
            return chunkParent ?? (chunkParent = new GameObject("TerrainChunks").transform);
        }

        #endregion


        TerrainChunk()
        {
        }

        public RectCoord ChunkCoord { get; private set; }
        public TerrainMesh Mesh { get; private set; }

        void Awake()
        {
            var meshFilter = GetComponent<MeshFilter>();
            Mesh = new TerrainMesh(meshFilter);
        }

        void Reset()
        {
            Mesh.Reset();
        }

    }

}
