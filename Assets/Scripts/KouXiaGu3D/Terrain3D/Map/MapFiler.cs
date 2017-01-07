using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 游戏地图管理;
    /// </summary>
    public static class MapFiler
    {

        /// <summary>
        /// 保存所有地图的文件路径;
        /// </summary>
        const string MAPS_DIRECTORY_NAME = "Maps";

        /// <summary>
        /// 预定义的的地图存放路径;
        /// </summary>
        public static string PredefinedDirectory
        {
            get { return TerrainResPath.Combine(MAPS_DIRECTORY_NAME); }
        }

        /// <summary>
        /// 指定其它的地图文件夹路径;
        /// </summary>
        public static IEnumerable<string> MapDirectorys { get; set; }

        /// <summary>
        /// 获取到所有的地图;
        /// </summary>
        public static IEnumerable<MapFileInfo> GetMaps()
        {
            IEnumerable<MapFileInfo> mapFiles = GetMaps(PredefinedDirectory);

            if (MapDirectorys != null)
            {
                foreach (var mapPath in MapDirectorys)
                {
                    mapFiles.Append(GetMaps(mapPath));
                }
            }

            return mapFiles;
        }

        /// <summary>
        /// 获取到目录下的地图;
        /// </summary>
        public static IEnumerable<MapFileInfo> GetMaps(string directoryPath)
        {
            var paths = Directory.GetDirectories(directoryPath);
            MapFileInfo mapFile;

            foreach (var mapPath in paths)
            {
                if(TryLoadMap(mapPath, out mapFile))
                    yield return mapFile;
            }
        }

        /// <summary>
        /// 尝试获取到这个目录下的地图;
        /// </summary>
        public static bool TryLoadMap(string directoryPath, out MapFileInfo mapFile)
        {
            try
            {
                mapFile = MapFileInfo.Load(directoryPath);
                return true;
            }
            catch (FileNotFoundException)
            {
                Debug.LogWarning("无法获取到描述文件 ;地图目录 :" + directoryPath + " ; 跳过此文件夹;");
            }

            mapFile = default(MapFileInfo);
            return false;
        }

    }

}
