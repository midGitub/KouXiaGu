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
    [ExecuteInEditMode]
    public sealed class Chunk : MonoBehaviour
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
                typeof(Chunk)
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
        static Chunk CraeteTerrainChunk()
        {
            GameObject gameObject = new GameObject("TerrainChunk", ChunkScripts);
            return gameObject.GetComponent<Chunk>();
        }

        public static Chunk Create()
        {
            var item = CraeteTerrainChunk();
            return item;
        }

        #endregion


        Chunk()
        {
        }

        LandformMesh terrainMesh;
        LandformRenderer terrainRenderer;
        LandformTrigger trigger;

        public LandformMesh Mesh
        {
            get { return terrainMesh; }
        }

        public LandformRenderer Texture
        {
            get { return terrainRenderer; }
        }

        void Awake()
        {
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var meshCollider = GetComponent<MeshCollider>();

            terrainMesh = new LandformMesh(meshFilter);
            terrainRenderer = new LandformRenderer(meshRenderer);
            trigger = new LandformTrigger(meshCollider, terrainRenderer, terrainRenderer.OnHeightMapUpdate);
        }

        void Reset()
        {
            Awake();
        }

        public void Set(Vector3 position, ChunkTexture textures)
        {
            transform.position = position;
            Texture.SetTextures(textures);
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
