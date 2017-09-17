using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.Concurrent;
using KouXiaGu.Grids;

namespace KouXiaGu.RectTerrain
{

    public class LandformChunkInfo : TerrainChunkInfo<RectCoord, LandformChunkRenderer>
    {
        public LandformChunkInfo(LandformBaker baker, RectCoord point) : base(baker.RequestDispatcher, point)
        {
            Baker = baker;
        }

        public LandformBaker Baker { get; private set; }

        protected override LandformChunkRenderer Create()
        {
            return Baker.CreateChunk(Point);
        }

        protected override void Destroy(LandformChunkRenderer chunk)
        {
            Baker.DestroyChunk(chunk);
        }

        protected override void Update(LandformChunkRenderer chunk)
        {
            Baker.UpdateChunk(Point, chunk);
        }
    }
}
