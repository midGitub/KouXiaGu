using KouXiaGu.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 对游戏资源文件路径进行定义,需要手动初始化;
    /// </summary>
    [ConsoleMethodsClass]
    public static class Resource
    {

        static string assetBundleDirectoryPath = string.Empty;

        /// <summary>
        /// 存放 AssetBundle 的文件夹路径;
        /// </summary>
        public static string AssetBundleDirectoryPath
        {
            get { return string.IsNullOrEmpty(assetBundleDirectoryPath) ? GetAssetBundleDirectoryPath() : assetBundleDirectoryPath; }
        }

        static string GetAssetBundleDirectoryPath()
        {
            return assetBundleDirectoryPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
        }

        public static string GetAssetBundleFullPath(this SingleConfigFileName assetBundleName)
        {
            return Path.Combine(AssetBundleDirectoryPath, assetBundleName);
        }



        static string configDirectoryPath = string.Empty;

        /// <summary>
        /// 存放配置文件的文件夹路径;
        /// </summary>
        public static string ConfigDirectoryPath
        {
            get { return string.IsNullOrEmpty(configDirectoryPath) ? GetConfigDirectoryPath() : configDirectoryPath; }
        }

        static string GetConfigDirectoryPath()
        {
            return configDirectoryPath = Application.streamingAssetsPath;
        }



        static string archiveDirectoryPath = string.Empty;

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static string ArchiveDirectoryPath
        {
            get { return string.IsNullOrEmpty(archiveDirectoryPath) ? GetArchiveDirectoryPath() : archiveDirectoryPath; }
        }

        static string GetArchiveDirectoryPath()
        {
            return archiveDirectoryPath = Path.Combine(Application.persistentDataPath, "Saves");
        }



        /// <summary>
        /// 用于测试使用的临时文件夹路径;
        /// </summary>
        static string tempDirectoryPath = string.Empty;

        public static string TempDirectoryPath
        {
            get { return string.IsNullOrEmpty(tempDirectoryPath) ? GetTempDirectoryPath() : tempDirectoryPath; }
        }

        static string GetTempDirectoryPath()
        {
            return tempDirectoryPath = Path.Combine(Application.streamingAssetsPath, "Temps");
        }



        /// <summary>
        /// 在游戏开始时初始化;
        /// </summary>
        public static void Initialize()
        {
            GetAssetBundleDirectoryPath();
            GetConfigDirectoryPath();
            GetArchiveDirectoryPath();
            GetTempDirectoryPath();
        }

        [ConsoleMethod("log_resource_path_info", "显示所有资源路径;", IsDeveloperMethod = true)]
        public static void LogInfoAll()
        {
            string str =
                "AssetBundleDirectoryPath : " + AssetBundleDirectoryPath +
                "\nConfigDirectoryPath : " + ConfigDirectoryPath +
                "\nArchiveDirectoryPath : " + ArchiveDirectoryPath +
                "\nTempDirectoryPath : " + TempDirectoryPath;
            Debug.Log(str);
        }
    }
}
