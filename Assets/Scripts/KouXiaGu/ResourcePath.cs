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

        /// <summary>
        /// 存放 AssetBundle 的文件夹路径;
        /// </summary>
        public static string AssetBundleDirectoryPath { get; private set; }

        /// <summary>
        /// 存放配置文件的文件夹路径;
        /// </summary>
        public static string ConfigDirectoryPath { get; private set; }


        public static void Initialize()
        {
            AssetBundleDirectoryPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
            ConfigDirectoryPath = Application.streamingAssetsPath;
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
