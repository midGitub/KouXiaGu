using System.Collections.Generic;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 将地形分为块的拓展;
    /// </summary>
    public static class ChunkPartitioner
    {

        #region 建筑物;

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
        /// 一个建筑块存在的建筑节点个数;
        /// </summary>
        public static int BuildingCount
        {
            get { return buildingOverlay.Length; }
        }

        /// <summary>
        /// 获取到地形块对应覆盖到的建筑物坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetBuilding(RectCoord chunkCoord)
        {
            CubicHexCoord chunkCenter = ChunkInfo.ChunkGrid.GetCenter(chunkCoord).GetTerrainCubic();
            return GetBuilding(chunkCenter);
        }

        /// <summary>
        /// 获取到地形块对应覆盖到的建筑物坐标;
        /// </summary>
        static IEnumerable<CubicHexCoord> GetBuilding(CubicHexCoord chunkCenter)
        {
            foreach (var item in buildingOverlay)
            {
                yield return chunkCenter + item;
            }
        }



        #endregion


        #region 地貌;

        /// <summary>
        /// (0, 0)对应覆盖的节点(依赖地图块大小);
        /// </summary>
        static readonly CubicHexCoord[] landformOverlay = new CubicHexCoord[]
            {
                new CubicHexCoord(-3, 3),
                new CubicHexCoord(-3, 2),
                new CubicHexCoord(-3, 1),
                new CubicHexCoord(-3, 0),

                new CubicHexCoord(-2, 3),
                new CubicHexCoord(-2, 2),
                new CubicHexCoord(-2, 1),
                new CubicHexCoord(-2, 0),
                new CubicHexCoord(-2, -1),

                new CubicHexCoord(-1, 3),
                new CubicHexCoord(-1, 2),
                new CubicHexCoord(-1, 1),
                new CubicHexCoord(-1, 0),
                new CubicHexCoord(-1, -1),
                new CubicHexCoord(-1, -2),

                new CubicHexCoord(0, 2),
                new CubicHexCoord(0, 1),
                new CubicHexCoord(0, 0),
                new CubicHexCoord(0, -1),
                new CubicHexCoord(0, -2),

                new CubicHexCoord(1, 2),
                new CubicHexCoord(1, 1),
                new CubicHexCoord(1, 0),
                new CubicHexCoord(1, -1),
                new CubicHexCoord(1, -2),
                new CubicHexCoord(1, -3),

                new CubicHexCoord(2, 1),
                new CubicHexCoord(2, 0),
                new CubicHexCoord(2, -1),
                new CubicHexCoord(2, -2),
                new CubicHexCoord(2, -3),

                new CubicHexCoord(3, 0),
                new CubicHexCoord(3, -1),
                new CubicHexCoord(3, -2),
                new CubicHexCoord(3, -3),
            };

        /// <summary>
        /// 一个地形块存在的节点个数;
        /// </summary>
        public static int LandformCount
        {
            get { return landformOverlay.Length; }
        }

        /// <summary>
        /// 获取到地形块对应覆盖到的地貌坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetLandform(RectCoord chunkCoord)
        {
            CubicHexCoord chunkCenter = ChunkInfo.ChunkGrid.GetCenter(chunkCoord).GetTerrainCubic();
            return GetLandform(chunkCenter);
        }

        /// <summary>
        /// 获取到地形块对应覆盖到的地貌坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetLandform(CubicHexCoord chunkCenter)
        {
            foreach (var item in landformOverlay)
            {
                yield return chunkCenter + item;
            }
        }

        #endregion


        #region 道路

        /// <summary>
        /// 获取到地形块对应覆盖到的建筑物坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetRoad(RectCoord chunkCoord)
        {
            CubicHexCoord chunkCenter = ChunkInfo.ChunkGrid.GetCenter(chunkCoord).GetTerrainCubic();
            return GetBuilding(chunkCenter);
        }

        /// <summary>
        /// 获取到地形块对应覆盖到的建筑物坐标;
        /// </summary>
        static IEnumerable<CubicHexCoord> GetRoad(CubicHexCoord chunkCenter)
        {
            foreach (var item in buildingOverlay)
            {
                yield return chunkCenter + item;
            }
        }

        #endregion

    }

}
