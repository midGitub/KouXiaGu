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
        Empty,

        /// <summary>
        /// 正在创建中;
        /// </summary>
        Creating,

        /// <summary>
        /// 创建完成;
        /// </summary>
        Created,

        /// <summary>
        /// 正在更新中;
        /// </summary>
        Updating,

        /// <summary>
        /// 正在销毁;
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
        event Action<IChunkInfo<TPoint, TChunk>> OnChangedEvent;
    }

    /// <summary>
    /// 块创建;
    /// </summary>
    public abstract class ChunkBuilder<TPoint, TChunk>
         where TChunk : class
    {
        public ChunkBuilder(IRequestDispatcher requestDispatcher)
        {
            chunks = new Dictionary<TPoint, ChunkInfo>();
            RequestDispatcher = requestDispatcher;
        }

        readonly Dictionary<TPoint, ChunkInfo> chunks;

        /// <summary>
        /// 请求处置器;
        /// </summary>
        public IRequestDispatcher RequestDispatcher { get; private set; }

        /// <summary>
        /// 创建/提供一个块请求;
        /// </summary>
        protected abstract ChunkRequest CreateChunkRequest(ChunkInfo info);

        /// <summary>
        /// 创建块,若已经存在则返回实例,不存在则创建到;
        /// </summary>
        public IChunkInfo<TPoint, TChunk> Create(TPoint point)
        {
            ChunkInfo state;
            if (!chunks.TryGetValue(point, out state))
            {
                state = new ChunkInfo(this, point);
                chunks.Add(point, state);
            }
            state.CreateChunk();
            return state;
        }

        /// <summary>
        /// 更新块,若不存在则返回null;
        /// </summary>
        public IChunkInfo<TPoint, TChunk> Update(TPoint point)
        {
            ChunkInfo state;
            if (chunks.TryGetValue(point, out state))
            {
                state.UpdateChunk();
                return state;
            }
            return null;
        }

        /// <summary>
        /// 销毁块,若不存在则返回NULL;
        /// </summary>
        public IChunkInfo<TPoint, TChunk> Destroy(TPoint point)
        {
            ChunkInfo state;
            if (chunks.TryGetValue(point, out state))
            {
                state.DestroyChunk();
                return state;
            }
            return null;
        }

        /// <summary>
        /// 提供内部使用,移除对应块信息;
        /// </summary>
        bool RemoveChunkInfo_internal(TPoint point)
        {
            return chunks.Remove(point);
        }

        /// <summary>
        /// 块状态信息;
        /// </summary>
        protected class ChunkInfo : IChunkInfo<TPoint, TChunk>
        {
            public ChunkInfo(ChunkBuilder<TPoint, TChunk> parent, TPoint point)
            {
                Parent = parent;
                Point = point;
                State = ChunkState.Empty;
            }

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
            /// 块请求,若不存在则为Null;
            /// </summary>
            ChunkRequest request;

            /// <summary>
            /// 当块发生变化时调用;
            /// </summary>
            Action<IChunkInfo<TPoint, TChunk>> onChangedEvent;

            /// <summary>
            /// 当块发生变化时调用;
            /// </summary>
            public event Action<IChunkInfo<TPoint, TChunk>> OnChangedEvent
            {
                add { onChangedEvent += value; }
                remove { onChangedEvent -= value; }
            }

            internal void OnChunkDataChanged()
            {
                if (onChangedEvent != null)
                {
                    onChangedEvent(this);
                }
            }

            public bool CreateChunk()
            {
                ValidateChunkRequest();
                if (request.SetRequestType(ChunkRequestType.Create))
                {
                    State = ChunkState.Creating;
                    return true;
                }
                return false;
            }

            public bool UpdateChunk()
            {
                ValidateChunkRequest();
                if (request.SetRequestType(ChunkRequestType.Update))
                {
                    State = ChunkState.Updating;
                    return true;
                }
                return false;
            }

            public bool DestroyChunk()
            {
                ValidateChunkRequest();
                if (request.SetRequestType(ChunkRequestType.Destroy))
                {
                    State = ChunkState.Destroying;
                    return true;
                }
                return false;
            }

            void ValidateChunkRequest()
            {
                if (request == null)
                {
                    request = Parent.CreateChunkRequest(this);
                }
            }

            /// <summary>
            /// 设置块,若当前块不为空则返回异常 ArgumentException;
            /// </summary>
            public void SetChunk(TChunk chunk)
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
                State = ChunkState.Created;
            }

            /// <summary>
            /// 移除块,若块已经为空则返回异常 ArgumentException;
            /// </summary>
            public void RemoveChunk()
            {
                if (Chunk == null)
                {
                    throw new ArgumentException();
                }
                Chunk = null;
                State = ChunkState.Empty;

                if (onChangedEvent == null)
                {
                    Parent.RemoveChunkInfo_internal(Point);
                }
            }
        }

        /// <summary>
        /// 请求类型;
        /// </summary>
        protected enum ChunkRequestType
        {
            None,
            Create,
            Update,
            Destroy,
        }

        /// <summary>
        /// 块请求;
        /// </summary>
        protected abstract class ChunkRequest : IAsyncRequest
        {
            public ChunkRequest(IRequestDispatcher requestDispatcher, ChunkInfo state)
            {
                RequestDispatcher = requestDispatcher;
                Info = state;
                RequestType = ChunkRequestType.None;
            }

            readonly object asyncLock = new object();

            /// <summary>
            /// 块状态信息;
            /// </summary>
            public ChunkInfo Info { get; private set; }

            /// <summary>
            /// 请求处置器;
            /// </summary>
            public IRequestDispatcher RequestDispatcher { get; private set; }

            /// <summary>
            /// 请求类型;
            /// </summary>
            public ChunkRequestType RequestType { get; private set; }

            /// <summary>
            /// 是否正在操作队列中?
            /// </summary>
            public bool InQueue { get; private set; }

            /// <summary>
            /// 当前操作是否已被取消?
            /// </summary>
            protected bool IsCancelled { get; private set; }

            /// <summary>
            /// 是否正在进行任何操作中?
            /// </summary>
            public bool IsBusy { get; private set; }

            /// <summary>
            /// 当前操作内容;
            /// </summary>
            IEnumerator currentCoroutine;

            protected abstract IEnumerator CreateCoroutine();
            protected abstract IEnumerator UpdateCoroutine();
            protected abstract IEnumerator DestroyCoroutine();

            /// <summary>
            /// 设置请求类型;
            /// </summary>
            public bool SetRequestType(ChunkRequestType requestType)
            {
                lock (asyncLock)
                {
                    if (IsBusy)
                    {
                        if (requestType == ChunkRequestType.None)
                        {
                            IsCancelled = true;
                        }
                        return false;
                    }

                    RequestType = requestType;
                    switch (requestType)
                    {
                        case ChunkRequestType.None:
                            currentCoroutine = null;
                            return true;

                        case ChunkRequestType.Create:
                            currentCoroutine = CreateCoroutine();
                            break;

                        case ChunkRequestType.Update:
                            currentCoroutine = UpdateCoroutine();
                            break;

                        case ChunkRequestType.Destroy:
                            currentCoroutine = DestroyCoroutine();
                            break;

                        default:
                            throw new ArgumentException("requestType");
                    }
                    if (!InQueue)
                    {
                        RequestDispatcher.Add(this);
                    }
                    return true;
                }
            }

            void IAsyncRequest.OnAddQueue()
            {
                lock (asyncLock)
                {
                    InQueue = true;
                    IsBusy = false;
                    IsCancelled = false;
                }
            }

            bool IAsyncRequest.Operate()
            {
                lock (asyncLock)
                {
                    IsBusy = true;
                    return currentCoroutine.MoveNext();
                }
            }

            void IAsyncRequest.OnQuitQueue()
            {
                lock (asyncLock)
                {
                    InQueue = false;
                    IsBusy = false;
                    IsCancelled = false;
                    RequestType = ChunkRequestType.None;
                }
            }
        }
    }
}
