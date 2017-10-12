using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// AssetBundle 引用,在不需要使用时需要 Dispose();
    /// </summary>
    public interface IAssetBundleReference : IDisposable
    {
        AssetBundle AssetBundle { get; }
    }

    /// <summary>
    /// 游戏使用的 AssetBundle 读取;
    /// </summary>
    public static class AssetBundleReader
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
        /// 从文件读取到资源包;
        /// </summary>
        public static AssetBundle Load(string assetBundleName)
        {
            string filePath = GetAssetBundleFilePath(assetBundleName);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);
            return assetBundle;
        }

        /// <summary>
        /// 获取到自带资源包目录路径;
        /// </summary>
        public static string GetAssetBundlesDirectory()
        {
            string directory = Path.Combine(Resource.CoreDataDirectory, AssetBundlesDirectoryName);
            return directory;
        }

        /// <summary>
        /// 获取到资源包路径;
        /// </summary>
        public static string GetAssetBundleFilePath(string assetBundleName)
        {
            string fileName = assetBundleName + AssetBundleFileExtension;
            string filePath = Path.Combine(GetAssetBundlesDirectory(), fileName);
            return filePath;
        }
    }

    /// <summary>
    /// AssetBundle 实例池(仅Unity线程调用);
    /// </summary>
    public static class AssetBundlePool
    {

        /// <summary>
        /// 游戏使用到的AssetBundle存放目录;
        /// </summary>
        [PathDefinition(ResourceTypes.DataDirectory, "游戏使用到的AssetBundle存放目录")]
        internal const string AssetBundlesDirectory = "AssetBundles";

        private static readonly Dictionary<string, AssetBundleCounter> assetBundles = new Dictionary<string, AssetBundleCounter>();

        internal static void Initialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加一个 AssetBundle 到池;
        /// </summary>
        internal static void AddAssetBundle(AssetBundle assetBundle)
        {
            AssetBundleCounter counter = new AssetBundleCounter(assetBundle);
            assetBundles.Add(counter.Name, counter);
        }

        /// <summary>
        /// 获取到指定 AssetBundle 实例;
        /// </summary>
        public static IAssetBundleReference GetByName(string assetBundleName)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //static AssetBundle Load(string assetBundleName)
        //{
        //    AssetBundle.LoadFromFile()
        //}

        //private static string GetAssetBundlePath(string assetBundleName)
        //{
            
        //}

        /// <summary>
        /// 获取到游戏使用到的AssetBundle存放目录;
        /// </summary>
        private static string GetAssetBundlesDirectory()
        {
            string directory = Path.Combine(Application.streamingAssetsPath, AssetBundlesDirectory);
            return directory;
        }

        /// <summary>
        /// AssetBundle 引用计数器;
        /// </summary>
        sealed class AssetBundleCounter
        {
            public int ReferenceCount { get; private set; }

            /// <summary>
            /// AssetBundle 资源实例;
            /// </summary>
            public AssetBundle AssetBundle { get; private set; }

            /// <summary>
            /// AssetBundle 名;
            /// </summary>
            public string Name
            {
                get { return AssetBundle.name; }
            }

            /// <summary>
            /// 当前此 AssetBundle 是否不再被需要?
            /// </summary>
            public bool IsUseless
            {
                get { return ReferenceCount == 0; }
            }

            public AssetBundleCounter(AssetBundle assetBundle)
            {
                if (assetBundle == null)
                    throw new ArgumentNullException(nameof(assetBundle));

                AssetBundle = assetBundle;
            }

            /// <summary>
            /// 获取到资源包的引用;
            /// </summary>
            public IAssetBundleReference GetReference()
            {
                return new AssetBundleReference(this, AssetBundle);
            }

            /// <summary>
            /// AssetBundle 引用;
            /// </summary>
            sealed class AssetBundleReference : IAssetBundleReference
            {
                private bool isDisposed = false;
                public AssetBundleCounter Counter { get; private set; }
                public AssetBundle AssetBundle { get; private set; }

                public AssetBundleReference(AssetBundleCounter counter, AssetBundle assetBundle)
                {
                    Counter.ReferenceCount++;
                    AssetBundle = assetBundle;
                }

                ~AssetBundleReference()
                {
                    Dispose(false);
                }

                private void Dispose(bool disposing)
                {
                    if (!isDisposed)
                    {
                        Counter.ReferenceCount--;
                        isDisposed = true;
                    }
                }

                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}
