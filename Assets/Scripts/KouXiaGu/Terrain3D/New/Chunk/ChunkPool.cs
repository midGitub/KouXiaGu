using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块池;
    /// </summary>
    public class ChunkPool : ObjectPool<Chunk>
    {
        public ChunkPool()
        {
        }

        public override Chunk Instantiate()
        {
            Chunk chunk = Chunk.Create();
            return chunk;
        }

        public override void Reset(Chunk chunk)
        {
            chunk.Clear();
        }

        public override void Destroy(Chunk chunk)
        {
            chunk.Destroy();
        }
    }

}
