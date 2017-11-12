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
        /// <summary>
        /// 存放核心数据和配置文件的文件夹;
        /// </summary>
        public static DirectoryInfo CoreDirectory { get; private set; }

        /// <summary>
        /// 存放用户配置的文件夹;
        /// </summary>
        public static DirectoryInfo UserConfigDirectory { get; private set; }

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static DirectoryInfo ArchiveDirectory { get; private set; }

        /// <summary>
        /// 存放模组的文件夹;
        /// </summary>
        public static DirectoryInfo ModDirectory { get; private set; }

        /// <summary>
        /// 存放拓展内容的文件夹;
        /// </summary>
        public static DirectoryInfo DlcDirectory { get; private set; }

        /// <summary>
        /// 初始化路径信息(仅在Unity线程调用);
        /// </summary>
        internal static void Initialize()
        {
            CoreDirectory = GetCoreDirectoryInfo();
            UserConfigDirectory = GetUserConfigDirectoryInfo();
            ArchiveDirectory = GetArchiveDirectoryInfo();
            ModDirectory = GetModsDirectoryInfo();
            DlcDirectory = GetDlcDirectoryInfo();
        }

        /// <summary>
        /// 获取存放核心数据和配置文件的文件夹;
        /// </summary>
        public static DirectoryInfo GetCoreDirectoryInfo()
        {
            string directory = Path.Combine(Application.streamingAssetsPath, "Data");
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
        public static DirectoryInfo GetArchiveDirectoryInfo()
        {
            var userConfigDirectory = UserConfigDirectory;
            string directory = Path.Combine(userConfigDirectory.FullName, "Save");
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
            string directory = Path.Combine(userConfigDirectory.FullName, "MOD");
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }

        /// <summary>
        /// 获取到存放拓展内容的文件夹;
        /// </summary>
        public static DirectoryInfo GetDlcDirectoryInfo()
        {
            string directory = Path.Combine(Application.streamingAssetsPath, "DLC");
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            directoryInfo.Create();
            return directoryInfo;
        }
    }
}
