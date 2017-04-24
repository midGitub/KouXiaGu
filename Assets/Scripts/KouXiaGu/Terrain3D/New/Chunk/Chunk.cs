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
    /// 地形块;需要通过静态方法创建;
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
            item.Initialize(null);
            return item;
        }

        public static Chunk Create(ChunkTexture textures)
        {
            var item = CraeteTerrainChunk();
            item.Initialize(textures);
            return item;
        }

        #endregion


        Chunk()
        {
        }

        public LandformMesh Mesh { get; private set; }
        public LandformRenderer Renderer { get; private set; }
        public LandformTrigger Trigger { get; private set; }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public void Initialize(ChunkTexture textures)
        {
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var meshCollider = GetComponent<MeshCollider>();

            Mesh = new LandformMesh(meshFilter);
            Renderer = new LandformRenderer(meshRenderer, textures);
            Trigger = new LandformTrigger(meshCollider, Renderer);

            Renderer.OnHeightChanged += OnHeightChanged;
        }

        void Reset()
        {
            Initialize(null);
        }

        void OnHeightChanged(LandformRenderer renderer)
        {
            Trigger.RebuildCollisionMesh();
        }

        public void Update(ChunkTexture textures)
        {
            Renderer.UpdateTextures(textures);
        }

        /// <summary>
        /// 清除所有引用,备下次重复使用;
        /// </summary>
        public void Clear()
        {
            Renderer.Clear();
        }

        /// <summary>
        /// 销毁Unity实例,并清除所有贴图资源;
        /// </summary>
        public void Destroy()
        {
            Destroy(gameObject);
        }

    }

}
