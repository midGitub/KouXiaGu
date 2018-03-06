using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示可读写游戏资源;
    /// 关于 AssetBundle 读取方式,推荐在加载游戏时异步加载所有 AssetBundle,在游戏运行中,若在游戏允许中还需要加载 AssetBundle,则使用 GetOrLoad 进行同步加载;
    /// </summary>
    public class Modification : IDisposable
    {
        private const AssetBundleLoadOption defaultLoadOption = AssetBundleLoadOption.Important;
        private bool isDisposed = false;
        public ModificationFactory Factory { get; private set; }
        public DirectoryContent BaseContent { get; private set; }
        public string DirectoryPath => BaseContent.DirectoryPath;
        private Lazy<List<AssetBundleInfo>> assetBundles = new Lazy<List<AssetBundleInfo>>();
        private Lazy<List<ModificationSubresource>> subresource = new Lazy<List<ModificationSubresource>>();

        /// <summary>
        /// 在创建时使用的描述;
        /// </summary>
        public ModificationDescription OriginalDescription { get; private set; }
        private ModificationDescription? newDescription;

        /// <summary>
        /// 最新的描述;
        /// </summary>
        public ModificationDescription Description
        {
            get { return newDescription.HasValue ? newDescription.Value : OriginalDescription; }
            internal set { newDescription = value; }
        }

        internal Modification(ModificationFactory factory, string directory, ModificationDescription description)
        {
            Factory = factory;
            BaseContent = new DirectoryContent(directory);
            OriginalDescription = description;
        }

        internal Modification(ModificationFactory factory, DirectoryContent content, ModificationDescription description)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            Factory = factory;
            BaseContent = content;
            OriginalDescription = description;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                BaseContent.Dispose();
                BaseContent = null;

                UnloadAllAssetBundles(true);

                Factory.OnDispose(this);

                isDisposed = true;
            }
        }

        private void ThrowIfObjectDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }


        /// <summary>
        /// 读取指定AssetBundle;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void LoadAllAssetBundles(AssetBundleLoadOption options = defaultLoadOption)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();
            if (Description.AssetBundles == null || Description.AssetBundles.Length == 0)
                return;

            foreach (var descr in SelectAssetBundleDescription(Description.AssetBundles, options))
            {
                var info = LoadAssetBundle(descr);
                assetBundles.Value.Add(info);
            }
        }

        /// <summary>
        /// 卸载所有 AssetBundles;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void UnloadAllAssetBundles(bool unloadAllLoadedObjects)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();

            if (assetBundles.IsValueCreated)
            {
                foreach (var assetBundle in assetBundles.Value)
                {
                    assetBundle.AssetBundle.Unload(unloadAllLoadedObjects);
                    assetBundle.Stream.Dispose();
                }
                assetBundles.Value.Clear();
            }
        }

        /// <summary>
        /// 筛选对应资源包描述;
        /// </summary>
        private IEnumerable<AssetBundleDescription> SelectAssetBundleDescription(AssetBundleDescription[] assetBundleDescriptions, AssetBundleLoadOption options)
        {
            if (assetBundleDescriptions == null)
            {
                return EmptyCollection<AssetBundleDescription>.Default;
            }
            else
            {
                bool includeImportant = (options & AssetBundleLoadOption.Important) > 0;
                bool includeNotImportant = (options & AssetBundleLoadOption.NotImportant) > 0;

                if (includeImportant || includeNotImportant)
                {
                    return assetBundleDescriptions.Where(delegate (AssetBundleDescription description)
                    {
                        return !string.IsNullOrWhiteSpace(description.Name) &&
                        !string.IsNullOrWhiteSpace(description.RelativePath) &&
                        (includeNotImportant ||
                        description.IsImportant);
                    });
                }
                else
                {
                    return EmptyCollection<AssetBundleDescription>.Default;
                }
            }
        }


        /// <summary>
        /// 获取到已经读取完毕的 AssetBundle;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="FileNotFoundException">未找到对应的已读 AssetBundle</exception>
        /// <exception cref="InvalidOperationException">AssetBundle 正在异步读取中,还未读取完毕</exception>
        public AssetBundle GetAssetBundle(string assetBundleName, bool waitComplete = false)
        {
            ThrowIfObjectDisposed();

            AssetBundleInfo assetBundlePack;
            if (TryGetAssetBundle(assetBundleName, out assetBundlePack))
            {
                if (assetBundlePack.IsLoadComplete)
                {
                    return assetBundlePack.AssetBundle;
                }
                else
                {
                    throw new InvalidOperationException("请求的AssetBundle正在异步读取中;");
                }
            }
            else
            {
                throw new FileNotFoundException(string.Format("未找到AssetBundle[Name : {0}]", assetBundleName));
            }
        }

        /// <summary>
        /// 获取到指定 AssetBundle,若还未读取则同步读取到;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException">未找到指定 AssetBundle 描述,传入名称格式错误</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public AssetBundle GetOrLoadAssetBundle(string assetBundleName)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();
            if (string.IsNullOrWhiteSpace(assetBundleName))
                throw new ArgumentException(nameof(assetBundleName));

            AssetBundleInfo assetBundlePack;
            if (!TryGetAssetBundle(assetBundleName, out assetBundlePack))
            {
                var description = FindAssetBundleDescription(assetBundleName);
                assetBundlePack = LoadAssetBundle(description);
                assetBundles.Value.Add(assetBundlePack);
            }
            return assetBundlePack.AssetBundle;
        }

        /// <summary>
        /// 尝试获取到 AssetBundle ,若不存在则返回false,否则返回true;
        /// </summary>
        private bool TryGetAssetBundle(string assetBundleName, out AssetBundleInfo assetBundle)
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
            assetBundle = default(AssetBundleInfo);
            return false;
        }

        /// <summary>
        /// 读取到 AssetBundle;
        /// </summary>
        /// <exception cref="ArgumentException">未找到指定 AssetBundle 描述</exception>
        private AssetBundleDescription FindAssetBundleDescription(string name)
        {
            var assetBundleDescrs = Description.AssetBundles;
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
        /// 同步读取到 AssetBundle;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private AssetBundleInfo LoadAssetBundle(AssetBundleDescription description)
        {
            var stream = BaseContent.GetInputStream(description.RelativePath);
            {
                AssetBundle assetBundle = AssetBundle.LoadFromStream(stream);
                if (assetBundle != null)
                {
                    var pack = new AssetBundleInfo(description.Name, assetBundle, stream);
                    return pack;
                }
                else
                {
                    stream.Dispose();
                    throw new IOException(string.Format("无法加载 AssetBundle[Name:{0}, Path:{1}]", description.Name, description.RelativePath));
                }
            }
        }


        #region Async

        /// <summary>
        /// 异步读取所有AssetBundle,在读取过程中会锁本实例;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public Task LoadAllAssetBundlesAsync(AssetBundleLoadOption options = defaultLoadOption)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();
            if (Description.AssetBundles == null || Description.AssetBundles.Length == 0)
                return Task.CompletedTask;

            var tasks = new List<Task>();

            foreach (var descr in SelectAssetBundleDescription(Description.AssetBundles, options))
            {
                var assetBundleInfo = new AssetBundleInfo(descr.Name);
                assetBundles.Value.Add(assetBundleInfo);

                var task = LoadAssetBundleInfoAsync(assetBundleInfo, descr);
                tasks.Add(task);
            }

            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// 异步读取到 AssetBundle;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private Task LoadAssetBundleInfoAsync(AssetBundleInfo assetBundleInfo, AssetBundleDescription description)
        {
            var stream = assetBundleInfo.Stream = BaseContent.GetInputStream(description.RelativePath);
            return AssetBundleHelper.LoadAssetBundleAsync(stream).ContinueWith(delegate(Task<AssetBundle> task)
            {
                assetBundleInfo.AssetBundle = task.Result;
            });
        }

        /// <summary>
        /// 获取到指定 AssetBundle,若还未读取则异步读取到;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException">未找到指定 AssetBundle 描述</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public async Task<AssetBundle> GetOrLoadAssetBundleAsync(string assetBundleName)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();
            if (string.IsNullOrWhiteSpace(assetBundleName))
                throw new ArgumentException(nameof(assetBundleName));

            AssetBundleInfo assetBundleInfo;
            if (!TryGetAssetBundle(assetBundleName, out assetBundleInfo))
            {
                var description = FindAssetBundleDescription(assetBundleName);
                assetBundleInfo = new AssetBundleInfo(assetBundleName);
                assetBundles.Value.Add(assetBundleInfo);

                var stream = assetBundleInfo.Stream = BaseContent.GetInputStream(description.RelativePath);
                assetBundleInfo.AssetBundle = await AssetBundleHelper.LoadAssetBundleAsync(stream);
            }
            return assetBundleInfo.AssetBundle;
        }

        #endregion


        /// <summary>
        /// 资源包读取状态信息;
        /// </summary>
        private class AssetBundleInfo
        {
            public string Name;
            public AssetBundle AssetBundle;
            public Stream Stream;
            public bool IsLoadComplete => AssetBundle != null;

            public AssetBundleInfo(string name)
            {
                Name = name;
                AssetBundle = null;
                Stream = null;
            }

            public AssetBundleInfo(string name, AssetBundle assetBundle, Stream stream)
            {
                Name = name;
                AssetBundle = assetBundle;
                Stream = stream;
            }
        }
    }
}
