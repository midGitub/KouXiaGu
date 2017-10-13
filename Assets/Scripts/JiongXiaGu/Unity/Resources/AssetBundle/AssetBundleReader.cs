using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 游戏使用的 AssetBundle 读取;
    /// </summary>
    public class AssetBundleReader
    {
        /// <summary>
        /// 游戏使用到的AssetBundle存放目录;
        /// </summary>
        [PathDefinition(ResourceTypes.DataDirectory, "游戏使用到的AssetBundle存放目录")]
        internal const string AssetBundlesDirectoryName = "AssetBundles";

        /// <summary>
        /// AssetBundle 文件拓展名;
        /// </summary>
        static readonly string AssetBundleFileExtension = String.Empty;

        /// <summary>
        /// 搜索所有可用的 AssetBundle;
        /// </summary>
        public static IEnumerable<AssetBundleFileInfo> SearchAll(ModInfo dataInfo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从文件读取到资源包;
        /// </summary>
        public static AssetBundle Load(ModInfo dataInfo, string assetBundleName)
        {
            string filePath = GetAssetBundleFile(dataInfo, assetBundleName);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);
            return assetBundle;
        }

        /// <summary>
        /// 获取到资源包路径;
        /// </summary>
        public static string GetAssetBundleFile(ModInfo dataInfo, string assetBundleName)
        {
            string assetBundlesDirectory = GetAssetBundlesDirectory(dataInfo);
            string fileName = assetBundleName + AssetBundleFileExtension;
            string assetBundleFile = Path.Combine(assetBundlesDirectory, fileName);
            return assetBundleFile;
        }

        /// <summary>
        /// 获取到资源包目录路径;
        /// </summary>
        public static string GetAssetBundlesDirectory(ModInfo dataInfo)
        {
            string directory = Path.Combine(dataInfo.Directory, AssetBundlesDirectoryName);
            return directory;
        }
    }
}
