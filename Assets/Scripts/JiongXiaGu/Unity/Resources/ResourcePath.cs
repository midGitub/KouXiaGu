using JiongXiaGu.Unity.GameConsoles;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源路径定义;
    /// </summary>
    public static class ResourcePath
    {
        private static DirectoryInfo coreDirectory;
        private static DirectoryInfo userConfigDirectory;
        private static DirectoryInfo archivesDirectory;
        private static DirectoryInfo modDirectory;
        private static DirectoryInfo dlcDirectory;

        /// <summary>
        /// 存放核心数据和配置文件的文件夹;
        /// </summary>
        public static DirectoryInfo CoreDirectory
        {
            get { return coreDirectory ?? (coreDirectory = GetCoreDirectoryInfo()); }
        }

        /// <summary>
        /// 存放用户配置的文件夹;
        /// </summary>
        public static DirectoryInfo UserConfigDirectory
        {
            get { return userConfigDirectory ?? (userConfigDirectory = GetUserConfigDirectoryInfo()); }
        }

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static DirectoryInfo ArchivesDirectory
        {
            get { return archivesDirectory ?? (archivesDirectory = GetArchivesDirectoryInfo()); }
        }

        /// <summary>
        /// 存放模组的文件夹;
        /// </summary>
        public static DirectoryInfo ModDirectory
        {
            get { return modDirectory ?? (modDirectory = GetModsDirectoryInfo()); }
        }

        /// <summary>
        /// 存放拓展内容的文件夹;
        /// </summary>
        public static DirectoryInfo DlcDirectory
        {
            get { return dlcDirectory ?? (dlcDirectory = GetDlcDirectoryInfo()); }
        }

        /// <summary>
        /// 在游戏开始时初始化;
        /// </summary>
        internal static void Initialize()
        {
            if (coreDirectory == null)
                coreDirectory = GetCoreDirectoryInfo();

            if (userConfigDirectory == null)
                userConfigDirectory = GetUserConfigDirectoryInfo();

            if (archivesDirectory == null)
                archivesDirectory = GetArchivesDirectoryInfo();

            if (modDirectory == null)
                modDirectory = GetModsDirectoryInfo();

            if (dlcDirectory == null)
                dlcDirectory = GetDlcDirectoryInfo();
        }

        /// <summary>
        /// 获取存放核心数据和配置文件的文件夹;
        /// </summary>
        public static DirectoryInfo GetCoreDirectoryInfo()
        {
            string directory = Application.streamingAssetsPath;
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.ThrowIfDirectoryNotExisted();
            return directoryInfo;
        }

        /// <summary>
        /// 获取到存放用户配置的文件夹;
        /// </summary>
        public static DirectoryInfo GetUserConfigDirectoryInfo()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", Application.productName);
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }

        /// <summary>
        /// 获取到存放存档的文件夹路径;
        /// </summary>
        public static DirectoryInfo GetArchivesDirectoryInfo()
        {
            var userConfigDirectory = UserConfigDirectory;
            string directory = Path.Combine(userConfigDirectory.FullName, "Saves");
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }

        /// <summary>
        /// 获取到存放模组的文件夹;
        /// </summary>
        public static DirectoryInfo GetModsDirectoryInfo()
        {
            var userConfigDirectory = UserConfigDirectory;
            string directory = Path.Combine(userConfigDirectory.FullName, "Mods");
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }

        /// <summary>
        /// 获取到存放拓展内容的文件夹;
        /// </summary>
        public static DirectoryInfo GetDlcDirectoryInfo()
        {
            var coreDirectory = CoreDirectory;
            string directory = Path.Combine(coreDirectory.FullName, "DLC");
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }
    }
}
