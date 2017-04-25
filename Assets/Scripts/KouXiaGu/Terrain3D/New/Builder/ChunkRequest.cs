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
        public ChunkRequest(RectCoord chunkCoord, LandformBuilder builder)
        {
            Builder = builder;
            ChunkCoord = chunkCoord;
            Current = null;
            ChunkCenter = ChunkCoord.GetChunkHexCenter();
            BakeCoroutine = Bake();
        }

        public LandformBuilder Builder { get; private set; }
        public RectCoord ChunkCoord { get; private set; }
        public object Current { get; private set; }
        public CubicHexCoord ChunkCenter { get; private set; }
        public IEnumerator BakeCoroutine { get; private set; }

        protected abstract IEnumerator Bake();

        /// <summary>
        /// 取消创建请求;
        /// </summary>
        public virtual void Dispose()
        {
            OnCanceled();
        }

        bool IEnumerator.MoveNext()
        {
            bool moveNext = BakeCoroutine.MoveNext();
            Current = BakeCoroutine.Current;
            return moveNext;
        }

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }

    }

}
