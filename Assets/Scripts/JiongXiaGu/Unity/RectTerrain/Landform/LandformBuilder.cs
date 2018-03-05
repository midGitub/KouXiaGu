//using JiongXiaGu.Grids;
//using JiongXiaGu.Unity.Maps;
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace JiongXiaGu.Unity.RectTerrain
//{

//    /// <summary>
//    /// 地形构建器;
//    /// </summary>
//    [Serializable]
//    public class LandformBuilder 
//    {
//        private LandformBaker baker;
//        private LandformChunkPool landformChunkPool;
//        private LandformResCreater landformResPool;
//        private WorldMap map;
//        private readonly ObservedBuff<DictionaryEvent<RectCoord, MapNode>> mapChanged;
//        private readonly Dictionary<RectCoord, ChunkInfo> chunks;
//        private readonly List<KeyValuePair<RectCoord, ChunkInfo>> waitChunks;

//        public LandformBuilder(WorldMap map)
//        {
//            this.map = map;
//            mapChanged = new ObservedBuff<DictionaryEvent<RectCoord, MapNode>>();
//            chunks = new Dictionary<RectCoord, ChunkInfo>();

//            using (map.Lock.WriteLock())
//            {
//                map.MapChangedTracker.Subscribe(mapChanged);
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        internal void MoveNext()
//        {
//            using (map.Lock.ReadLock())
//            {

//            }
//        }

//        /// <summary>
//        /// 枚举需要显示的坐标;
//        /// </summary>
//        private IEnumerable<RectCoord> EnumeratePointsToDisplay()
//        {
//            return LandformDisplayGuider.EnumeratePointsToDisplay();
//        }

//        /// <summary>
//        /// 创建地形块;
//        /// </summary>
//        public void Create(RectCoord point)
//        {
//            ChunkInfo chunkInfo;
//            if (chunks.TryGetValue(point, out chunkInfo))
//            {
//                Create(point, chunkInfo);
//            }
//            else
//            {
//                LandformChunkRenderer chunk = landformChunkPool.Get();
//                chunkInfo = new ChunkInfo(chunk);
//                Create(point, chunkInfo);
//            }
//        }

//        public bool Update(RectCoord point)
//        {
//            ChunkInfo chunkInfo;
//            if (chunks.TryGetValue(point, out chunkInfo))
//            {
//                Update(point, chunkInfo);
//                return true;
//            }
//            return false;
//        }

//        public bool Destroy(RectCoord point)
//        {
//            ChunkInfo chunkInfo;
//            if (chunks.TryGetValue(point, out chunkInfo))
//            {
//                Destroy(point, chunkInfo);
//                return true;
//            }
//            return false;
//        }

//        private void Create(RectCoord chunkCoord, ChunkInfo chunkInfo)
//        {
//            lock (chunkInfo.StateLock)
//            {
//                switch (chunkInfo.State)
//                {
//                    case ChunkState.Destroy:
//                        chunkInfo.SetState(ChunkState.Complete);
//                        break;

//                    case ChunkState.Complete:
//                    case ChunkState.Create:
//                        break;

//                    case ChunkState.None:
//                        chunkInfo.State = ChunkState.Create;
//                        CancellationTokenSource tokenSource = chunkInfo.CancellationTokenSource = new CancellationTokenSource();
//                        chunkInfo.Task = InternalCreate(chunkCoord, chunkInfo, tokenSource.Token);
//                        break;

//                    default:
//                        throw new NotImplementedException();
//                }
//            }
//        }

//        private async Task InternalCreate(RectCoord chunkCoord, ChunkInfo chunkInfo, CancellationToken token)
//        {
//            LandformBakeRequest request = await CreateBakeRequest(chunkCoord, token);
//            await TaskHelper.Run(delegate()
//            {
//                var result = baker.Bake(request);
//                lock (chunkInfo.StateLock)
//                {
//                    if (chunkInfo.State == ChunkState.Create)
//                    {
//                        chunkInfo.Chunk.SetDiffuseMap(result.DiffuseMap);
//                        chunkInfo.Chunk.SetHeightMap(result.HeightMap);
//                    }
//                    else
//                    {
//                        result.Destroy();
//                    }
//                }

//            }, token, UnityUpdateTaskScheduler.TaskScheduler);
//        }

//        private void Update(RectCoord chunkCoord, ChunkInfo chunkInfo)
//        {
//            lock (chunkInfo.StateLock)
//            {
//                switch (chunkInfo.State)
//                {
//                    case ChunkState.Destroy:
//                    case ChunkState.Complete:
//                    case ChunkState.Create:
//                        chunkInfo.SetState(ChunkState.Create);
//                        throw new NotImplementedException();

//                    case ChunkState.None:
//                    default:
//                        throw new NotImplementedException();
//                }
//            }
//        }

//        private void Destroy(RectCoord chunkCoord, ChunkInfo chunkInfo)
//        {
//            lock (chunkInfo.StateLock)
//            {
//                switch (chunkInfo.State)
//                {
//                    case ChunkState.Destroy:
//                    case ChunkState.None:
//                        break;

//                    case ChunkState.Complete:
//                        break;

//                    case ChunkState.Create:
//                        break;

//                    default:
//                        throw new NotImplementedException();
//                }
//            }
//        }

//        //private void InternalUpdate(RectCoord chunkCoord, ChunkInfo chunkInfo)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        /// <summary>
//        /// 创建烘培请求;
//        /// </summary>
//        private async Task<LandformBakeRequest> CreateBakeRequest(RectCoord chunkCoord, CancellationToken token)
//        {
//            var childPoints = LandformBaker.GetBakePoints(chunkCoord);
//            List<LandformBakeNode> bakePoints = new List<LandformBakeNode>();
//            List<Task> tasks = new List<Task>();

//            foreach (var childPoint in childPoints)
//            {
//                MapNode mapNode;
//                if (map.Map.TryGetValue(childPoint, out mapNode))
//                {
//                    Task task = landformResPool.Get(mapNode.Landform.TypeID).ContinueWith(delegate (Task<LandformRes> res)
//                    {
//                        LandformBakeNode node = new LandformBakeNode()
//                        {
//                            Position = chunkCoord,
//                            Node = mapNode.Landform,
//                        };
//                        bakePoints.Add(node);
//                    });
//                    tasks.Add(task);
//                }
//                else
//                {
//                    LandformBakeNode node = new LandformBakeNode()
//                    {
//                        Position = chunkCoord,
//                        Node = mapNode.Landform,
//                        Res = landformResPool.Default(),
//                    };
//                    bakePoints.Add(node);
//                }
//            }

//            await Task.WhenAll(tasks);
//            LandformBakeRequest request = new LandformBakeRequest(chunkCoord, bakePoints);
//            return request;
//        }


//        private sealed class ChunkInfo
//        {
//            public object StateLock { get; private set; } = new object();
//            public LandformChunkRenderer Chunk { get; private set; }
//            public ChunkState State { get; internal set; }
//            public CancellationTokenSource CancellationTokenSource { get; internal set; }
//            public Task Task { get; internal set; }

//            public ChunkInfo(LandformChunkRenderer chunk)
//            {
//                Chunk = chunk;
//            }

//            /// <summary>
//            /// 设置一个新的状态,并且取消之前的操作;
//            /// </summary>
//            public void SetState(ChunkState state)
//            {
//                State = state;
//                CancellationTokenSource?.Cancel();
//                CancellationTokenSource = null;
//                Task = null;
//            }
//        }

//        private enum ChunkState
//        {
//            None,
//            Create,
//            Complete,
//            Destroy,
//        }

//        //[SerializeField]
//        //private LandformChunkPool landformChunkPool;
//        //[SerializeField]
//        //private LandformBaker baker;

//        //private LandformResPool landformResPool;
//        //private WorldMap map;
//        //private ObservedBuff<DictionaryEvent<RectCoord, MapNode>> mapChanged;
//        //private Dictionary<RectCoord, LandformChunk> chunks;
//        //private readonly object asyncLock = new object();

//        //private void Initialize(WorldMap map, LandformResPool landformResPool)
//        //{
//        //    this.map = map;
//        //    this.landformResPool = landformResPool;
//        //    mapChanged = new ObservedBuff<DictionaryEvent<RectCoord, MapNode>>();
//        //    chunks = new Dictionary<RectCoord, LandformChunk>();

//        //    using (map.Lock.WriteLock())
//        //    {
//        //        map.MapChangedTracker.Subscribe(mapChanged);
//        //    }
//        //}

//        ///// <summary>
//        ///// 更新地形,在非Unity线程调用;
//        ///// </summary>
//        //internal void UpdateLandform()
//        //{
//        //    using (map.Lock.ReadLock())
//        //    {
//        //        UpdateChunkState();
//        //        UpdateChunkContent();
//        //    }
//        //}

//        ///// <summary>
//        ///// 更新地形块状态;
//        ///// </summary>
//        //private void UpdateChunkState()
//        //{
//        //    while(mapChanged.Count != 0)
//        //    {
//        //        var changed = mapChanged.Dequeue();

//        //        LandformChunk chunk;
//        //        if (chunks.TryGetValue(changed.Key, out chunk))
//        //        {
//        //            if (changed.EventType == DictionaryEventType.Update)
//        //            {
//        //                var newValue = changed.NewValue.Landform;
//        //                var original = changed.OriginalValue.Landform;

//        //                if (newValue.TypeID == original.TypeID && newValue.Angle == original.Angle)
//        //                {
//        //                    continue;
//        //                }
//        //            }

//        //            chunk.IsObsolete = true;
//        //        }
//        //    }
//        //}

//        ///// <summary>
//        ///// 更新地形块内容;
//        ///// </summary>
//        //private void UpdateChunkContent()
//        //{
//        //    foreach (var displayPoint in EnumeratePointsToDisplay())
//        //    {
//        //        LandformChunk chunk;
//        //        if (chunks.TryGetValue(displayPoint, out chunk))
//        //        {
//        //            if (!chunk.IsObsolete)
//        //            {
//        //                continue;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            LandformChunkRenderer renderer = landformChunkPool.Get();
//        //            chunk = new LandformChunk(renderer);
//        //            chunks.Add(displayPoint, chunk);
//        //        }

//        //        CreateBakeRequest(displayPoint, chunk);
//        //    }
//        //}

//        //private void Bake(LandformChunkRenderer renderer, LandformBakeRequest request, CancellationToken token)
//        //{
//        //    LandformBakeResult result = baker.Bake(request);
//        //    renderer.SetDiffuseMap(result.DiffuseMap);
//        //    renderer.SetHeightMap(result.HeightMap);
//        //}

//        ///// <summary>
//        ///// 枚举需要显示的坐标;
//        ///// </summary>
//        //private IEnumerable<RectCoord> EnumeratePointsToDisplay()
//        //{
//        //    return LandformDisplayGuider.EnumeratePointsToDisplay();
//        //}

//        //private class LandformChunk
//        //{

//        //    public LandformChunkRenderer Renderer { get; set; }

//        //    /// <summary>
//        //    /// 表示地形块在Unity线程进行的操作;
//        //    /// </summary>
//        //    public Task UnityTask { get; set; }

//        //    /// <summary>
//        //    /// 是否已经过时了?地图节点已经在更新其之后发生了变化;
//        //    /// </summary>
//        //    public bool IsObsolete { get; set; }

//        //    public LandformChunk(LandformChunkRenderer renderer)
//        //    {
//        //        Renderer = renderer;
//        //    }
//        //}
//    }
//}
