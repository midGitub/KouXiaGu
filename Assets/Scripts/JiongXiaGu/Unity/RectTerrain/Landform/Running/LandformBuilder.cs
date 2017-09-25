using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Grids;

namespace JiongXiaGu.Unity.RectTerrain
{


    public class LandformBuilder : TerrainBuilder<RectCoord, LandformChunkRenderer>
    {
        public LandformBuilder(LandformBaker baker)
        {
            Baker = baker;
        }

        public LandformBaker Baker { get; private set; }

        protected override TerrainChunkInfo<RectCoord, LandformChunkRenderer> Create(RectCoord chunkPos)
        {
            return new LandformChunkInfo(Baker, chunkPos);
        }
    }
}
