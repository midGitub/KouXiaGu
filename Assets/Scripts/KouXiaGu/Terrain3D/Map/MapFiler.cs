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
    public static class MapFiler
    {

        /// <summary>
        /// 预制地图数据文件;
        /// </summary>
        const string DATA_FILE_NAME = "Map";

        /// <summary>
        /// 读取地图;
        /// </summary>
        public static TerrainMap Read()
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            return TerrainMap.Read(filePath);
        }

        /// <summary>
        /// 保存地图;
        /// </summary>
        public static void Write(TerrainMap Map)
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            TerrainMap.Write(filePath, Map);
        }

    }

}
