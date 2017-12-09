//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JiongXiaGu.Unity.RectTerrain
//{

//    /// <summary>
//    /// 抽象类 块创建器;
//    /// </summary>
//    public abstract class ChunkBuilder<TPoint, TChunk, TData>
//    {
//        private readonly Dictionary<TPoint, ChunkInfo> chunks;
//        private readonly List<KeyValuePair<TPoint, ChunkInfo>> destroyingChunks;

//        public ChunkBuilder()
//        {
//            chunks = new Dictionary<TPoint, ChunkInfo>();
//            destroyingChunks = new List<KeyValuePair<TPoint, ChunkInfo>>();
//        }

//        protected abstract Task<TData> CreateChunk(TPoint point, CancellationToken token);
//        protected abstract Task DestroyChunk(TChunk chunk, CancellationToken token);
//        protected abstract IEnumerable<TPoint> GetPointsToDisplay();

//        /// <summary>
//        /// 创建地形块;
//        /// </summary>
//        public Task Create(TPoint point)
//        {
//            ChunkInfo chunkInfo;
//            if (chunks.TryGetValue(point, out chunkInfo))
//            {
//                return InternalCreate(point, chunkInfo);
//            }
//            else
//            {
//                chunkInfo = new ChunkInfo();
//                return InternalCreate(point, chunkInfo);
//            }
//        }

//        public Task Update(TPoint point)
//        {
//            throw new NotImplementedException();
//        }

//        public Task Destroy(TPoint point)
//        {
//            throw new NotImplementedException();
//        }

//        //private Task InternalCreate(TPoint point, ChunkInfo chunkInfo)
//        //{
//        //    lock (chunkInfo.StateLock)
//        //    {
//        //        switch (chunkInfo.State)
//        //        {
//        //            case ChunkState.Destroying:
//        //                chunkInfo.SetState(ChunkState.Completed);
//        //                return Task.CompletedTask;

//        //            case ChunkState.None:
//        //                chunkInfo.SetState(ChunkState.Creating);
//        //                CancellationTokenSource tokenSource = chunkInfo.CancellationTokenSource = new CancellationTokenSource();
//        //                return chunkInfo.Task = CreateChunk(point, tokenSource.Token).ContinueWith(delegate (Task<TChunk> task)
//        //                {
//        //                    lock (chunkInfo.StateLock)
//        //                    {
//        //                        if (chunkInfo.State == ChunkState.Creating)
//        //                        {
//        //                            chunkInfo.Chunk = task.Result;
//        //                            chunkInfo.State = ChunkState.Completed;
//        //                            chunkInfo.CancellationTokenSource = null;
//        //                            chunkInfo.Task = null;
//        //                        }
//        //                        else
//        //                        {
//        //                            throw new OperationCanceledException();
//        //                        }
//        //                    }
//        //                }, tokenSource.Token);

//        //            case ChunkState.Creating:
//        //            case ChunkState.Updating:
//        //                return chunkInfo.Task;

//        //            default:
//        //                throw new NotImplementedException();
//        //        }
//        //    }
//        //}

//        private Task InternalUpdate(TPoint point, ChunkInfo chunkInfo)
//        {
//            lock (chunkInfo.StateLock)
//            {
//                switch (chunkInfo.State)
//                {
//                    case ChunkState.Destroying:
//                        chunkInfo.SetState(ChunkState.Updating);
//                        CancellationTokenSource tokenSource = chunkInfo.CancellationTokenSource = new CancellationTokenSource();
//                        throw new NotImplementedException();

//                    default:
//                        throw new NotImplementedException();
//                }
//            }
//        }

//        protected sealed class ChunkInfo
//        {
//            public object StateLock { get; private set; } = new object();
//            public TChunk Chunk { get; internal set; }
//            public ChunkState State { get; internal set; }
//            public Task Task { get; internal set; }
//            public CancellationTokenSource CancellationTokenSource { get; internal set; }

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

//        protected enum ChunkState
//        {
//            /// <summary>
//            /// 未创建;
//            /// </summary>
//            None,

//            /// <summary>
//            /// 创建中;
//            /// </summary>
//            Creating,

//            /// <summary>
//            /// 创建完成;
//            /// </summary>
//            Completed,

//            /// <summary>
//            /// 正在更新中;
//            /// </summary>
//            Updating,

//            /// <summary>
//            /// 正在销毁中;
//            /// </summary>
//            Destroying,
//        }
//    }
//}
