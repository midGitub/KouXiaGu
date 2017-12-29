using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示可读写资源;(线程安全)
    /// </summary>
    public class LoadableContent : IDisposable
    {
        private bool isDisposed = false;
        private ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 延迟实例化,若不存在AssetBundle则为Null;
        /// </summary>
        private List<KeyValuePair<string, AssetBundle>> assetBundles;

        /// <summary>
        /// 资源;
        /// </summary>
        public ConcurrentContent Content { get; private set; }

        /// <summary>
        /// 在创建时使用的描述;
        /// </summary>
        public LoadableContentDescription OriginalDescription { get; private set; }

        private LoadableContentDescription? description;

        /// <summary>
        /// 最新的描述;
        /// </summary>
        public LoadableContentDescription Description
        {
            get { return description.HasValue ? description.Value : OriginalDescription; }
            internal set { description = value; }
        }

        /// <summary>
        /// 创建该实例时指定的ID;
        /// </summary>
        public string ID => OriginalDescription.ID;

        public LoadableContent(ConcurrentContent content, LoadableContentDescription description)
        {
            Content = content;
            OriginalDescription = description;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                using (readerWriterLock.WriteLock())
                {
                    if (!isDisposed)
                    {
                        isDisposed = true;

                        //readerWriterLock.Dispose();
                        readerWriterLock = null;

                        UnloadAssetBundles();

                        Content.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 卸载所有 AssetBundle;
        /// </summary>
        private void UnloadAssetBundles()
        {
            if (assetBundles != null)
            {
                if (assetBundles.Count > 0)
                {
                    var _assetBundles = assetBundles;
                    assetBundles = null;
                    UnityThread.RunInUnityThread(delegate ()
                    {
                        foreach (var assetBundle in _assetBundles)
                        {
                            assetBundle.Value.Unload(false);
                        }
                    });
                }
                else
                {
                    assetBundles = null;
                }
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取则返回异常;
        /// </summary>
        public AssetBundle GetAssetBundle(string assetBundleName)
        {
            using (readerWriterLock.ReadLock())
            {
                ThrowIfObjectDisposed();

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
        public AssetBundle GetOrLoadAssetBundle(string assetBundleName)
        {
            using (readerWriterLock.UpgradeableReadLock())
            {
                ThrowIfObjectDisposed();

                AssetBundle assetBundle;
                if (InternalTryGetAssetBundle(assetBundleName, out assetBundle))
                {
                    return assetBundle;
                }
                else
                {
                    using (readerWriterLock.WriteLock())
                    {
                        assetBundle = InternalLoadAssetBundle(assetBundleName);
                        if (assetBundles == null)
                            assetBundles = new List<KeyValuePair<string, AssetBundle>>();

                        assetBundles.Add(new KeyValuePair<string, AssetBundle>(assetBundleName, assetBundle));
                        return assetBundle;
                    }
                }
            }
        }

        private bool InternalTryGetAssetBundle(string assetBundleName, out AssetBundle assetBundle)
        {
            if (assetBundles != null)
            {
                int index = assetBundles.FindIndex(item => item.Key == assetBundleName);
                if (index >= 0)
                {
                    assetBundle = assetBundles[index].Value;
                    return true;
                }
            }
            assetBundle = default(AssetBundle);
            return false;
        }

        /// <summary>
        /// 读取到 AssetBundle;
        /// </summary>
        private AssetBundle InternalLoadAssetBundle(string name)
        {
            var assetBundleDescrs = OriginalDescription.AssetBundles;
            if (assetBundleDescrs != null)
            {
                foreach (var descr in assetBundleDescrs)
                {
                    if (descr.Name == name)
                    {
                        var stream = Content.GetInputStream(descr.RelativePath);
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

        /// <summary>
        /// 若该实例已经被销毁,则返回异常;
        /// </summary>
        private void ThrowIfObjectDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }
    }
}
