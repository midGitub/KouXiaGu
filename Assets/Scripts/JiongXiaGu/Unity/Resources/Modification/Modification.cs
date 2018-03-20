using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示可读写游戏资源;
    /// 关于 AssetBundle 读取方式,推荐在加载游戏时异步加载所有 AssetBundle;
    /// </summary>
    public class Modification : IDisposable
    {
        private bool isDisposed = false;
        public ModificationFactory Factory { get; private set; }
        public DirectoryContent BaseContent { get; private set; }
        public string DirectoryPath => BaseContent.DirectoryPath;
        public ModificationDescription Description { get; private set; }
        private Lazy<List<AssetBundleInfo>> lazyAssetBundles = new Lazy<List<AssetBundleInfo>>();

        internal Modification(ModificationFactory factory, string directory, ModificationDescription description)
        {
            Factory = factory;
            BaseContent = new DirectoryContent(directory);
            Description = description;
        }

        internal Modification(ModificationFactory factory, DirectoryContent content, ModificationDescription description)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            Factory = factory;
            BaseContent = content;
            Description = description;
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
        /// 卸载所有 AssetBundles;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void UnloadAllAssetBundles(bool unloadAllLoadedObjects)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();

            if (lazyAssetBundles.IsValueCreated)
            {
                foreach (var info in lazyAssetBundles.Value)
                {
                    Unload(info);
                }
                lazyAssetBundles.Value.Clear();
            }
        }

        /// <summary>
        /// 卸载 AssetBundle 资源;
        /// </summary>
        private void Unload(AssetBundleInfo info)
        {
            if (info.LoadTask.Status == TaskStatus.RanToCompletion)
            {
                info.LoadTask.Result.Unload(true);
            }
            else
            {
                info.LoadTask.ContinueWith(delegate (Task<AssetBundle> task)
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        task.Result.Unload(true);
                    }
                });
            }
        }

        /// <summary>
        /// 获取到已经读取完毕的 AssetBundle;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="KeyNotFoundException">未找到对应的已读 AssetBundle</exception>
        /// <exception cref="InvalidOperationException">AssetBundle 正在异步读取中,还未读取完毕</exception>
        /// <exception cref="IOException"></exception>
        public AssetBundle GetAssetBundle(string assetBundleName)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();
            if (string.IsNullOrWhiteSpace(assetBundleName))
                throw new ArgumentException(assetBundleName);

            int index = FindAssetBundleIndex(assetBundleName);
            if (index >= 0)
            {
                AssetBundleInfo info = lazyAssetBundles.Value[index];
                if (info.LoadTask.Status == TaskStatus.RanToCompletion)
                {
                    return info.LoadTask.Result;
                }
                else if (info.LoadTask.Status == TaskStatus.Faulted)
                {
                    lazyAssetBundles.Value.RemoveAt(index);
                    throw new KeyNotFoundException(assetBundleName);
                }
                else
                {
                    throw new InvalidOperationException("AssetBundle 正在异步读取中;");
                }
            }
            else
            {
                throw new KeyNotFoundException(assetBundleName);
            }
        }

        /// <summary>
        /// 获取到读取完毕或正在读取的 AssetBundle;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="KeyNotFoundException">未找到对应的 AssetBundle</exception>
        /// <exception cref="IOException"></exception>
        public Task<AssetBundle> GetAssetBundleAsync(string assetBundleName)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();
            if (string.IsNullOrWhiteSpace(assetBundleName))
                throw new ArgumentException(assetBundleName);

            int index = FindAssetBundleIndex(assetBundleName);
            if (index >= 0)
            {
                AssetBundleInfo info = lazyAssetBundles.Value[index];
                if (info.LoadTask.Status == TaskStatus.Faulted)
                {
                    lazyAssetBundles.Value.RemoveAt(index);
                    throw new KeyNotFoundException(assetBundleName);
                }
                else
                {
                    return info.LoadTask;
                }
            }
            else
            {
                throw new KeyNotFoundException(assetBundleName);
            }
        }


        /// <summary>
        /// 读取指定AssetBundle;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void LoadAllAssetBundles(IEnumerable<AssetBundleDescription> descriptions)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();
            if (Description.AssetBundles == null || Description.AssetBundles.Count == 0)
                return;

            foreach (var descr in descriptions)
            {
                LoadAssetBundle(descr);
            }
        }

        /// <summary>
        /// 读取到指定 AssetBundle;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException">传入描述格式错误 或 已经存在相同的名称的资源</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public AssetBundle LoadAssetBundle(AssetBundleDescription description)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();
            if (string.IsNullOrWhiteSpace(description.Name))
                throw new ArgumentException(nameof(description.Name));

            AssetBundleInfo assetBundlePack;
            int index = FindAssetBundleIndex(description.Name);
            if (index >= 0)
            {
                throw new ArgumentException(string.Format("已经存在相同的名称的资源:{0}", description.Name));
            }
            else
            {
                AssetBundle assetBundle = InternalLoadAssetBundle(description);
                assetBundlePack = new AssetBundleInfo(description, assetBundle);
                lazyAssetBundles.Value.Add(assetBundlePack);
                return assetBundlePack.LoadTask.Result;
            }
        }

        /// <summary>
        /// 同步读取 AssetBundle;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private AssetBundle InternalLoadAssetBundle(AssetBundleDescription description)
        {
            string filePath = Path.Combine(DirectoryPath, description.RelativePath);
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);
            if (assetBundle != null)
            {
                return assetBundle;
            }
            else
            {
                throw new IOException(string.Format("无法加载 AssetBundle[Name:{0}, Path:{1}]", description.Name, description.RelativePath));
            }
        }



        /// <summary>
        /// 异步读取所有AssetBundle;(仅Unity线程调用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public Task LoadAllAssetBundlesAsync(IEnumerable<AssetBundleDescription> descriptions)
        {
            ThrowIfObjectDisposed();
            UnityThread.ThrowIfNotUnityThread();
            if (Description.AssetBundles == null || Description.AssetBundles.Count == 0)
                return Task.CompletedTask;

            var tasks = new List<Task>();

            foreach (var descr in descriptions)
            {
                var loadTask = LoadAssetBundleAsync(descr);
                var assetBundleInfo = new AssetBundleInfo(descr, loadTask);
                lazyAssetBundles.Value.Add(assetBundleInfo);
                tasks.Add(loadTask);
            }

            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// 异步读取到 AssetBundle;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private Task<AssetBundle> LoadAssetBundleAsync(AssetBundleDescription description)
        {
            string filePath = Path.Combine(DirectoryPath, description.RelativePath);
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            return AssetBundle.LoadFromFileAsync(filePath).AsTask();
        }

        /// <summary>
        /// 尝试获取到对应 AssetBundle 信息 的下标,若不存在则返回 -1;
        /// </summary>
        private int FindAssetBundleIndex(string assetBundleName)
        {
            if (lazyAssetBundles.IsValueCreated)
            {
                int index = lazyAssetBundles.Value.FindIndex(item => item.Name == assetBundleName);
                return index;
            }
            return -1;
        }

        /// <summary>
        /// 资源包读取状态信息;
        /// </summary>
        private struct AssetBundleInfo
        {
            public AssetBundleDescription Description { get; set; }
            public Task<AssetBundle> LoadTask { get; set; }
            public string Name => Description.Name;

            public AssetBundleInfo(AssetBundleDescription description, AssetBundle assetBundle)
            {
                Description = description;
                LoadTask = Task.FromResult(assetBundle);
            }

            public AssetBundleInfo(AssetBundleDescription description, Task<AssetBundle> assetBundle)
            {
                Description = description;
                LoadTask = assetBundle;
            }
        }


        #region 资源获取

        /// <summary>
        /// 获取到资产;
        /// </summary>
        public T GetAsset<T>(AssetInfo info)
            where T : UnityEngine.Object
        {
            switch (info.From)
            {
                case AssetFrom.AssetBundle:
                    AssetBundle assetBundle = GetAssetBundle(info.Name);
                    return AssetFatroy.Default.Load<T>(assetBundle);

                case AssetFrom.Stream:
                    Stream stream = BaseContent.GetInputStream(info.Name);
                    return AssetFatroy.Default.Load<T>(stream);

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        #endregion

    }
}
