using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 对游戏资源文件路径进行定义,需要手动初始化;
    /// </summary>
    public static class ResourcePath
    {

        static string assetBundleDirectoryPath = string.Empty;
        static string configDirectoryPath = string.Empty;

        /// <summary>
        /// 存放 AssetBundle 的文件夹路径;
        /// </summary>
        public static string AssetBundleDirectoryPath
        {
            get { return assetBundleDirectoryPath != string.Empty ? assetBundleDirectoryPath :
                    (assetBundleDirectoryPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles")); }
            private set { assetBundleDirectoryPath = value; }
        }

        /// <summary>
        /// 存放配置文件的文件夹路径;
        /// </summary>
        public static string ConfigDirectoryPath
        {
            get { return configDirectoryPath != string.Empty ? configDirectoryPath : (configDirectoryPath = Application.streamingAssetsPath); }
            private set { configDirectoryPath = value; }
        }


        public static void Initialize()
        {
            AssetBundleDirectoryPath = AssetBundleDirectoryPath;
            ConfigDirectoryPath = ConfigDirectoryPath;
        }

        /// <summary>
        ///连结到 AssetBundle 文件的存放路径;
        /// </summary>
        public static string CombineAssetBundle(string assetBundleName)
        {
            return Path.Combine(AssetBundleDirectoryPath, assetBundleName);
        }

        /// <summary>
        /// 连结到 配置 文件的存放路径;
        /// </summary>
        public static string CombineConfiguration(string path)
        {
            return Path.Combine(ConfigDirectoryPath, path);
        }

    }

}
