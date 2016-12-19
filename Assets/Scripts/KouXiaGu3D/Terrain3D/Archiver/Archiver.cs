using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    public static class Archiver
    {
        /// <summary>
        /// 保存到的存档目录;
        /// </summary>
        public const string MAP_ARCHIVED_DIRECTORY_NAME = "Maps";


        /// <summary>
        /// 连接到地形信息统一存放的的文件夹路径;
        /// </summary>
        /// <param name="archiveDirectory">存档文件夹路径</param>
        /// <param name="fileName">需要连结在其后的文件名</param>
        /// <returns></returns>
        public static string CombineDirectory(string archiveDirectory, string fileName)
        {
            archiveDirectory = Path.Combine(archiveDirectory, Archiver.MAP_ARCHIVED_DIRECTORY_NAME);
            string filePath = Path.Combine(archiveDirectory, fileName);
            return filePath;
        }

        /// <summary>
        /// 创建地形信息统一存放的的文件夹;
        /// </summary>
        /// <param name="archiveDirectory">存档文件夹路径</param>
        /// <returns></returns>
        public static DirectoryInfo CreateDirectory(string archiveDirectory)
        {
            archiveDirectory = Path.Combine(archiveDirectory, Archiver.MAP_ARCHIVED_DIRECTORY_NAME);
            return Directory.CreateDirectory(archiveDirectory);
        }

        /// <summary>
        /// 若不存在则,创建地形信息统一存放的的文件夹,并且连结路径到文件名;
        /// </summary>
        public static string CreateDirectory(string archiveDirectory, string fileName)
        {
            archiveDirectory = Path.Combine(archiveDirectory, Archiver.MAP_ARCHIVED_DIRECTORY_NAME);

            if (!Directory.Exists(archiveDirectory))
                Directory.CreateDirectory(archiveDirectory);

            return Path.Combine(archiveDirectory, fileName);
        }

    }

}
