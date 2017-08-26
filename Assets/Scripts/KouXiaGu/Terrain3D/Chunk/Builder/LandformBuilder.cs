using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.Concurrent;
using KouXiaGu.Grids;
using System.Collections;

namespace KouXiaGu.Terrain3D
{

    public class LandformBuilder : ChunkBuilder<RectCoord, LandformChunk>
    {
        public LandformBuilder(IRequestDispatcher requestDispatcher) : base(requestDispatcher)
        {
        }

        class LandformChunkInfo : ChunkInfo_Coroutine
        {
            public LandformChunkInfo(ChunkBuilder<RectCoord, LandformChunk> parent, RectCoord point) : base(parent, point)
            {
            }

            protected override IEnumerator CreateChunkCoroutine()
            {
                throw new NotImplementedException();
            }

            protected override IEnumerator UpdateChunkCoroutine()
            {
                throw new NotImplementedException();
            }

            protected override IEnumerator DestroyChunkCoroutine()
            {
                throw new NotImplementedException();
            }
        }
    }
}
