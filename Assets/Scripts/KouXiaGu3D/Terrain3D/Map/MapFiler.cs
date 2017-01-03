using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 游戏地图管理;
    /// </summary>
    public class MapFiler
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



        ///// <summary>
        ///// 获取到目录下的地图
        ///// </summary>
        //public static IEnumerable<MapFileInfo> GetMap(string directoryPath)
        //{

        //}

    }

}
