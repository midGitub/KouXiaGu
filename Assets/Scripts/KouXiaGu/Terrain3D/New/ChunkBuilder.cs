using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    class ChunkBuilder : AsyncOperation<Chunk>
    {
        public static ChunkBuilder Create(RectCoord chunkCoord, ChunkManager chunk)
        {
            return new ChunkBuilder(chunkCoord);
        }

        ChunkBuilder(RectCoord chunkCoord)
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
            ChunkBuilder item = obj as ChunkBuilder;
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
