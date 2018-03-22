using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示可读写游戏资源,包括 AssetBundle 资源;
    /// </summary>
    public class Modification : IDisposable
    {
        private bool isDisposed = false;
        public DirectoryContent BaseContent { get; private set; }
        public string DirectoryPath => BaseContent.DirectoryPath;
        public ModificationDescription Description { get; private set; }

        internal Modification(string directory, ModificationDescription description)
        {
            BaseContent = new DirectoryContent(directory);
            Description = description;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                BaseContent.Dispose();
                BaseContent = null;

                UnloadAll();

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

        #region AssetBundle

        private List<AssetBundleInfo> assetBundles;

        /// <summary>
        /// 获取到指定AssetBundle;
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        public AssetBundle GetAssetBundle(string name)
        {
            ThrowIfObjectDisposed();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            int index = FindAssetBundleInfoIndex(name);
            if (index >= 0)
            {
                var info = assetBundles[index];
                return info.AssetBundle;
            }
            else
            {
                throw new KeyNotFoundException(name);
            }
        }

        /// <summary>
        /// 尝试获取到指定AssetBundle;
        /// </summary>
        public bool TryGetAssetBundle(string name, out AssetBundle assetBundle)
        {
            ThrowIfObjectDisposed();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            int index = FindAssetBundleInfoIndex(name);
            if (index >= 0)
            {
                var info = assetBundles[index];
                assetBundle = info.AssetBundle;
                return true;
            }
            else
            {
                assetBundle = default(AssetBundle);
                return false;
            }
        }

        /// <summary>
        /// 尝试获取到对应 AssetBundle 信息 的下标,若不存在则返回 -1;
        /// </summary>
        private int FindAssetBundleInfoIndex(string name)
        {
            return assetBundles.FindIndex(item => item.Description.Name == name);
        }

        /// <summary>
        /// 读取所有 AssetBundle;
        /// </summary>
        /// <exception cref="AggregateException">当读取某个或多个 AssetBundle 失败时返回</exception>
        internal void LoadAllAssetBundles()
        {
            var assetBundleDescriptions = Description.AssetBundles;

            if (assetBundleDescriptions != null)
            {
                assetBundles = new List<AssetBundleInfo>(assetBundleDescriptions.Count);
                foreach (var description in assetBundleDescriptions)
                {
                    LoadAssetBundle(description);
                }
            }
            else
            {
                assetBundles = new List<AssetBundleInfo>(0);
            }
        }

        /// <summary>
        /// 异步读取所有 AssetBundle;
        /// </summary>
        /// <exception cref="AggregateException">当读取某个或多个 AssetBundle 失败时返回</exception>
        internal Task LoadAllAssetBundlesAsync()
        {
            var assetBundleDescriptions = Description.AssetBundles;
            if (assetBundleDescriptions != null)
            {
                assetBundles = new List<AssetBundleInfo>(assetBundleDescriptions.Count);
                List<Task> tasks = new List<Task>(assetBundleDescriptions.Count);

                foreach (var description in assetBundleDescriptions)
                {
                    var result = LoadAssetBundleAsync(description);
                    tasks.Add(result);
                }

                return Task.WhenAll(tasks);
            }
            else
            {
                assetBundles = new List<AssetBundleInfo>(0);
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 卸载所有 AssetBundle;
        /// </summary>
        private void UnloadAll(bool unloadAllLoadedObjects = true)
        {
            foreach (var assetBundleInfo in assetBundles)
            {
                assetBundleInfo.AssetBundle.Unload(unloadAllLoadedObjects);
            }
        }

        /// <summary>
        /// 添加 AssetBundle 到合集;
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private AssetBundleInfo LoadAssetBundle(AssetBundleDescription description)
        {
            if (string.IsNullOrWhiteSpace(description.Name))
                throw new ArgumentException(nameof(description.Name));

            AssetBundleInfo assetBundleInfo;
            int index = FindAssetBundleInfoIndex(description.Name);
            if (index >= 0)
            {
                throw new ArgumentException(string.Format("已经存在相同的名称的资源:{0}", description.Name));
            }
            else
            {
                AssetBundle assetBundle = LoadAssetBundle(description.RelativePath);
                assetBundleInfo = new AssetBundleInfo(description, assetBundle);
                assetBundles.Add(assetBundleInfo);
                return assetBundleInfo;
            }
        }

        /// <summary>
        /// 异步添加 AssetBundle 到合集;
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private Task<AssetBundleInfo> LoadAssetBundleAsync(AssetBundleDescription description)
        {
            if (string.IsNullOrWhiteSpace(description.Name))
                throw new ArgumentException(nameof(description.Name));

            int index = FindAssetBundleInfoIndex(description.Name);
            if (index >= 0)
            {
                throw new ArgumentException(string.Format("已经存在相同的名称的资源:{0}", description.Name));
            }
            else
            {
                return LoadAssetBundleAsync(description.RelativePath).ContinueWith(delegate(Task<AssetBundle> task)
                {
                    AssetBundleInfo assetBundleInfo = new AssetBundleInfo(description, task.Result);
                    assetBundles.Add(assetBundleInfo);
                    return assetBundleInfo;
                });
            }
        }

        /// <summary>
        /// 同步读取 AssetBundle;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private AssetBundle LoadAssetBundle(string relativePath)
        {
            string filePath = Path.Combine(DirectoryPath, relativePath);
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);
            if (assetBundle != null)
            {
                return assetBundle;
            }
            else
            {
                throw new IOException(string.Format("无法加载 AssetBundle[Path:{0}]", relativePath));
            }
        }

        /// <summary>
        /// 异步读取 AssetBundle,在Unity线程执行,并且在Unity线程回调;
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private Task<AssetBundle> LoadAssetBundleAsync(string relativePath)
        {
            string filePath = Path.Combine(DirectoryPath, relativePath);
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var request = AssetBundle.LoadFromFileAsync(filePath);
            return request.AsTask();
        }

        private struct AssetBundleInfo
        {
            public AssetBundleDescription Description { get; private set; }
            public AssetBundle AssetBundle { get; private set; }

            public AssetBundleInfo(AssetBundleDescription description, AssetBundle assetBundle)
            {
                Description = description;
                AssetBundle = assetBundle;
            }
        }

        #endregion


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
