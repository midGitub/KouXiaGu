using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示可读写游戏资源;
    /// </summary>
    public class LoadableContent : BlockingContent, IDisposable
    {
        private bool isDisposed = false;
        private readonly ReaderWriterLockSlim assetBundleLock = new ReaderWriterLockSlim();
        private Lazy<List<KeyValuePair<string, AssetBundle>>> assetBundles = new Lazy<List<KeyValuePair<string, AssetBundle>>>();

        /// <summary>
        /// 在创建时使用的描述;
        /// </summary>
        public LoadableContentDescription OriginalDescription { get; private set; }
        private LoadableContentDescription? newDescription;

        /// <summary>
        /// 最新的描述;
        /// </summary>
        public LoadableContentDescription Description
        {
            get { return newDescription.HasValue ? newDescription.Value : OriginalDescription; }
            internal set { newDescription = value; }
        }

        public LoadableContent(Content content, LoadableContentDescription description) : base(content)
        {
            OriginalDescription = description;
        }

        /// <summary>
        /// 卸载所有资源;
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (!isDisposed)
            {
                UnloadAssetBundles();
                assetBundles = null;

                isDisposed = true;
            }
        }

        /// <summary>
        /// 卸载所有 AssetBundle;
        /// </summary>
        public void UnloadAssetBundles()
        {
            if (assetBundles.IsValueCreated && assetBundles.Value.Count > 0)
            {
                foreach (var assetBundle in assetBundles.Value)
                {
                    assetBundle.Value.Unload(false);
                }
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取则返回异常;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public AssetBundle GetAssetBundle(string assetBundleName)
        {
            ThrowIfObjectDisposed();

            using (assetBundleLock.ReadLock())
            {
                AssetBundle assetBundle;
                if (InternalTryGetAssetBundle(assetBundleName, out assetBundle))
                {
                    return assetBundle;
                }
                else
                {
                    throw new FileNotFoundException(string.Format("未找到AssetBundle[Name : {0}]", assetBundleName));
                }
            }
        }

        /// <summary>
        /// 读取到指定的 AssetBundle,若未找到则返回异常;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public AssetBundle GetOrLoadAssetBundle(string assetBundleName)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();

            AssetBundle assetBundle;
            if (InternalTryGetAssetBundle(assetBundleName, out assetBundle))
            {
                return assetBundle;
            }
            else
            {
                assetBundle = InternalLoadAssetBundle(assetBundleName);
                assetBundles.Value.Add(new KeyValuePair<string, AssetBundle>(assetBundleName, assetBundle));
                return assetBundle;
            }
        }

        /// <summary>
        /// 尝试获取到 AssetBundle ,若不存在则返回false,否则返回true;
        /// </summary>
        private bool InternalTryGetAssetBundle(string assetBundleName, out AssetBundle assetBundle)
        {
            if (assetBundles != null)
            {
                int index = assetBundles.Value.FindIndex(item => item.Key == assetBundleName);
                if (index >= 0)
                {
                    assetBundle = assetBundles.Value[index].Value;
                    return true;
                }
            }
            assetBundle = default(AssetBundle);
            return false;
        }

        /// <summary>
        /// 读取到 AssetBundle;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private AssetBundle InternalLoadAssetBundle(string name)
        {
            var assetBundleDescrs = Description.AssetBundles;
            if (assetBundleDescrs != null)
            {
                foreach (var descr in assetBundleDescrs)
                {
                    if (descr.Name == name)
                    {
                        var stream = GetInputStream(descr.RelativePath);
                        {
                            AssetBundle assetBundle = AssetBundle.LoadFromStream(stream);
                            if (assetBundle != null)
                            {
                                return assetBundle;
                            }
                            else
                            {
                                stream.Dispose();
                                throw new IOException(string.Format("无法加载 AssetBundle[Name:{0}]", name));
                            }
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("未找到 AssetBundle[Name:{0}]的定义信息", name));
        }
    }
}
