using JiongXiaGu.Unity.GameConsoles;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 对游戏资源文件路径进行定义,需要手动初始化;
    /// </summary>
    [ConsoleMethodClass]
    public static class Resource
    {
        private static ModInfo coreDirectoryInfo;
        private static DirectoryInfo userConfigDirectoryInfo;
        private static DirectoryInfo archivesDirectoryInfo;

        /// <summary>
        /// 核心数据和配置文件的文件夹;
        /// </summary>
        public static ModInfo CoreDirectoryInfo
        {
            get { return coreDirectoryInfo ?? (coreDirectoryInfo = GetCoreDirectoryInfo()); }
        }

        /// <summary>
        /// 用于存放用户配置文件的文件夹;
        /// </summary>
        public static DirectoryInfo UserConfigDirectoryInfo
        {
            get { return userConfigDirectoryInfo ?? (userConfigDirectoryInfo = GetUserConfigDirectoryInfo()); }
        }

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static DirectoryInfo ArchivesDirectoryInfo
        {
            get { return archivesDirectoryInfo ?? (archivesDirectoryInfo = GetArchivesDirectoryInfo()); }
        }

        /// <summary>
        /// 主要数据和配置文件的文件夹;
        /// </summary>
        public static string CoreDirectory
        {
            get { return CoreDirectoryInfo.DirectoryInfo.FullName; }
        }

        /// <summary>
        /// 用于存放用户配置文件的文件夹;
        /// </summary>
        public static string UserConfigDirectory
        {
            get { return UserConfigDirectoryInfo.FullName; }
        }

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static string ArchivesDirectory
        {
            get { return ArchivesDirectoryInfo.FullName; }
        }

        /// <summary>
        /// 在游戏开始时初始化;
        /// </summary>
        internal static void Initialize()
        {
            if (coreDirectoryInfo == null)
                coreDirectoryInfo = GetCoreDirectoryInfo();

            if (userConfigDirectoryInfo == null)
                userConfigDirectoryInfo = GetUserConfigDirectoryInfo();

            if (archivesDirectoryInfo == null)
                archivesDirectoryInfo = GetArchivesDirectoryInfo();
        }

        /// <summary>
        /// 获取到核心数据和配置文件的文件夹;
        /// </summary>
        public static ModInfo GetCoreDirectoryInfo()
        {
            string directory = Application.streamingAssetsPath;
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.ThrowIfDirectoryNotExisted();
            var dataDirectoryInfo = new ModInfo(directoryInfo);
            return dataDirectoryInfo;
        }

        public static DirectoryInfo GetUserConfigDirectoryInfo()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", Application.productName);
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }

        public static DirectoryInfo GetArchivesDirectoryInfo()
        {
            var userConfigDirectory = UserConfigDirectory;
            if (userConfigDirectory == null)
            {
                userConfigDirectory = GetUserConfigDirectoryInfo().FullName;
            }

            string directory = Path.Combine(userConfigDirectory, "Saves");
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }


        [ConsoleMethod("log_resource_path_info", Message = "显示所有资源路径")]
        public static void LogInfoAll()
        {
            string str =
                "\nDataDirectoryPath : " + CoreDirectory +
                "\nUserConfigDirectoryPath" + UserConfigDirectory +
                "\nArchiveDirectoryPath : " + ArchivesDirectory;
            Debug.Log(str);
        }
    }
}
