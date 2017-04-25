using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形控制;
    /// </summary>
    public class Landform : MonoBehaviour
    {

        public static Landform Initialize(IWorldData worldData)
        {
            var item = SceneObject.GetObject<Landform>();
            item.worldData = worldData;
            return item;
        }


        Landform()
        {
        }

        IWorldData worldData;
        ChunkSceneManager chunkManager;
        [SerializeField]
        LandformBaker bakeManager;
        [SerializeField]
        Stopwatch runtimeStopwatch;
        CoroutineQueue<ChunkRequest> requestQueue;

        public ChunkSceneManager ChunkManager
        {
            get { return chunkManager; }
        }

        public BakeCamera BakeManager
        {
            get { return bakeManager; }
        }

        public bool IsRunning
        {
            get { return requestQueue.Count == 0; }
        }

        public Stopwatch RuntimeStopwatch
        {
            get { return runtimeStopwatch; }
            set { runtimeStopwatch = value; }
        }

        void Awake()
        {
            chunkManager = new ChunkSceneManager();
            requestQueue = new CoroutineQueue<ChunkRequest>(runtimeStopwatch);
        }

        void Update()
        {
            requestQueue.Next();
        }

        public IAsyncOperation<Chunk> Create(RectCoord chunkCoord)
        {
            var creater = new CreateChunk(chunkCoord, this);
            AddRequest(creater);
            return creater;
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

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord chunkCoord = ChunkInfo.ChunkGrid.GetCoord(position);
            Chunk chunk;
            if (ChunkManager.InSceneChunks.TryGetValue(chunkCoord, out chunk))
            {
                Vector2 uv = ChunkInfo.ChunkGrid.GetUV(chunkCoord, position);
                return chunk.Renderer.GetHeight(uv);
            }
            return 0;
        }

    }

}
