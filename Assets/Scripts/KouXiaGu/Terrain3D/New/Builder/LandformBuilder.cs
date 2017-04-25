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
    public class LandformBuilder : MonoBehaviour
    {

        public static LandformBuilder Initialise(IWorldData worldData, 
            ChunkSceneManager chunkManager, LandformBakeManager bakeManager)
        {
            var item = SceneObject.GetObject<LandformBuilder>();
            item.WorldData = worldData;
            item.ChunkManager = chunkManager;
            item.BakeManager = bakeManager;
            return item;
        }

        LandformBuilder()
        {
        }

        IWorldData WorldData;
        public ChunkSceneManager ChunkManager { get; private set; }
        public LandformBakeManager BakeManager { get; private set; }

        [SerializeField]
        Stopwatch runtimeStopwatch;
        CoroutineQueue<ChunkRequest> requestQueue;

        public Stopwatch RuntimeStopwatch
        {
            get { return runtimeStopwatch; }
            set { runtimeStopwatch = value; }
        }

        void Awake()
        {
            requestQueue = new CoroutineQueue<ChunkRequest>(runtimeStopwatch);
        }

        void Update()
        {
            requestQueue.Next();
        }

        /// <summary>
        /// 
        /// </summary>
        public IAsyncOperation<Chunk> Create(RectCoord chunkCoord)
        {
            throw new NotImplementedException();
        }

        public void AddRequest(ChunkRequest request)
        {
            requestQueue.Add(request);
        }

        /// <summary>
        /// 取消所有请求;
        /// </summary>
        public void CanceleAll()
        {
            foreach (var request in requestQueue)
            {
                request.Dispose();
            }
            requestQueue.Clear();
        }

    }

}
