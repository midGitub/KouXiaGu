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
    class BuildingBuilder : ChunkBuilder<CubicHexCoord, BuildingUnit>
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

        /// <summary>
        /// 创建
        /// </summary>
        public void Create(RectCoord point)
        {
            throw new NotImplementedException();
        }

        public void Update(RectCoord point)
        {
            throw new NotImplementedException();
        }

        public void Destroy(RectCoord point)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (0, 0)对应覆盖的节点(依赖地图块大小);
        /// </summary>
        static readonly CubicHexCoord[] buildingOverlay = new CubicHexCoord[]
            {
                new CubicHexCoord(-2, 2),
                new CubicHexCoord(-2, 1),
                new CubicHexCoord(-2, 0),

                new CubicHexCoord(-1, 2),
                new CubicHexCoord(-1, 1),
                new CubicHexCoord(-1, 0),

                new CubicHexCoord(0, 1),
                new CubicHexCoord(0, 0),
                new CubicHexCoord(0, -1),

                new CubicHexCoord(1, 1),
                new CubicHexCoord(1, 0),
                new CubicHexCoord(1, -1),
            };

        /// <summary>
        /// 获取到地形块对应覆盖到的建筑物坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetOverlayPoints(RectCoord chunkCoord)
        {
            CubicHexCoord chunkCenter = LandformChunkInfo.ChunkGrid.GetCenter(chunkCoord).GetTerrainCubic();
            foreach (var item in buildingOverlay)
            {
                yield return chunkCenter + item;
            }
        }



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
