using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 烘培请求;
    /// </summary>
    public class BakeRequest : AsyncOperation<Chunk>, IEnumerator
    {
        public BakeRequest(RectCoord chunkCoord, LandformBaker baker)
        {
            Baker = baker;
            ChunkCoord = chunkCoord;
            Current = null;
            ChunkCenter = ChunkCoord.GetChunkHexCenter();
            Coroutine = Operate();
        }

        public LandformBaker Baker { get; private set; }
        public RectCoord ChunkCoord { get; private set; }
        public object Current { get; private set; }
        public CubicHexCoord ChunkCenter { get; private set; }
        public IEnumerator Coroutine { get; private set; }

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

        public void Cancele()
        {
            OnFaulted(new OperationCanceledException());
        }

        /// <summary>
        /// 进行的操作;
        /// </summary>
        IEnumerator Operate()
        {
            if (IsCompleted)
                yield break;


        }

    }

}
