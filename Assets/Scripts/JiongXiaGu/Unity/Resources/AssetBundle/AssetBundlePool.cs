using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// AssetBundle 实例池(大部分方法仅由Unity线程调用);
    /// </summary>
    public class AssetBundlePool : IDisposable
    {
        private bool isDisposed = false;
        private Dictionary<string, AssetBundle> assetBundles;

        public AssetBundlePool()
        {
            assetBundles = new Dictionary<string, AssetBundle>();
        }

        ~AssetBundlePool()
        {
            Dispose(false);
        }

        /// <summary>
        /// 获取到对应对应资源包;
        /// </summary>
        public AssetBundle Get(string dataName, string assetBundleName)
        {
            var key = GetAssetBundleHashKey(dataName, assetBundleName);
            AssetBundle assetBundle = assetBundles[key];
            return assetBundle;
        }

        /// <summary>
        /// 获取到对应对应资源包;
        /// </summary>
        public AssetBundle GetOrLoad(ModInfo dataInfo, string assetBundleName)
        {
            var key = GetAssetBundleHashKey(dataInfo.Name, assetBundleName);
            AssetBundle assetBundle;
            if (!assetBundles.TryGetValue(key, out assetBundle))
            {
                assetBundle = AssetBundleReader.Load(dataInfo, assetBundleName);
                assetBundles.Add(key, assetBundle);
            }
            return assetBundle;
        }

        /// <summary>
        /// 获取到 assetBundles 字典关键词;
        /// </summary>
        private string GetAssetBundleHashKey(string dataName, string assetBundleName)
        {
            return string.Concat(dataName, ":", assetBundleName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                foreach (var assetBundle in assetBundles.Values)
                {
                    assetBundle.Unload(true);
                }

                assetBundles = null;
                isDisposed = true;
            }
        }
    }
}
