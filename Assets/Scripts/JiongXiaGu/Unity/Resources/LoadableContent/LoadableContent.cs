using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示可读写游戏资源;
    /// 关于 AssetBundle 读取方式,推荐在加载游戏时异步加载所有 AssetBundle,在游戏运行中,若在游戏允许中还需要加载 AssetBundle,则使用 GetOrLoad 进行同步加载;
    /// </summary>
    public class LoadableContent : BlockingContent, IDisposable
    {
        /// <summary>
        /// 仅提供工厂类使用!用于指定最新描述使用;
        /// </summary>
        internal readonly object AsyncLock = new object();

        private bool isDisposed = false;
        private Lazy<List<AssetBundlePack>> assetBundles = new Lazy<List<AssetBundlePack>>();

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
        /// 读取所有AssetBundle,在读取过程中会锁本实例;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public void LoadAllAssetBundles(AssetBundleLoadOption options = AssetBundleLoadOption.Main)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();

            foreach (var descr in GetAssetBundleDescription(Description, options))
            {
                var pack = InternalLoadAssetBundle(descr);
                assetBundles.Value.Add(pack);
            }
        }

        /// <summary>
        /// 异步读取所有AssetBundle,在读取过程中会锁本实例;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public async Task LoadAllAssetBundlesAsync(AssetBundleLoadOption options = AssetBundleLoadOption.Main)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();

            foreach (var descr in GetAssetBundleDescription(Description, options))
            {
                var task = InternalLoadAssetBundleAsync(descr);
                await task;
                assetBundles.Value.Add(task.Result);
            }
        }

        /// <summary>
        /// 卸载所有 AssetBundles;(仅Unity线程调用)
        /// </summary>
        public void UnloadAssetBundlesAll()
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();

            if (assetBundles.IsValueCreated)
            {
                foreach (var assetBundle in assetBundles.Value)
                {
                    assetBundle.AssetBundle.Unload(false);
                    assetBundle.Stream.Dispose();
                }
                assetBundles.Value.Clear();

            }
        }

        private IEnumerable<AssetBundleDescription> GetAssetBundleDescription(LoadableContentDescription description, AssetBundleLoadOption options)
        {
            if ((options & AssetBundleLoadOption.Main) > 0 && description.MainAssetBundles != null)
            {
                foreach (var item in description.MainAssetBundles)
                {
                    yield return item;
                }
            }
            if ((options & AssetBundleLoadOption.Secondary) > 0 && description.SecondaryAssetBundles != null)
            {
                foreach (var item in description.SecondaryAssetBundles)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public AssetBundle GetAssetBundle(string assetBundleName, bool waitComplete = false)
        {
            ThrowIfObjectDisposed();

            AssetBundlePack assetBundlePack;
            if (InternalTryGetAssetBundle(assetBundleName, out assetBundlePack))
            {
                return assetBundlePack.AssetBundle;
            }
            else
            {
                throw new FileNotFoundException(string.Format("未找到AssetBundle[Name : {0}]", assetBundleName));
            }
        }

        /// <summary>
        /// 获取到指定 AssetBundle,若还未读取则异步读取到;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public AssetBundle GetOrLoadAssetBundle(string assetBundleName)
        {
            ThrowIfObjectDisposed();

            AssetBundlePack assetBundlePack;
            if (!InternalTryGetAssetBundle(assetBundleName, out assetBundlePack))
            {
                var description = InternalFindAssetBundleDescription(assetBundleName);
                assetBundlePack = InternalLoadAssetBundle(description);
                assetBundles.Value.Add(assetBundlePack);
            }
            return assetBundlePack.AssetBundle;
        }

        /// <summary>
        /// 尝试获取到 AssetBundle ,若不存在则返回false,否则返回true;
        /// </summary>
        private bool InternalTryGetAssetBundle(string assetBundleName, out AssetBundlePack assetBundle)
        {
            if (assetBundles != null)
            {
                int index = assetBundles.Value.FindIndex(item => item.Name == assetBundleName);
                if (index >= 0)
                {
                    assetBundle = assetBundles.Value[index];
                    return true;
                }
            }
            assetBundle = default(AssetBundlePack);
            return false;
        }

        /// <summary>
        /// 读取到 AssetBundle;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private AssetBundleDescription InternalFindAssetBundleDescription(string name)
        {
            var assetBundleDescrs = Description.MainAssetBundles;
            if (assetBundleDescrs != null)
            {
                foreach (var descr in assetBundleDescrs)
                {
                    if (descr.Name == name)
                    {
                        return descr;
                    }
                }
            }
            throw new ArgumentException(string.Format("未找到 AssetBundle[Name:{0}]的定义信息", name));
        }

        /// <summary>
        /// 读取到 AssetBundle;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private AssetBundlePack InternalLoadAssetBundle(AssetBundleDescription description)
        {
            var stream = GetInputStream(description.RelativePath);
            {
                AssetBundle assetBundle = AssetBundle.LoadFromStream(stream);
                if (assetBundle != null)
                {
                    var pack = new AssetBundlePack(description.Name, assetBundle, stream);
                    return pack;
                }
                else
                {
                    stream.Dispose();
                    throw new IOException(string.Format("无法加载 AssetBundle[Name:{0}]", description.Name));
                }
            }
        }

        /// <summary>
        /// 异步读取到 AssetBundle;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private Task<AssetBundlePack> InternalLoadAssetBundleAsync(AssetBundleDescription description)
        {
            var stream = GetInputStream(description.RelativePath);
            {
                var taskCompletionSource = new TaskCompletionSource<AssetBundlePack>();

                //Unity线程执行;
                UnityThread.RunInUnityThread(delegate ()
                {
                    AssetBundleCreateRequest request = AssetBundle.LoadFromStreamAsync(stream);
                    request.completed += delegate (AsyncOperation asyncOperation)
                    {
                        var assetBundle = request.assetBundle;
                        if (assetBundle == null)
                        {
                            stream.Dispose();
                            taskCompletionSource.SetException(new IOException("无法读取到 AssetBundle;"));
                        }
                        else
                        {
                            taskCompletionSource.SetResult(new AssetBundlePack(description.Name, assetBundle, stream));
                        }
                    };

                });

                return taskCompletionSource.Task;
            }
        }

        private struct AssetBundlePack
        {
            public string Name { get; private set; }
            public AssetBundle AssetBundle { get; private set; }
            public Stream Stream { get; private set; }

            public AssetBundlePack(string name, AssetBundle assetBundle, Stream stream)
            {
                Name = name;
                AssetBundle = assetBundle;
                Stream = stream;
            }
        }

        [Flags]
        public enum AssetBundleLoadOption
        {
            None = 0,
            Main = 1 << 0,
            Secondary = 1 << 1,
        }
    }
}
