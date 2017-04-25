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
        public CreateChunk(RectCoord chunkCoord, Landform landform) : base(chunkCoord, landform)
        {
        }

        protected override IEnumerator Operate()
        {
            Landform.ChunkManager.Create(ChunkCoord, null);
            yield break;
        }

    }

}
