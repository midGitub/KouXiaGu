using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形资源存放的路径;
    /// </summary>
    public static class TerrainResPath
    {
        /// <summary>
        /// 资源存放的文件夹;
        /// </summary>
        public const string ResDirectory = "Terrain";

        /// <summary>
        /// 路径组合到地形资源文件夹下;
        /// </summary>
        public static string Combine(string path)
        {
            string resPath = Path.Combine(ResourcePath.ConfigurationDirectoryPath, ResDirectory);
            return Path.Combine(resPath, path);
        }

    }

}
