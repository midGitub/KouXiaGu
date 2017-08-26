using KouXiaGu.Concurrent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 显示坐标提供;
    /// </summary>
    public interface IChunkGuider<TPoint>
    {
        /// <summary>
        /// 获取到需要显示的坐标;
        /// </summary>
        IReadOnlyCollection<TPoint> GetPointsToDisplay();
    }

    /// <summary>
    /// 显示合集\组;
    /// </summary>
    public class ChunkGuiderGroup<TPoint> : IChunkGuider<TPoint>
    {
        public ChunkGuiderGroup()
        {
            chunkGuiderList = new List<IChunkGuider<TPoint>>();
            pisplayPointSet = new HashSet<TPoint>();
        }

        List<IChunkGuider<TPoint>> chunkGuiderList;
        HashSet<TPoint> pisplayPointSet;

        public void Add(IChunkGuider<TPoint> guider)
        {
            if (!chunkGuiderList.Contains(guider))
            {
                chunkGuiderList.Add(guider);
            }
        }

        public bool Remove(IChunkGuider<TPoint> guider)
        {
            return chunkGuiderList.Remove(guider);
        }

        public IReadOnlyCollection<TPoint> GetPointsToDisplay()
        {
            pisplayPointSet.Clear();
            foreach(var chunkGuider in chunkGuiderList)
            {
                foreach (var display in chunkGuider.GetPointsToDisplay())
                {
                    pisplayPointSet.Add(display);
                }
            }
            return pisplayPointSet;
        }
    }

    /// <summary>
    /// 更新器;
    /// </summary>
    public class ChunkUpdater<TPoint, TChunk>
    {
        public ChunkUpdater(ChunkBuilder<TPoint, TChunk> builder, IChunkGuider<TPoint> guider)
        {
            Builder = builder;
            GuiderGroup = guider;
            needDestoryPoints = new List<TPoint>();
        }

        public ChunkBuilder<TPoint, TChunk> Builder { get; private set; }
        public IChunkGuider<TPoint> GuiderGroup { get; private set; }
        List<TPoint> needDestoryPoints;

        public void Update()
        {
            IReadOnlyCollection<TPoint> needDisplayPoints = GuiderGroup.GetPointsToDisplay();
            lock (Builder.AsyncLock)
            {
                foreach (var chunk in Builder)
                {
                    if (!needDisplayPoints.Contains(chunk.Point))
                    {
                        needDestoryPoints.Add(chunk.Point);
                    }
                }

                foreach (var destoryPoint in needDestoryPoints)
                {
                    Builder.DestroyChunk(destoryPoint);
                }

                foreach (var createPoint in needDisplayPoints)
                {
                    Builder.CreateChunk(createPoint);
                }
            }
            needDestoryPoints.Clear();
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
    {
        public ChunkBuilder(IRequestDispatcher requestDispatcher)
        {
            chunks = new Dictionary<TPoint, ChunkInfo>();
            RequestDispatcher = requestDispatcher;
        }

        /// <summary>
        /// 实例线程锁;
        /// </summary>
        protected readonly object asyncLock = new object();

        /// <summary>
        /// 实例线程锁;
        /// </summary>
        public object AsyncLock
        {
            get { return asyncLock; }
        }

        /// <summary>
        /// 块信息合集;
        /// </summary>
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

            public ChunkBuilder<TPoint, TChunk> Parent { get; private set; }

            /// <summary>
            /// 实例线程锁;
            /// </summary>
            protected object AsyncLock
            {
                get { return Parent.AsyncLock; }
            }

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

            void IAsyncRequest.OnAddQueue()
            {
                lock (AsyncLock)
                {
                    InQueue = true;
                    IsBusy = false;
                    IsCancelled = false;
                    OnAddQueue();
                }
            }

            protected virtual void OnAddQueue()
            {
            }

            bool IAsyncRequest.Prepare()
            {
                lock (AsyncLock)
                {
                    return Prepare();
                }
            }

            /// <summary>
            /// 提供子类重写,不需要实现父类内容;
            /// </summary>
            protected virtual bool Prepare()
            {
                return true;
            }

            bool IAsyncRequest.Operate()
            {
                lock (AsyncLock)
                {
                    IsBusy = true;
                    return Operate();
                }
            }

            /// <summary>
            /// 提供子类重写,不需要实现父类内容;
            /// </summary>
            protected virtual bool Operate()
            {
                return false;
            }

            void IAsyncRequest.OnQuitQueue()
            {
                lock (AsyncLock)
                {
                    InQueue = false;
                    IsBusy = false;
                    IsCancelled = false;
                    OnQuitQueue();
                    if (State != ChunkState.Completed || State == ChunkState.None)
                    {
                        State = ChunkState.None;
                    }
                }
            }

            protected virtual void OnQuitQueue()
            {
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
                Chunk = default(TChunk);
                State = ChunkState.None;
                TryRemoveFromParent();
            }
        }

        protected abstract class ChunkInfo_Coroutine : ChunkInfo
        {
            public ChunkInfo_Coroutine(ChunkBuilder<TPoint, TChunk> parent, TPoint point) : base(parent, point)
            {
            }

            IEnumerator currentCoroutine;

            protected abstract IEnumerator CreateChunkCoroutine();
            protected abstract IEnumerator UpdateChunkCoroutine();
            protected abstract IEnumerator DestroyChunkCoroutine();

            protected override bool Prepare()
            {
                switch (State)
                {
                    case ChunkState.Creating:
                        currentCoroutine = CreateChunkCoroutine();
                        break;

                    case ChunkState.Updating:
                        currentCoroutine = UpdateChunkCoroutine();
                        break;

                    case ChunkState.Destroying:
                        currentCoroutine = DestroyChunkCoroutine();
                        break;

                    default:
                        return false;
                }
                return true;
            }

            protected override bool Operate()
            {
                return currentCoroutine.MoveNext();
            }

            protected override void OnQuitQueue()
            {
                currentCoroutine = null;
            }
        }
    }
}
