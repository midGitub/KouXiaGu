using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Concurrent;
using JiongXiaGu.World;

namespace JiongXiaGu.Terrain3D
{

    /// <summary>
    /// 建筑创建器;
    /// </summary>
    public class BuildingBuilder : ChunkBuilder<CubicHexCoord, BuildingUnit>
    {
        public BuildingBuilder(IWorldData worldData, IAsyncRequestDispatcher requestDispatcher) : base(requestDispatcher)
        {
            if (worldData == null)
            {
                throw new ArgumentNullException("worldData");
            }

            WorldData = worldData;
        }

        public IWorldData WorldData { get; set; }

        class BuildingData : ChunkData
        {
            public BuildingData(BuildingBuilder parent, CubicHexCoord point) : base(parent, point)
            {
                this.parent = parent;
            }

            BuildingBuilder parent;
        }
    }
}
