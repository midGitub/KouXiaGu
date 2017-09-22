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

#region AssetBundle文件

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

#endregion

#region 游戏资源文件

        static string dataDirectoryPath = string.Empty;

        /// <summary>
        /// 存放配置文件的文件夹路径;
        /// </summary>
        public static string DataDirectoryPath
        {
            get { return string.IsNullOrEmpty(dataDirectoryPath) ? GetDataDirectoryPath() : dataDirectoryPath; }
        }

        static string GetDataDirectoryPath()
        {
            return dataDirectoryPath = Application.streamingAssetsPath;
        }

#endregion

#region 用户配置文件

        static string userConfigDirectoryPath = string.Empty;

        /// <summary>
        /// 用于存放用户配置文件的文件夹;
        /// </summary>
        public static string UserConfigDirectoryPath
        {
            get { return string.IsNullOrEmpty(userConfigDirectoryPath) ? GetUserConfigDirectoryPath() : userConfigDirectoryPath; }
        }

        static string GetUserConfigDirectoryPath()
        {
            return userConfigDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", Application.productName);
        }

#endregion

#region 用户存档文件

        static string archiveDirectoryPath = string.Empty;

        /// <summary>
        /// 存放存档的文件夹路径;
        /// </summary>
        public static string ArchivesDirectoryPath
        {
            get { return string.IsNullOrEmpty(archiveDirectoryPath) ? GetArchiveDirectoryPath() : archiveDirectoryPath; }
        }

        static string GetArchiveDirectoryPath()
        {
            return archiveDirectoryPath = Path.Combine(UserConfigDirectoryPath, "Saves");
        }

        #endregion

#region 临时文件

        static string tempDirectoryPath = string.Empty;

        /// <summary>
        /// 临时文件夹路径;
        /// </summary>
        public static string TempDirectoryPath
        {
            get { return string.IsNullOrEmpty(tempDirectoryPath) ? GetTempDirectoryPath() : tempDirectoryPath; }
        }

        static string GetTempDirectoryPath()
        {
            return tempDirectoryPath = Path.Combine(Application.streamingAssetsPath, "Temps");
        }

#endregion

        /// <summary>
        /// 在游戏开始时初始化;
        /// </summary>
        internal static void Initialize()
        {
            GetAssetBundleDirectoryPath();
            GetDataDirectoryPath();
            GetUserConfigDirectoryPath();
            GetArchiveDirectoryPath();
            GetTempDirectoryPath();
        }

        [ConsoleMethod("log_resource_path_info", "显示所有资源路径;", IsDeveloperMethod = true)]
        public static void LogInfoAll()
        {
            string str =
                "AssetBundleDirectoryPath : " + AssetBundleDirectoryPath +
                "\nDataDirectoryPath : " + DataDirectoryPath +
                "\nUserConfigDirectoryPath" + UserConfigDirectoryPath +
                "\nArchiveDirectoryPath : " + ArchivesDirectoryPath +
                "\nTempDirectoryPath : " + TempDirectoryPath;
            Debug.Log(str);
        }
    }
}
