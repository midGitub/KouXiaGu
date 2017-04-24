using System;
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
    public class BakeRequest : AsyncOperation<ChunkTexture>, IBakingRequest
    {
        public BakeRequest(LandformBaker baker, RectCoord chunkCoord)
        {
            this.WorldData = baker.WorldData;
            ChunkCoord = chunkCoord;
            ChunkCenter = chunkCoord.GetChunkHexCenter();
            Current = null;
        }

        public IWorldData WorldData{ get; private set; }
        public RectCoord ChunkCoord { get; private set; }
        public CubicHexCoord ChunkCenter { get; private set; }
        public object Current { get; private set; }

        public void Cancel()
        {
            OnCanceled();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

}
