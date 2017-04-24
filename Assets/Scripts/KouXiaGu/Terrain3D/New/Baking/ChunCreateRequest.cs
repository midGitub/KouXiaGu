using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{


    /// <summary>
    /// 烘培请求;
    /// </summary>
    public abstract class ChunCreateRequest : AsyncOperation<ChunkTexture>, IBakingRequest
    {
        public ChunCreateRequest(RectCoord chunkCoord, IWorldData worldData)
        {
            WorldData = worldData;
            ChunkCoord = chunkCoord;
            Current = null;
            bakeCoroutine = Bake();
        }

        public IWorldData WorldData { get; private set; }
        public RectCoord ChunkCoord { get; private set; }
        public object Current { get; private set; }
        IEnumerator bakeCoroutine;

        protected LandformBaker Baker { get; private set; }
        protected CubicHexCoord ChunkCenter { get; private set; }
        protected IEnumerable<CubicHexCoord> Displays { get; private set; }

        protected abstract IEnumerator BakeCoroutine();

        IEnumerator Bake()
        {
            ChunkCenter = ChunkCoord.GetChunkHexCenter();
            Displays = GetOverlaye();
            yield return BakeCoroutine();
        }

        /// <summary>
        /// 取消创建请求;
        /// </summary>
        public virtual void Cancel()
        {
            OnCanceled();
        }

        public void Dispose()
        {
            Baker.Reset();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取到覆盖到的坐标;
        /// </summary>
        IEnumerable<CubicHexCoord> GetOverlaye()
        {
            return ChunkPartitioner.GetLandform(ChunkCoord);
        }
    }

}
