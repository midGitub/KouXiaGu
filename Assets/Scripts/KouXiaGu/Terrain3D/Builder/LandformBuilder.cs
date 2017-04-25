using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块创建管理;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformBuilder : MonoBehaviour
    {
        public static LandformBuilder Initialise(IWorldData worldData, Landform landform)
        {
            var item = SceneObject.GetObject<LandformBuilder>();
            item.WorldData = worldData;
            return item;
        }

        LandformBuilder()
        {
        }

        IWorldData WorldData;

        [SerializeField]
        Stopwatch runtimeStopwatch;
        CoroutineQueue<ChunkRequest> requestQueue;

        public Stopwatch RuntimeStopwatch
        {
            get { return runtimeStopwatch; }
            set { runtimeStopwatch = value; }
        }

        /// <summary>
        /// 是否正在构建中?
        /// </summary>
        public bool IsBuilding
        {
            get { return requestQueue.Count == 0; }
        }

        void Awake()
        {
            requestQueue = new CoroutineQueue<ChunkRequest>(runtimeStopwatch);
        }

        void Update()
        {
            requestQueue.Next();
        }

        public IAsyncOperation<Chunk> Create(RectCoord chunkCoord)
        {
            throw new NotImplementedException();
        }

        void AddRequest(ChunkRequest request)
        {
            requestQueue.Add(request);
        }

        /// <summary>
        /// 取消所有请求;
        /// </summary>
        void CanceleAll()
        {
            foreach (var request in requestQueue)
            {
                request.Dispose();
            }
            requestQueue.Clear();
        }

    }

}
