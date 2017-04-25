using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块操作请求;
    /// </summary>
    public abstract class ChunkRequest : AsyncOperation<Chunk>, IEnumerator, IDisposable
    {
        public ChunkRequest(RectCoord chunkCoord, Landform landform)
        {
            Landform = landform;
            ChunkCoord = chunkCoord;
            Current = null;
            ChunkCenter = ChunkCoord.GetChunkHexCenter();
            Coroutine = Operate();
        }

        public Landform Landform { get; private set; }
        public RectCoord ChunkCoord { get; private set; }
        public object Current { get; private set; }
        public CubicHexCoord ChunkCenter { get; private set; }
        public IEnumerator Coroutine { get; private set; }

        /// <summary>
        /// 进行的操作;
        /// </summary>
        protected abstract IEnumerator Operate();

        /// <summary>
        /// 取消创建请求;
        /// </summary>
        public virtual void Dispose()
        {
            OnCanceled();
        }

        bool IEnumerator.MoveNext()
        {
            bool moveNext = Coroutine.MoveNext();
            Current = Coroutine.Current;
            return moveNext;
        }

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }

    }

}
