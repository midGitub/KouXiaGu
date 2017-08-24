using KouXiaGu.Concurrent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    public class ChunkGuider<TPoint>
    {
        /// <summary>
        /// 获取到需要创建的坐标;
        /// </summary>
        public IEnumerable<TPoint> GetCreate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取到需要销毁的坐标;
        /// </summary>
        public IEnumerable<TPoint> GetDestroy()
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 块当前的状态;
    /// </summary>
    public enum ChunkState
    {
        /// <summary>
        /// 未创建;
        /// </summary>
        None,

        /// <summary>
        /// 正在创建中;
        /// </summary>
        Creating,

        /// <summary>
        /// 创建完成;
        /// </summary>
        Completed,

        /// <summary>
        /// 正在更新中;
        /// </summary>
        Updating,

        /// <summary>
        /// 正在销毁中;
        /// </summary>
        Destroying,
    }

    /// <summary>
    /// 块信息;
    /// </summary>
    public interface IChunkInfo<TPoint, TChunk>
    {
        TPoint Point { get; }
        TChunk Chunk { get; }
        ChunkState State { get; }
        void AddListener(Action<IChunkInfo<TPoint, TChunk>> listener);
        void RemoveListener(Action<IChunkInfo<TPoint, TChunk>> listener);
    }

    /// <summary>
    /// 块创建;
    /// </summary>
    public abstract class ChunkBuilder<TPoint, TChunk> : IEnumerable<IChunkInfo<TPoint, TChunk>>
         where TChunk : class
    {
        public ChunkBuilder(IRequestDispatcher requestDispatcher)
        {
            chunks = new Dictionary<TPoint, ChunkInfo>();
            RequestDispatcher = requestDispatcher;
        }

        readonly Dictionary<TPoint, ChunkInfo> chunks;

        /// <summary>
        /// 当任何块变化事件;
        /// </summary>
        Action<IChunkInfo<TPoint, TChunk>> onAnyChunkValueChanged;

        /// <summary>
        /// 请求处置器;
        /// </summary>
        public IRequestDispatcher RequestDispatcher { get; private set; }

        /// <summary>
        /// 获取到对应块信息,若不存在则返回异常 ArgumentException();
        /// </summary>
        public IChunkInfo<TPoint, TChunk> this[TPoint point]
        {
            get { return chunks[point]; }
        }

        /// <summary>
        /// 创建块,若已经存在则返回实例,不存在则创建到;
        /// </summary>
        public IChunkInfo<TPoint, TChunk> CreateChunk(TPoint point)
        {
            ChunkInfo info = GetOrCreate(point);
            info.CreateChunk();
            return info;
        }

        /// <summary>
        /// 获取到块信息,若不存在则创建到;
        /// </summary>
        protected ChunkInfo GetOrCreate(TPoint point)
        {
            ChunkInfo info;
            if (!chunks.TryGetValue(point, out info))
            {
                info = new ChunkInfo(this, point);
                chunks.Add(point, info);
            }
            return info;
        }

        /// <summary>
        /// 更新块,若不存在则返回null;
        /// </summary>
        public IChunkInfo<TPoint, TChunk> UpdateChunk(TPoint point)
        {
            ChunkInfo info;
            if (chunks.TryGetValue(point, out info))
            {
                info.UpdateChunk();
                return info;
            }
            return null;
        }

        /// <summary>
        /// 销毁块,若不存在则返回NULL;
        /// </summary>
        public IChunkInfo<TPoint, TChunk> DestroyChunk(TPoint point)
        {
            ChunkInfo info;
            if (chunks.TryGetValue(point, out info))
            {
                info.DestroyChunk();
                return info;
            }
            return null;
        }

        /// <summary>
        /// 当块发生变化时调用;
        /// </summary>
        void OnAnyChunkValueChanged(IChunkInfo<TPoint, TChunk> info)
        {
            if (onAnyChunkValueChanged != null)
            {
                onAnyChunkValueChanged(info);
            }
        }

        /// <summary>
        /// 添加监视任何块变化;
        /// </summary>
        public void AddListener(Action<IChunkInfo<TPoint, TChunk>> listener)
        {
            onAnyChunkValueChanged += listener;
        }

        /// <summary>
        /// 移除监视任何块变化;
        /// </summary>
        public void RemoveListener(Action<IChunkInfo<TPoint, TChunk>> listener)
        {
            onAnyChunkValueChanged -= listener;
        }

        /// <summary>
        /// 添加监视某个块变化;
        /// </summary>
        public void AddListener(TPoint point, Action<IChunkInfo<TPoint, TChunk>> listener)
        {
            ChunkInfo info  = GetOrCreate(point);
            info.AddListener(listener);
        }

        /// <summary>
        /// 移除监视某个块变化;
        /// </summary>
        public void RemoveListener(TPoint point, Action<IChunkInfo<TPoint, TChunk>> listener)
        {
            ChunkInfo info = GetOrCreate(point);
            info.RemoveListener(listener);
        }

        /// <summary>
        /// 尝试获取到对应块信息;
        /// </summary>
        public bool TryGetValue(TPoint point, out IChunkInfo<TPoint, TChunk> chunkInfo)
        {
            ChunkInfo info;
            bool find = chunks.TryGetValue(point, out info);
            chunkInfo = info;
            return find;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IChunkInfo<TPoint, TChunk>> GetEnumerator()
        {
            return chunks.Values.Cast<IChunkInfo<TPoint, TChunk>>().GetEnumerator();
        }


        /// <summary>
        /// 块状态信息;
        /// </summary>
        protected class ChunkInfo : IChunkInfo<TPoint, TChunk>, IAsyncRequest
        {
            public ChunkInfo(ChunkBuilder<TPoint, TChunk> parent, TPoint point)
            {
                Parent = parent;
                Point = point;
                State = ChunkState.None;
            }

            /// <summary>
            /// 实例线程锁;
            /// </summary>
            protected readonly object AsyncLock = new object();

            public ChunkBuilder<TPoint, TChunk> Parent { get; private set; }

            /// <summary>
            /// 块坐标;
            /// </summary>
            public TPoint Point { get; private set; }

            /// <summary>
            /// 块内容;
            /// </summary>
            public TChunk Chunk { get; set; }

            /// <summary>
            /// 块状态;
            /// </summary>
            public ChunkState State { get; private set; }

            /// <summary>
            /// 是否正在处置队列中?
            /// </summary>
            public bool InQueue { get; private set; }

            /// <summary>
            /// 是否正在进行任何操作中?
            /// </summary>
            public bool IsBusy { get; private set; }

            /// <summary>
            /// 当前操作是否已被取消?仅在 IsBusy == true 的时候有效;
            /// </summary>
            protected bool IsCancelled { get; private set; }

            /// <summary>
            /// 当块发生变化时调用;
            /// </summary>
            Action<IChunkInfo<TPoint, TChunk>> onChangedEvent;

            /// <summary>
            /// 添加监视块变化;
            /// </summary>
            public void AddListener(Action<IChunkInfo<TPoint, TChunk>> listener)
            {
                onChangedEvent += listener;
            }

            /// <summary>
            /// 移除监视块变化;
            /// </summary>
            public void RemoveListener(Action<IChunkInfo<TPoint, TChunk>> listener)
            {
                onChangedEvent -= listener;
                TryRemoveFromParent();
            }

            /// <summary>
            /// 从父实例合集中移除本身;若无法移除则返回false;
            /// </summary>
            bool TryRemoveFromParent()
            {
                if (onChangedEvent == null && State == ChunkState.None)
                {
                    Parent.chunks.Remove(Point);
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 当块发生变化时手动调用;
            /// </summary>
            protected void OnChunkDataChanged()
            {
                if (onChangedEvent != null)
                {
                    onChangedEvent(this);
                }
                Parent.OnAnyChunkValueChanged(this);
            }

            /// <summary>
            /// 创建块;若 正在进行其它操作中 则返回false;
            /// </summary>
            public bool CreateChunk()
            {
                lock (AsyncLock)
                {
                    if (IsBusy)
                    {
                        if (State != ChunkState.Creating)
                        {
                            IsCancelled = true;
                        }
                        return false;
                    }
                    else if (State == ChunkState.Destroying)
                    {
                        State = ChunkState.None;
                        return true;
                    }
                    else if (State == ChunkState.None)
                    {
                        State = ChunkState.Creating;
                        TryAddDispatcherQueue();
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            /// <summary>
            /// 更新块内容;若 正在进行其它操作中 则返回false;
            /// </summary>
            public bool UpdateChunk()
            {
                lock (AsyncLock)
                {
                    if (IsBusy)
                    {
                        if (State != ChunkState.Updating || State != ChunkState.Creating)
                        {
                            IsCancelled = true;
                        }
                        return false;
                    }
                    else if (State == ChunkState.Destroying)
                    {
                        State = ChunkState.Updating;
                        return true;
                    }
                    else if (State == ChunkState.None || State == ChunkState.Completed)
                    {
                        State = ChunkState.Updating;
                        TryAddDispatcherQueue();
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            /// <summary>
            /// 移除块内容;若 正在进行其它操作中 则返回false;
            /// </summary>
            public bool DestroyChunk()
            {
                lock (AsyncLock)
                {
                    if (IsBusy)
                    {
                        if (State != ChunkState.Destroying)
                        {
                            IsCancelled = true;
                        }
                        return false;
                    }
                    else if (State == ChunkState.Completed)
                    {
                        State = ChunkState.Destroying;
                        TryAddDispatcherQueue();
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            /// <summary>
            /// 取消当前操作;若 正在进行其它操作中 则返回false;
            /// </summary>
            public bool Cancel()
            {
                lock (AsyncLock)
                {
                    if (IsBusy)
                    {
                        IsCancelled = true;
                        return true;
                    }
                    else if (State == ChunkState.Updating || State == ChunkState.Destroying)
                    {
                        State = ChunkState.Completed;
                        return true;
                    }
                    else if (State == ChunkState.Creating)
                    {
                        State = ChunkState.None;
                        return true;
                    }
                    else
                    {
                        return true;
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
                    Parent.RequestDispatcher.Add(this);
                }
            }

            /// <summary>
            /// 由处置器调用,提供子类重写,需要实现父类内容;
            /// </summary>
            public virtual void OnAddQueue()
            {
                lock (AsyncLock)
                {
                    InQueue = true;
                    IsBusy = false;
                    IsCancelled = false;
                }
            }

            /// <summary>
            /// 由处置器调用,提供子类重写,不需要实现父类内容;
            /// </summary>
            public virtual bool Prepare()
            {
                return true;
            }

            /// <summary>
            /// 由处置器调用,提供子类重写,需要实现父类内容;
            /// </summary>
            public virtual bool Operate()
            {
                lock (AsyncLock)
                {
                    IsBusy = true;
                    return false;
                }
            }

            /// <summary>
            /// 由处置器调用,提供子类重写,需要实现父类内容;
            /// </summary>
            public virtual void OnQuitQueue()
            {
                lock (AsyncLock)
                {
                    InQueue = false;
                    IsBusy = false;
                    IsCancelled = false;
                }
            }

            /// <summary>
            /// 设置块,若当前块不为空则返回异常 ArgumentException;
            /// </summary>
            protected void SetChunk(TChunk chunk)
            {
                if (chunk == null)
                {
                    throw new ArgumentNullException("chunk");
                }
                if (Chunk != null)
                {
                    throw new ArgumentException();
                }
                Chunk = chunk;
                State = ChunkState.Completed;
            }

            /// <summary>
            /// 移除块,若块已经为空则返回异常 ArgumentException;
            /// </summary>
            protected void RemoveChunk()
            {
                if (Chunk == null)
                {
                    throw new ArgumentException();
                }
                Chunk = null;
                State = ChunkState.None;
                TryRemoveFromParent();
            }
        }
    }
}
