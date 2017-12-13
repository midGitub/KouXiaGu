using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示进行分享的资源;(非线程安全)
    /// </summary>
    public sealed class SharedContent : IDisposable
    {
        private bool isDisposed = false;
        internal SharedContentSource Parent { get; private set; }
        public LoadableContent LoadableContent { get; private set; }
        private ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 延迟实例化,若不存在AssetBundle则为Null;
        /// </summary>
        internal List<KeyValuePair<string, AssetBundle>> AssetBundles { get; private set; }

        public string ID
        {
            get { return LoadableContent.Description.ID; }
        }

        internal SharedContent(LoadableContent loadableContent)
        {
            LoadableContent = loadableContent;
        }

        internal SharedContent(SharedContentSource parent, LoadableContent loadableContent)
        {
            Parent = parent;
            LoadableContent = loadableContent;
        }

        public void Dispose()
        {
            using (readerWriterLock.WriteLock())
            {
                if (!isDisposed)
                {
                    isDisposed = true;

                    Parent?.Remove(LoadableContent);
                    Parent = null;

                    //readerWriterLock.Dispose();
                    readerWriterLock = null;

                    UnloadAssetBundles();
                }
            }
        }

        private void UnloadAssetBundles()
        {
            if (AssetBundles != null)
            {
                if (AssetBundles.Count > 0)
                {
                    var _assetBundles = AssetBundles;
                    AssetBundles = null;
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
                    AssetBundles = null;
                }
            }
        }


        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取则返回异常;
        /// </summary>
        public AssetBundle GetAssetBundle(AssetPath path)
        {
            string contentID;
            string assetBundleName;

            if (path.GetRelativePath(out contentID, out assetBundleName))
            {
                return GetAssetBundle(assetBundleName);
            }
            else
            {
                if (Parent == null)
                {
                    throw new FileNotFoundException();
                }
                else
                {
                    var sharedContent = Parent.Find(contentID);
                    return sharedContent.GetAssetBundle(assetBundleName);
                }
            }
        }

        /// <summary>
        /// 读取到指定的 AssetBundle,若未找到则返回异常;
        /// </summary>
        public AssetBundle GetOrLoadAssetBundle(AssetPath path)
        {
            string contentID;
            string assetBundleName;

            if (path.GetRelativePath(out contentID, out assetBundleName))
            {
                return GetOrLoadAssetBundle(assetBundleName);
            }
            else
            {
                if (Parent == null)
                {
                    throw new FileNotFoundException();
                }
                else
                {
                    var sharedContent = Parent.Find(contentID);
                    return sharedContent.GetOrLoadAssetBundle(assetBundleName);
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
                    assetBundle = InternalLoadAssetBundle(assetBundleName);
                    using (readerWriterLock.WriteLock())
                    {
                        if (AssetBundles == null)
                            AssetBundles = new List<KeyValuePair<string, AssetBundle>>();

                        AssetBundles.Add(new KeyValuePair<string, AssetBundle>(assetBundleName, assetBundle));
                    }
                    return assetBundle;
                }
            }
        }

        private bool InternalTryGetAssetBundle(string assetBundleName, out AssetBundle assetBundle)
        {
            if (AssetBundles != null)
            {
                int index = AssetBundles.FindIndex(item => item.Key == assetBundleName);
                if (index >= 0)
                {
                    assetBundle = AssetBundles[index].Value;
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
            var assetBundleDescrs = LoadableContent.Description.AssetBundles;
            if (assetBundleDescrs != null)
            {
                foreach (var descr in assetBundleDescrs)
                {
                    if (descr.Name == name)
                    {
                        var stream = LoadableContent.ConcurrentGetInputStream(descr.RelativePath);
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
