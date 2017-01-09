using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地图管理;
    /// </summary>
    public static class TerrainMapFiler
    {

        /// <summary>
        /// 保存所有地图的文件路径;
        /// </summary>
        const string MAPS_DIRECTORY_NAME = "Maps";

        /// <summary>
        /// 预定义的的地图存放路径;
        /// </summary>
        public static string MapDirectory
        {
            get { return TerrainResPath.Combine(MAPS_DIRECTORY_NAME); }
        }


        /// <summary>
        /// 获取到所有地图;
        /// </summary>
        public static IEnumerable<TerrainMap> FindAll()
        {
            string[] paths = Directory.GetDirectories(MapDirectory);
            return TerrainMap.Find(paths);
        }

        /// <summary>
        /// 获取到Id相同的地图,若不存在则返回异常;
        /// </summary>
        public static TerrainMap Find(string id)
        {
            return FindAll().First(item => item.Description.Id == id);
        }

    }

}
