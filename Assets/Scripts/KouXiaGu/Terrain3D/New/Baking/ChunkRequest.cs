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
    public abstract class ChunkRequest : AsyncOperation<ChunkTexture>, IBakingRequest
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
        protected IEnumerable<CubicHexCoord> Displays { get; private set; }

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
            Displays = GetOverlaye();
            yield return BakeCoroutine();
            bakerDisposer.Dispose();
        }

        protected abstract IEnumerator BakeCoroutine();

        /// <summary>
        /// 获取到覆盖到的坐标;
        /// </summary>
        IEnumerable<CubicHexCoord> GetOverlaye()
        {
            return ChunkPartitioner.GetLandform(ChunkCoord);
        }

        /// <summary>
        /// 取消创建请求;
        /// </summary>
        public virtual void Dispose()
        {
            OnCanceled();

            if (bakerDisposer != null)
                bakerDisposer.Dispose();
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
