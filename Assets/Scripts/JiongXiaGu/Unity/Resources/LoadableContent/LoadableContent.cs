using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 抽象类 表示游戏可读资源;
    /// </summary>
    public abstract class LoadableContent : IDisposable
    {
        /// <summary>
        /// 提供公共存储 AssetBundle;
        /// </summary>
        private static readonly WeakReferenceObjectDictionary DefaultAssetBundleDictionary = new WeakReferenceObjectDictionary();

        /// <summary>
        /// 实例锁,对该类进行操作之前需要上锁;
        /// </summary>
        [Obsolete]
        public object AsyncLock { get; private set; } = new object();

        /// <summary>
        /// 是否已经销毁;
        /// </summary>
        public bool IsDisposed { get; private set; } = false;

        /// <summary>
        /// 在创建时使用的描述;
        /// </summary>
        public LoadableContentDescription Description { get; private set; }

        /// <summary>
        /// 最新的描述;
        /// </summary>
        public LoadableContentDescription? NewDescription { get; private set; }

        protected LoadableContent(LoadableContentDescription description)
        {
            Description = description;
        }

        protected LoadableContent(LoadableContentDescription description, WeakReferenceObjectDictionary assetBundleDictionary)
        {
            Description = description;
        }

        public virtual void Dispose()
        {
            IsDisposed = true;
        }

        protected void ThrowIfObjectDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }


        /// <summary>
        /// 根据资源更新实例信息,若无法更新则返回异常;
        /// </summary>
        public virtual void Update(LoadableContentFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            Description = factory.ReadDescription(this);
        }


        /// <summary>
        /// 枚举所有文件信息;
        /// </summary>
        public abstract IEnumerable<string> EnumerateFiles();

        /// <summary>
        /// 枚举所有文件;(参考 DirectoryInfo.EnumerateFiles 方法 (String, SearchOption))
        /// </summary>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (string entry in EnumerateFiles())
                    {
                        string fileName = PathHelper.GetFileName(entry);
                        if (PathHelper.IsMatch(fileName, searchPattern))
                        {
                            yield return entry;
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (string entry in EnumerateFiles())
                    {
                        if (PathHelper.IsFileName(entry))
                        {
                            string fileName = PathHelper.GetFileName(entry);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry;
                            }
                        }
                    }
                    break;

                default:
                    throw new IndexOutOfRangeException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
        }

        /// <summary>
        /// 枚举所有文件;(参考 DirectoryInfo.EnumerateFiles 方法 (String, SearchOption))
        /// </summary>
        /// <param name="directoryName">指定目录名称</param>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            string fileName;
            directoryName = PathHelper.Normalize(directoryName);

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (var entry in EnumerateFiles())
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry))
                        {
                            fileName = PathHelper.GetFileName(entry);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry;
                            }
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (var entry in EnumerateFiles())
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry))
                        {
                            fileName = PathHelper.GetFileName(entry);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry;
                            }
                        }
                    }
                    break;

                default:
                    throw new IndexOutOfRangeException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
        }

        /// <summary>
        /// 搜索指定路径,直到返回true停止,若未找到则返回 null;
        /// </summary>
        public string Find(Func<string, bool> func)
        {
            ThrowIfObjectDisposed();
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            foreach (var file in EnumerateFiles())
            {
                if (func.Invoke(file))
                {
                    return file;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取到只读的流,若未能获取到则返回 FileNotFound 异常;
        /// </summary>
        public abstract Stream GetInputStream(string relativePath);


        /// <summary>
        /// 在更改内容之前需要先调用;
        /// </summary>
        public abstract void BeginUpdate();

        /// <summary>
        /// 在更改内容之后,需要调用此方法来完成内容更新;
        /// </summary>
        public abstract void CommitUpdate();

        /// <summary>
        /// 添加到资源,若不存在该文件则加入到,若已经存在该文件,则更新其;
        /// </summary>
        public abstract void AddOrUpdate(string relativePath, Stream stream);

        /// <summary>
        /// 移除指定资源;
        /// </summary>
        public abstract bool Remove(string relativePath);

        /// <summary>
        /// 获取到输出流,若文件已经存在则返回该流,否则返回空的用于写的流;
        /// </summary>
        public abstract Stream GetOutStream(string relativePath);

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流;
        /// </summary>
        public abstract Stream CreateOutStream(string relativePath);



        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取或不存在则返回null;
        /// </summary>
        public static AssetBundle GetAssetBundlePublic(string key)
        {
            if (!IsAssetBundleKey(key))
                throw new ArgumentException(key);

            AssetBundle assetBundle;
            if (DefaultAssetBundleDictionary.TryGet(key, out assetBundle))
            {
                return assetBundle;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取或不存在则返回null;
        /// </summary>
        public AssetBundle GetAssetBundle(string assetBundleName)
        {
            ThrowIfObjectDisposed();
            if (string.IsNullOrWhiteSpace(assetBundleName))
                throw new ArgumentException(nameof(assetBundleName));

            string key = GetAssetBundleKey(assetBundleName);
            AssetBundle assetBundle;
            if (DefaultAssetBundleDictionary.TryGet(key, out assetBundle))
            {
                return assetBundle;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取,则读取并返回,若不存在 AssetBundle 或读取失败则返回异常;
        /// </summary>
        public AssetBundle GetOrLoadAssetBundle(string assetBundleName)
        {
            ThrowIfObjectDisposed();
            if (string.IsNullOrWhiteSpace(assetBundleName))
                throw new ArgumentException(nameof(assetBundleName));

            string key = GetAssetBundleKey(assetBundleName);
            AssetBundle assetBundle;
            if (DefaultAssetBundleDictionary.TryGet(key, out assetBundle))
            {
                return assetBundle;
            }
            else
            {
                return DefaultAssetBundleDictionary.Load(key, () => InternalAssetBundle(assetBundleName));
            }
        }

        /// <summary>
        /// 读取到 AssetBundle;
        /// </summary>
        private AssetBundle InternalAssetBundle(string name)
        {
            var assetBundleDescrs = Description.AssetBundles;
            if (assetBundleDescrs != null)
            {
                foreach (var descr in assetBundleDescrs)
                {
                    if (descr.Name == name)
                    {
                        using (var stream = GetInputStream(descr.RelativePath))
                        {
                            AssetBundle assetBundle = AssetBundle.LoadFromStream(stream);
                            if (assetBundle != null)
                            {
                                return assetBundle;
                            }
                            else
                            {
                                throw new IOException(string.Format("无法加载 AssetBundle[Name:{0}]", name));
                            }
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("未找到 AssetBundle[Name:{0}]的定义信息", name));
        }



        private const char AssetBundleKeyFristChar = '@';
        private const char AssetBundleKeySeparator = ':';

        /// <summary>
        /// 确认是否为 AssetBundle 字典结构的 key;
        /// </summary>
        public static bool IsAssetBundleKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(name);

            return name[0] == AssetBundleKeyFristChar;
        }

        /// <summary>
        /// 获取到存放 AssetBundle 字典结构的 key;
        /// </summary>
        public string GetAssetBundleKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(name);

            if (name[0] == AssetBundleKeyFristChar)
            {
                return name;
            }
            else
            {
                string key = InternalGetAssetBundleKey(name);
                return key;
            }
        }

        /// <summary>
        /// 转换为 AssetBundle 字典结构的 key;若已经为key,则返回true,需要转换则返回false;
        /// </summary>
        public bool TryGetAssetBundleKey(string name, out string key)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(name);

            if (name[0] == AssetBundleKeyFristChar)
            {
                key = name;
                return true;
            }
            else
            {
                key = InternalGetAssetBundleKey(name);
                return false;
            }
        }

        private string InternalGetAssetBundleKey(string assetBundleName)
        {
            string key = string.Concat(AssetBundleKeyFristChar, Description.ID, AssetBundleKeySeparator, assetBundleName);
            return key;
        }

        public static void Remove()
        {

        }
    }
}
