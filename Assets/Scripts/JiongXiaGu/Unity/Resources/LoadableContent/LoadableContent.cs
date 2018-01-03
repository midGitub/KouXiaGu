using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示可读写游戏资源;
    /// </summary>
    public class LoadableContent : PackagedContent, IDisposable
    {
        private bool isDisposed = false;
        private Lazy<List<KeyValuePair<string, AssetBundle>>> assetBundles;

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

        public LoadableContent(Content content, LoadableContentDescription description) : base(content)
        {
            OriginalDescription = description;
            assetBundles = new Lazy<List<KeyValuePair<string, AssetBundle>>>(() => new List<KeyValuePair<string, AssetBundle>>());
        }

        public override void Dispose()
        {
            base.Dispose();
            if (!isDisposed)
            {
                UnloadAssetBundles();
                isDisposed = true;
            }
        }

        /// <summary>
        /// 卸载所有 AssetBundle;
        /// </summary>
        private async void UnloadAssetBundles()
        {
            if (assetBundles.IsValueCreated)
            {
                if (assetBundles.Value.Count > 0)
                {
                    var _assetBundles = assetBundles.Value;
                    assetBundles = null;
                    await UnityThread.RunInUnityThread(delegate ()
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

        /// <summary>
        /// 读取到指定的 AssetBundle,若未找到则返回异常;
        /// </summary>
        public AssetBundle GetOrLoadAssetBundle(string assetBundleName)
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
                assetBundles.Value.Add(new KeyValuePair<string, AssetBundle>(assetBundleName, assetBundle));
                return assetBundle;
            }
        }

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
        private AssetBundle InternalLoadAssetBundle(string name)
        {
            var assetBundleDescrs = OriginalDescription.AssetBundles;
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
