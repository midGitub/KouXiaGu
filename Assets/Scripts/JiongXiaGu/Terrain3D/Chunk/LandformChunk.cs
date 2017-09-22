using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Grids;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JiongXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块;需要通过静态方法创建;
    /// </summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public sealed class LandformChunk : MonoBehaviour
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
                typeof(LandformChunk)
            };

        static Transform chunkObjectParent;

        public static Transform ChunkObjectParent
        {
            get { return chunkObjectParent ?? (chunkObjectParent = new GameObject("LandformChunk").transform); }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Create Other/LandformChunk")]
#endif
        static void _CraeteLandformChunk()
        {
            new GameObject("LandformChunk", ChunkScripts);
        }

        /// <summary>
        /// 使用默认名实例一个地形块;
        /// </summary>
        public static LandformChunk Create()
        {
            return Create("TerrainChunk");
        }

        /// <summary>
        /// 指定实例名创建一个地形块;
        /// </summary>
        public static LandformChunk Create(string name)
        {
            GameObject gameObject = new GameObject(name, ChunkScripts);
            gameObject.transform.SetParent(ChunkObjectParent);
            LandformChunk chunk = gameObject.GetComponent<LandformChunk>();
            return chunk;
        }

        #endregion


        LandformChunk()
        {
        }

        public bool IsInitialized { get; private set; }
        public LandformMesh Mesh { get; private set; }
        public LandformRenderer Renderer { get; private set; }
        public LandformTrigger Trigger { get; private set; }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        void Awake()
        {
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var meshCollider = GetComponent<MeshCollider>();

            Mesh = new LandformMesh(meshFilter);
            Renderer = new LandformRenderer(meshRenderer);
            Trigger = new LandformTrigger(meshCollider, Renderer);

            Renderer.OnHeightChanged += OnHeightChanged;
            gameObject.layer = LandformRay.Instance.RayLayer;
        }

        /// <summary>
        /// 当地形块高度变化时调用;
        /// </summary>
        void OnHeightChanged(LandformRenderer renderer)
        {
            Trigger.BuildCollisionMesh();
        }

        /// <summary>
        /// 销毁所有贴图,备下次重复使用;
        /// </summary>
        public void ResetData()
        {
            Renderer.Destroy();
        }

        /// <summary>
        /// 销毁Unity实例,并清除所有贴图资源;
        /// </summary>
        public void Destroy()
        {
            Renderer.Destroy();
            Destroy(gameObject);
        }
    }
}
