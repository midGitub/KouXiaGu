using KouXiaGu.Concurrent;
using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地形烘培;
    /// </summary>
    [Serializable]
    public class LandformBuilder : ChunkBuilder<RectCoord, LandformChunkRenderer>
    {
        LandformBakeCamera bakeCamera;

        public LandformBuilder(LandformBakeCamera bakeCamera, IAsyncRequestDispatcher requestDispatcher) : base(requestDispatcher)
        {
            this.bakeCamera = bakeCamera;
        }

        class BakeRequest : ChunkData
        {
            LandformBuilder parent;

            public BakeRequest(LandformBuilder parent, RectCoord point) : base(parent, point)
            {
                this.parent = parent;
            }
        }
    }
}
