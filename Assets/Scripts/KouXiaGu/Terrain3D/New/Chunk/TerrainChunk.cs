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
            Create();
        }

        /// <summary>
        /// 实例一个地形块,并指定名称;
        /// </summary>
        static TerrainChunk CraeteTerrainChunk()
        {
            GameObject gameObject = new GameObject("TerrainChunk", ChunkScripts);
            return gameObject.GetComponent<TerrainChunk>();
        }

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

        #endregion


        TerrainChunk()
        {
        }

        TerrainMesh terrainMesh;
        [SerializeField]
        TerrainRenderer terrainRenderer;
        TerrainTrigger trigger;

        public TerrainMesh Mesh
        {
            get { return terrainMesh; }
        }

        public TerrainRenderer Texture
        {
            get { return terrainRenderer; }
        }

        [ContextMenu("重新初始化;")]
        void Init()
        {
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var meshCollider = GetComponent<MeshCollider>();

            terrainMesh = new TerrainMesh(meshFilter);
            terrainRenderer = new TerrainRenderer(meshRenderer);
            trigger = new TerrainTrigger(meshCollider, terrainRenderer, terrainRenderer.OnHeightMapUpdate);
        }

        void Init(TerrainChunkTexture textures)
        {
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var meshCollider = GetComponent<MeshCollider>();

            terrainMesh = new TerrainMesh(meshFilter);
            terrainRenderer = new TerrainRenderer(meshRenderer, textures);
            trigger = new TerrainTrigger(meshCollider, terrainRenderer, terrainRenderer.OnHeightMapUpdate);
        }

        void OnValidate()
        {
            terrainRenderer.OnValidate();
        }

        //void Reset()
        //{
        //    Mesh.Reset();
        //    Texture.Clear();
        //}

    }

}
