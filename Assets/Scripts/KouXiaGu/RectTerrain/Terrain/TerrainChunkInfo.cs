using System;
using KouXiaGu.Concurrent;

namespace KouXiaGu.RectTerrain
{
    /// <summary>
    /// 块信息;
    /// </summary>
    public abstract class TerrainChunkInfo<TPoint, TChunk> : ITerrainChunkInfo<TPoint, TChunk>, IRequest
    {
        public TerrainChunkInfo(IRequestDispatcher requestDispatcher, TPoint point)
        {
            this.requestDispatcher = requestDispatcher;
            Point = point;
        }

        readonly object asyncLock = new object();

        /// <summary>
        /// 请求处置器;
        /// </summary>
        IRequestDispatcher requestDispatcher;

        /// <summary>
        /// 块坐标;
        /// </summary>
        public TPoint Point { get; private set; }

        /// <summary>
        /// 地形块实例;
        /// </summary>
        public TChunk Chunk { get; private set; }

        /// <summary>
        /// 块状态;
        /// </summary>
        public ChunkState State { get; private set; }

        /// <summary>
        /// 是否正在处置队列中?
        /// </summary>
        internal bool InQueue { get; private set; }

        /// <summary>
        /// IRequest 接口变量,表示是否完成;
        /// </summary>
        bool isCompleted;

        /// <summary>
        /// IRequest 接口变量,表示是否完成;
        /// </summary>
        bool IRequest.IsCompleted
        {
            get { return isCompleted; }
        }

        /// <summary>
        /// 块内容变化委托;
        /// </summary>
        internal Action<TerrainChunkInfo<TPoint, TChunk>> onChunkChanged { get; private set; }

        /// <summary>
        /// 块内容变化委托;
        /// </summary>
        internal event Action<TerrainChunkInfo<TPoint, TChunk>> OnChunkChanged
        {
            add { onChunkChanged += value; }
            remove { onChunkChanged -= value; }
        }

        /// <summary>
        /// 异步创建;
        /// </summary>
        internal void CreateAsync()
        {
            lock (asyncLock)
            {
                if (State == ChunkState.Destroying)
                {
                    State = ChunkState.None;
                }
                else if (State == ChunkState.None)
                {
                    State = ChunkState.Creating;
                    TryAddDispatcherQueue();
                }
            }
        }

        /// <summary>
        /// 异步更新;
        /// </summary>
        internal void UpdateAsync()
        {
            lock (asyncLock)
            {
                if (State == ChunkState.Destroying)
                {
                    State = ChunkState.Updating;
                }
                else if (State == ChunkState.None || State == ChunkState.Completed)
                {
                    State = ChunkState.Updating;
                    TryAddDispatcherQueue();
                }
            }
        }

        /// <summary>
        /// 异步销毁;
        /// </summary>
        internal void DestroyAsync()
        {
            lock (asyncLock)
            {
                if (State == ChunkState.Completed)
                {
                    State = ChunkState.Destroying;
                    TryAddDispatcherQueue();
                }
                else
                {
                    State = ChunkState.None;
                }
            }
        }

        /// <summary>
        /// 检查添加到处置队列内;
        /// </summary>
        void TryAddDispatcherQueue()
        {
            if (!InQueue)
            {
                requestDispatcher.Add(this);
                InQueue = true;
            }
        }

        /// <summary>
        /// IRequest 接口,对应的操作;
        /// </summary>
        void IRequest.Operate()
        {
            lock (asyncLock)
            {
                switch (State)
                {
                    case ChunkState.Creating:
                        Chunk = Create();
                        State = ChunkState.Completed;
                        break;

                    case ChunkState.Updating:
                        Update(Chunk);
                        State = ChunkState.Completed;
                        break;

                    case ChunkState.Destroying:
                        Destroy(Chunk);
                        Chunk = default(TChunk);
                        State = ChunkState.None;
                        break;
                }
                isCompleted = true;
                InQueue = false;
            }
            onChunkChanged?.Invoke(this);
        }

        /// <summary>
        /// 创建;
        /// </summary>
        protected abstract TChunk Create();

        /// <summary>
        /// 更新;
        /// </summary>
        protected abstract void Update(TChunk chunk);

        /// <summary>
        /// 销毁;
        /// </summary>
        protected abstract void Destroy(TChunk chunk);
    }
}
