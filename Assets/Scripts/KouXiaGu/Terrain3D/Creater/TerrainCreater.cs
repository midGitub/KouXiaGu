using System;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形创建;
    /// </summary>
    [DisallowMultipleComponent]
    public class TerrainCreater : SceneSington<TerrainCreater>
    {

        /// <summary>
        /// 地形块池;
        /// </summary>
        static ChunkPool RestingChunks
        {
            get { return GetInstance.restingChunks; }
        }

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        static CustomDictionary<RectCoord, TerrainChunk> ActivatedChunks
        {
            get { return GetInstance.activatedChunks; }
        }

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        public static IReadOnlyDictionary<RectCoord, TerrainChunk> ReadOnlyActivatedChunks
        {
            get { return ActivatedChunks; }
        }


        /// <summary>
        /// 创建此地形块,若已经存在 或者 正在创建中 则返回true;
        /// </summary>
        public static bool Create(RectCoord coord)
        {
            NotInstancedException();
            if (ActivatedChunks.ContainsKey(coord) || Request.Requested.ContainsKey(coord))
                return false;

            Request.Create(coord, CreateOrUpdate);
            return true;
        }

        /// <summary>
        /// 创建或者更新已创建的地图块;
        /// </summary>
        public static void CreateOrUpdate(RectCoord coord)
        {
            NotInstancedException();
            Request.Create(coord, CreateOrUpdate);
        }

        static void CreateOrUpdate(Request request)
        {
            RectCoord coord = request.ChunkCoord;
            TerrainTexPack textures = request.Textures;
            TerrainChunk chunk;

            if (!ActivatedChunks.TryGetValue(request.ChunkCoord, out chunk))
            {
                chunk = RestingChunks.Get();
                ActivatedChunks.Add(coord, chunk);
            }

            chunk.Set(coord);
            chunk.Set(textures);
        }

        /// <summary>
        /// 取消创建或者摧毁已创建的;
        /// </summary>
        public static bool Destroy(RectCoord coord)
        {
            NotInstancedException();
            TerrainChunk chunk;

            if (ActivatedChunks.TryGetValue(coord, out chunk))
            {
                chunk.Destroy();
                ActivatedChunks.Remove(coord);
                return true;
            }
            else
            {
                return Request.Remove(coord);
            }
        }


        TerrainCreater()
        {
        }

        /// <summary>
        /// 地形块对象池;
        /// </summary>
        [SerializeField]
        ChunkPool restingChunks;

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        CustomDictionary<RectCoord, TerrainChunk> activatedChunks;

        protected override void Awake()
        {
            base.Awake();
            restingChunks.Initialize();
            activatedChunks = new CustomDictionary<RectCoord, TerrainChunk>();
        }

        protected override void OnDestroy()
        {
            Request.ClearRequested();
            base.OnDestroy();
        }


        /// <summary>
        /// 地形块 对象池;
        /// </summary>
        [Serializable]
        class ChunkPool : ObjectPool<TerrainChunk>
        {
            const int DEFAULT_CAPACITY = 30;

            public ChunkPool() : base(DEFAULT_CAPACITY)
            {
            }

            public ChunkPool(int capacity) : base(capacity)
            {
            }

            protected override TerrainChunk Instantiate()
            {
                return new TerrainChunk();
            }

            protected override void Reset(TerrainChunk item)
            {
                item.Reset();
            }

            protected override void Destroy(TerrainChunk item)
            {
                item.Destroy();
            }
        }

    }

}
