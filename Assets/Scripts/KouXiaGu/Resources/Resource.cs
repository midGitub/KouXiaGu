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
            get { return assetBundleDirectoryPath; }
            private set { assetBundleDirectoryPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles"); }
        }


        static string configDirectoryPath = string.Empty;

        /// <summary>
        /// 存放配置文件的文件夹路径;
        /// </summary>
        public static string ConfigDirectoryPath
        {
            get { return configDirectoryPath; }
            private set { configDirectoryPath = Application.streamingAssetsPath; }
        }


        static string archiveDirectoryPath = string.Empty;

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static string ArchiveDirectoryPath
        {
            get { return archiveDirectoryPath; }
            private set { archiveDirectoryPath = Path.Combine(Application.persistentDataPath, "Saves"); }
        }


        /// <summary>
        /// 用于测试使用的临时文件夹路径;
        /// </summary>
        static string tempDirectoryPath = string.Empty;

        public static string TempDirectoryPath
        {
            get { return tempDirectoryPath; }
            private set { tempDirectoryPath = Path.Combine(Application.streamingAssetsPath, "Temps"); }
        }


        /// <summary>
        /// 在游戏开始时初始化;
        /// </summary>
        public static void Initialize()
        {
            AssetBundleDirectoryPath = AssetBundleDirectoryPath;
            ConfigDirectoryPath = ConfigDirectoryPath;
            ArchiveDirectoryPath = ArchiveDirectoryPath;
            TempDirectoryPath = TempDirectoryPath;
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
