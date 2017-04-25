using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    class CreateChunk : ChunkRequest
    {
        public CreateChunk(RectCoord chunkCoord, LandformBuilder builder) : base(chunkCoord, builder)
        {
        }

        protected override IEnumerator Operate()
        {
            Builder.ChunkManager.Create(ChunkCoord, null);
            yield break;
        }

    }

}
