using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 地形地图保存和提供;
    /// 采用分块保存的方式;
    /// </summary>
    public static class TerrainMap
    {

        static readonly Map2D<CubicHexCoord, LandformNode> terrainMap = new Map2D<CubicHexCoord, LandformNode>();


        public static IReadOnlyMap2D<CubicHexCoord, LandformNode> Map
        {
            get { return terrainMap; }
        }


        /// <summary>
        /// 保存地图;
        /// </summary>
        public static void Save()
        {
            throw new NotImplementedException();
        }

        public static void Load()
        {
            throw new NotImplementedException();
        }

    }

}
