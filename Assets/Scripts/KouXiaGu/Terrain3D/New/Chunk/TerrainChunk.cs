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
                typeof(MeshFilter),
                typeof(MeshRenderer),
                typeof(MeshCollider),
                typeof(TerrainChunk)
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
        static TerrainChunk CraeteTerrainChunk()
        {
            GameObject gameObject = new GameObject("TerrainChunk", ChunkScripts);
            gameObject.transform.SetParent(GetChunkParent(), false);
            return gameObject.GetComponent<TerrainChunk>();
        }

        #endregion

        static TerrainChunk Create()
        {
            var item = CraeteTerrainChunk();
            item.Init();
            return item;
        }

        static TerrainChunk Create(TerrainChunkTexture textures)
        {
            var item = CraeteTerrainChunk();
            item.Init(textures);
            return item;
        }


        TerrainChunk()
        {
        }

        TerrainMesh terrainMesh;
        [SerializeField]
        TerrainRenderer terrainRenderer;

        public TerrainMesh Mesh
        {
            get { return terrainMesh; }
        }

        public TerrainRenderer Texture
        {
            get { return terrainRenderer; }
        }

        void Init()
        {
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();

            terrainMesh = new TerrainMesh(meshFilter);
            terrainRenderer = new TerrainRenderer(meshRenderer);
        }

        void Init(TerrainChunkTexture textures)
        {
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();

            terrainMesh = new TerrainMesh(meshFilter);
            terrainRenderer = new TerrainRenderer(meshRenderer, textures);
        }

        void OnValidate()
        {
            terrainRenderer.OnValidate();
        }

        void Reset()
        {
            Mesh.Reset();
            Texture.Clear();
        }

    }

}
