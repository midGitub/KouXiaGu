using System;
using System.IO;
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

        const short MapSize = 1000;

        static readonly BlockMapRecord<LandformNode> terrainMap = new BlockMapRecord<LandformNode>(MapSize);


        public static IReadOnlyMap<CubicHexCoord, LandformNode> Map
        {
            get { return terrainMap; }
        }


        /// <summary>
        /// 保存地图到这个文件夹;
        /// </summary>
        public static void Save(string directoryPath)
        {
            throw new NotImplementedException();
        }

        public static void Load()
        {
            throw new NotImplementedException();
        }

    }

}
