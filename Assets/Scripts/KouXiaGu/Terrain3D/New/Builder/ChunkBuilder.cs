using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块创建管理;
    /// </summary>
    public class ChunkBuilder
    {
        public ChunkBuilder(IWorldData data, ChunkSceneManager chunkManager)
        {
            this.data = data;
            this.chunkManager = chunkManager;
        }

        readonly IWorldData data;
        readonly ChunkSceneManager chunkManager;

        IDictionary<CubicHexCoord, MapNode> map
        {
            get { return data.Map.Data; }
        }

        public IAsyncOperation<Chunk> Create(RectCoord chunkCoord)
        {
            throw new NotImplementedException();
        }

    }

}
