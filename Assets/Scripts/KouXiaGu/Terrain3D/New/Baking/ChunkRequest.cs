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
        public ChunkRequest(Landform landform, RectCoord chunkCoord)
        {
            Landform = landform;
            ChunkCoord = chunkCoord;
            Current = null;
            bakeCoroutine = Bake();
        }

        public Landform Landform { get; private set; }
        public RectCoord ChunkCoord { get; private set; }
        public object Current { get; private set; }
        IEnumerator bakeCoroutine;

        LandformBaker baker;
        IDisposable bakerDisposer;
        protected CubicHexCoord ChunkCenter { get; private set; }

        protected ChunkSceneManager ChunkManager
        {
            get { return Landform.ChunkManager; }
        }

        protected LandformBaker Baker
        {
            get { return baker; }
        }

        IEnumerator Bake()
        {
            bakerDisposer = Landform.BakeManager.GetBakerAndLock(out baker);
            ChunkCenter = ChunkCoord.GetChunkHexCenter();
            yield return BakeCoroutine();
            bakerDisposer.Dispose();
            bakerDisposer = null;
            baker = null;
        }

        protected abstract IEnumerator BakeCoroutine();

        /// <summary>
        /// 取消创建请求;
        /// </summary>
        public virtual void Dispose()
        {
            OnCanceled();

            if (bakerDisposer != null)
            {
                bakerDisposer.Dispose();
                bakerDisposer = null;
                baker = null;
            }
        }

        bool IEnumerator.MoveNext()
        {
            bool moveNext = bakeCoroutine.MoveNext();
            Current = bakeCoroutine.Current;
            return moveNext;
        }

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }

    }

}
