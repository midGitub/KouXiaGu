using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地图文件管理;
    /// </summary>
    public static class MapDataFiler
    {

        /// <summary>
        /// 预制地图数据文件;
        /// </summary>
        const string DATA_FILE_NAME = "Terrain";

        /// <summary>
        /// 读取地图;
        /// </summary>
        public static MapData Read()
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            return MapData.Read(filePath);
        }

        /// <summary>
        /// 保存地图;
        /// </summary>
        public static void Write(MapData Map)
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            MapData.Write(filePath, Map);
        }

    }

}
