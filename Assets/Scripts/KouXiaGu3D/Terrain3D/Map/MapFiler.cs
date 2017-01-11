using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    public static class MapFiler
    {

        /// <summary>
        /// 预制地图数据文件;
        /// </summary>
        const string DATA_FILE_NAME = "Map";

        /// <summary>
        /// 当前游戏使用的地图;
        /// </summary>
        public static TerrainMap Map { get; private set; }

        /// <summary>
        /// 地图是否加载?
        /// </summary>
        public static bool IsLoaded
        {
            get { return Map != null; }
        }

        public static void Read()
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            Map = TerrainMap.Read(filePath);
        }

        public static void Write()
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            TerrainMap.Write(filePath, Map);
        }

    }

}
