using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.Concurrent;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 建筑创建器;
    /// </summary>
    public class BuildingBuilder : ChunkBuilder<CubicHexCoord, BuildingUnit>
    {
        public BuildingBuilder(IWorldData worldData, IRequestDispatcher requestDispatcher) : base(requestDispatcher)
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
