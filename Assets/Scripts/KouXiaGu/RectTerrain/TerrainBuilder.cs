using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.Concurrent;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 块信息;
    /// </summary>
    public interface ITerrainChunkInfo<TPoint, TChunk>
    {
        TPoint Point { get; }
        TChunk Chunk { get; }
        ChunkState State { get; }
    }

    /// <summary>
    /// 块信息;
    /// </summary>
    public abstract class TerrainChunkInfo<TPoint, TChunk> : ITerrainChunkInfo<TPoint, TChunk>, IRequest
    {
        public TerrainChunkInfo(TerrainBuilder<TPoint, TChunk> parent, TPoint point)
        {
            Parent = parent;
            Point = point;
        }

        readonly object asyncLock = new object();
        public TerrainBuilder<TPoint, TChunk> Parent { get; private set; }

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
        public void CreateAsync()
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
        public void UpdateAsync()
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
        public void DestroyAsync()
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
        public void TryAddDispatcherQueue()
        {
            if (!InQueue)
            {
                Parent.RequestDispatcher.Add(this);
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
                onChunkChanged?.Invoke(this);
            }
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

    /// <summary>
    /// 块创建器;
    /// </summary>
    public abstract class TerrainBuilder<TPoint, TChunk>
    {
        public TerrainBuilder()
        {
            chunks = new Dictionary<TPoint, TerrainChunkInfo<TPoint, TChunk>>();
        }

        readonly Dictionary<TPoint, TerrainChunkInfo<TPoint, TChunk>> chunks;

        /// <summary>
        /// 请求处置器;
        /// </summary>
        public IRequestDispatcher RequestDispatcher { get; private set; }

        /// <summary>
        /// 获取到块信息,若不存在则创建到;
        /// </summary>
        protected TerrainChunkInfo<TPoint, TChunk> GetOrCreate(TPoint chunkPos)
        {
            TerrainChunkInfo<TPoint, TChunk> info;
            if (!chunks.TryGetValue(chunkPos, out info))
            {
                info = Create(chunkPos);
                info.OnChunkChanged += OnChunkInfoChanged;
                chunks.Add(chunkPos, info);
            }
            return info;
        }

        void OnChunkInfoChanged(TerrainChunkInfo<TPoint, TChunk> info)
        {
            if (info.State == ChunkState.None)
            {
                chunks.Remove(info.Point);
            }
        }

        /// <summary>
        /// 创建空的块信息;
        /// </summary>
        protected abstract TerrainChunkInfo<TPoint, TChunk> Create(TPoint chunkPos);

        /// <summary>
        /// 创建块,若已经存在则返回实例,不存在则创建到;
        /// </summary>
        public TerrainChunkInfo<TPoint, TChunk> CreateAsync(TPoint chunkPos)
        {
            var info = GetOrCreate(chunkPos);
            info.CreateAsync();
            return info;
        }

        /// <summary>
        /// 更新块,若不存在则返回null;
        /// </summary>
        public TerrainChunkInfo<TPoint, TChunk> UpdateAsync(TPoint chunkPos)
        {
            TerrainChunkInfo<TPoint, TChunk> info;
            if (chunks.TryGetValue(chunkPos, out info))
            {
                info.UpdateAsync();
                return info;
            }
            return null;
        }

        /// <summary>
        /// 销毁块,若不存在则返回NULL;
        /// </summary>
        public TerrainChunkInfo<TPoint, TChunk> DestroyAsync(TPoint chunkPos)
        {
            TerrainChunkInfo<TPoint, TChunk> info;
            if (chunks.TryGetValue(chunkPos, out info))
            {
                info.DestroyAsync();
                return info;
            }
            return null;
        }
    }
}
