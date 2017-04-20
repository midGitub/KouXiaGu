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
    public sealed class TerrainChunk : MonoBehaviour
    {

        #region Static;

        /// <summary>
        /// 地形块挂载的脚本;
        /// </summary>
        static readonly Type[] ChunkScripts = new Type[]
            {
                typeof(MeshFilter)
                ,typeof(MeshRenderer)
                ,typeof(TerrainChunk)
            };


#if UNITY_EDITOR
        [MenuItem("GameObject/Create Other/TerrainChunk")]
#endif
        static void _CraeteTerrainChunk()
        {
            new GameObject("TerrainChunk", ChunkScripts);
        }

        /// <summary>
        /// 放置地形块的父节点;
        /// </summary>
        static Transform chunkParent;
        static Transform GetChunkParent()
        {
            return chunkParent ?? (chunkParent = new GameObject("TerrainChunks").transform);
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

        #endregion


        TerrainChunk()
        {
        }


        public RectCoord ChunkCoord { get; private set; }

        TerrainMesh mesh;

        [SerializeField]
        TerrainRenderer terrainRenderer;

        public TerrainMesh Mesh
        {
            get { return mesh; }
            private set { mesh = value; }
        }

        public TerrainRenderer Renderer
        {
            get { return terrainRenderer; }
            private set { terrainRenderer = value; }
        }

        void Awake()
        {
            var meshFilter = GetComponent<MeshFilter>();
            Mesh = new TerrainMesh(meshFilter);

            var meshRenderer = GetComponent<MeshRenderer>();
            Renderer = new TerrainRenderer(meshRenderer);
        }

        void OnValidate()
        {
            Renderer.OnValidate();
        }

        void Reset()
        {
            Mesh.Reset();
        }

    }

}
