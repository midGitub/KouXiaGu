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
    /// 地形块;需要通过静态变量创建;
    /// </summary>
    [DisallowMultipleComponent]
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

        public static TerrainChunk Create()
        {
            var item = CraeteTerrainChunk();
            return item;
        }

        #endregion


        TerrainChunk()
        {
        }

        TerrainMesh terrainMesh;
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

        void Awake()
        {
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var meshCollider = GetComponent<MeshCollider>();

            terrainMesh = new TerrainMesh(meshFilter);
            terrainRenderer = new TerrainRenderer(meshRenderer);
            trigger = new TerrainTrigger(meshCollider, terrainRenderer, terrainRenderer.OnHeightMapUpdate);
        }

        public void Clear()
        {
            terrainRenderer.Clear();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

    }

}
