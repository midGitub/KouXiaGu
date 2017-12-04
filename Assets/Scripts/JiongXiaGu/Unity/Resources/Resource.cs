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
    public static class Resource
    {
        /// <summary>
        /// 自定义的 AssetBundle 后缀名,因为Unity构建的 AssetBundle 没有后缀名;
        /// </summary>
        public const string AssetBundleExtension = ".assetbundle";

        /// <summary>
        /// 存放核心数据和配置文件的文件夹;
        /// </summary>
        public static string CoreDirectory { get; private set; }

        /// <summary>
        /// 存放用户配置的文件夹;
        /// </summary>
        public static string UserConfigDirectory { get; private set; }

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static string ArchiveDirectory { get; private set; }

        /// <summary>
        /// 存放模组的文件夹;
        /// </summary>
        public static string ModDirectory { get; private set; }

        /// <summary>
        /// 存放拓展内容的文件夹;
        /// </summary>
        public static string DlcDirectory { get; private set; }

        /// <summary>
        /// 缓存目录;
        /// </summary>
        public static string CacheDirectory { get; private set; }

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
            CacheDirectory = GetCacheDirectory();
        }

        /// <summary>
        /// 获取存放核心数据和配置文件的文件夹;
        /// </summary>
        public static string GetCoreDirectoryInfo()
        {
            string directory = Path.Combine(Application.streamingAssetsPath, "Data");
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException();
            }
            return directory;
        }

        /// <summary>
        /// 获取到存放用户配置的文件夹;
        /// </summary>
        public static string GetUserConfigDirectoryInfo()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", Application.productName);
            Directory.CreateDirectory(directory);
            return directory;
        }

        /// <summary>
        /// 获取到存放存档的文件夹路径;
        /// </summary>
        public static string GetArchiveDirectoryInfo()
        {
            var userConfigDirectory = UserConfigDirectory;

            if (string.IsNullOrEmpty(userConfigDirectory))
            {
                userConfigDirectory = GetUserConfigDirectoryInfo();
            }

            string directory = Path.Combine(userConfigDirectory, "Save");
            Directory.CreateDirectory(directory);
            return directory;
        }

        /// <summary>
        /// 获取到存放模组的文件夹;
        /// </summary>
        public static string GetModsDirectoryInfo()
        {
            var userConfigDirectory = UserConfigDirectory;

            if (string.IsNullOrEmpty(userConfigDirectory))
            {
                userConfigDirectory = GetUserConfigDirectoryInfo();
            }

            string directory = Path.Combine(userConfigDirectory, "MOD");
            Directory.CreateDirectory(directory);
            return directory;
        }

        /// <summary>
        /// 获取到存放拓展内容的文件夹;
        /// </summary>
        public static string GetDlcDirectoryInfo()
        {
            string directory = Path.Combine(Application.streamingAssetsPath, "DLC");
            Directory.CreateDirectory(directory);
            return directory;
        }

        /// <summary>
        /// 缓存目录;
        /// </summary>
        public static string GetCacheDirectory()
        {
            string directory = Application.temporaryCachePath;
            Directory.CreateDirectory(directory);
            return directory;
        }
    }
}
