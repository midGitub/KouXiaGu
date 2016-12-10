using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 地形地图保存和提供;
    /// </summary>
    public static class TerrainMap
    {

        static readonly Map2D<CubicHexCoord, LandformNode> terrainMap = new Map2D<CubicHexCoord, LandformNode>();


        public static IReadOnlyMap2D<CubicHexCoord, LandformNode> Map
        {
            get { return terrainMap; }
        }







    }

}
