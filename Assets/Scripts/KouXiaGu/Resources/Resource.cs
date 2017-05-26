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
    public static class Resource
    {

        static string assetBundleDirectoryPath = string.Empty;

        /// <summary>
        /// 存放 AssetBundle 的文件夹路径;
        /// </summary>
        public static string AssetBundleDirectoryPath
        {
            get
            {
                return assetBundleDirectoryPath != string.Empty ? assetBundleDirectoryPath :
                  (assetBundleDirectoryPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles"));
            }
            private set { assetBundleDirectoryPath = value; }
        }

        static string configDirectoryPath = string.Empty;

        /// <summary>
        /// 存放配置文件的文件夹路径;
        /// </summary>
        public static string ConfigDirectoryPath
        {
            get { return configDirectoryPath != string.Empty ? configDirectoryPath : (configDirectoryPath = Application.streamingAssetsPath); }
            private set { configDirectoryPath = value; }
        }

        /// <summary>
        /// 在游戏开始时初始化;
        /// </summary>
        public static void Initialize()
        {
            AssetBundleDirectoryPath = AssetBundleDirectoryPath;
            ConfigDirectoryPath = ConfigDirectoryPath;
        }
    }

    public abstract class FilePath : IFilePath
    {
        public abstract string FileName { get; }

        public IEnumerable<string> FindFile()
        {
            yield return Path.Combine(Resource.ConfigDirectoryPath, FileName);
        }
    }


    public interface IFilePath
    {
        /// <summary>
        /// 获取到所有文件路径;
        /// </summary>
        IEnumerable<string> FindFile();
    }
}
