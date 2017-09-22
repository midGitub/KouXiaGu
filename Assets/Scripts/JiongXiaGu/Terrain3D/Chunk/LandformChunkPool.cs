using System;
using System.Collections.Generic;
using JiongXiaGu.Grids;
using UnityEngine;

namespace JiongXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块池;
    /// </summary>
    public class LandformChunkPool : ObjectPool<LandformChunk>
    {
        const int defaultMaxCapacity = 100;

        public LandformChunkPool() : base(defaultMaxCapacity)
        {
        }

        public override LandformChunk Instantiate()
        {
            LandformChunk chunk = LandformChunk.Create();
            return chunk;
        }

        public override void ResetWhenOutPool(LandformChunk chunk)
        {
            chunk.gameObject.SetActive(true);
        }

        public override void ResetWhenEnterPool(LandformChunk chunk)
        {
            chunk.ResetData();
            chunk.gameObject.SetActive(false);
        }

        public override void Destroy(LandformChunk chunk)
        {
            chunk.Destroy();
        }
    }
}
