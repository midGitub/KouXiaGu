using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Concurrent;
using JiongXiaGu.Grids;

namespace JiongXiaGu.Unity.RectTerrain
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
            var chunk = Baker.LandformChunkPool.Get();
            chunk.transform.position = Point.ToLandformChunkPixel();
            Baker.Bake(Point, chunk);
            return chunk;
        }

        protected override void Update(LandformChunkRenderer chunk)
        {
            Baker.Bake(Point, chunk);
        }

        protected override void Destroy(LandformChunkRenderer chunk)
        {
            Baker.LandformChunkPool.Release(chunk);
        }
    }
}
