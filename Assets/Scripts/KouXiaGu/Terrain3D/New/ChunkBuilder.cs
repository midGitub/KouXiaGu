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
        public ChunkBuilder(IWorldData data, ChunkManager chunkManager)
        {
            this.data = data;
            this.chunkManager = chunkManager;
        }

        readonly IWorldData data;
        readonly ChunkManager chunkManager;

        IDictionary<CubicHexCoord, MapNode> map
        {
            get { return data.Map.Data; }
        }

        public IAsyncOperation<Chunk> Create(RectCoord chunkCoord)
        {
            throw new NotImplementedException();
        }

        class CreateChunk : AsyncOperation<Chunk>
        {
            CreateChunk(RectCoord chunkCoord, ChunkManager chunk)
            {
                ChunkCoord = chunkCoord;
            }

            public RectCoord ChunkCoord { get; private set; }

            /// <summary>
            /// 取消创建;
            /// </summary>
            public void Cancel()
            {
                if (IsCompleted)
                    throw new ArgumentException();

                throw new NotImplementedException();
            }

            public override bool Equals(object obj)
            {
                CreateChunk item = obj as CreateChunk;
                if (item == null)
                    return false;
                return item.ChunkCoord == ChunkCoord;
            }

            public override int GetHashCode()
            {
                return ChunkCoord.GetHashCode();
            }

        }

    }

}
