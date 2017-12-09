using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 抽象类 表示可读资源;
    /// 每一个资源只允许拥有一个AssetBundle;
    /// </summary>
    public abstract class LoadableContent
    {
        /// <summary>
        /// 实例锁,对该类进行操作之前需要上锁;
        /// </summary>
        public object AsyncLock { get; private set; } = new object();

        /// <summary>
        /// 描述信息;
        /// </summary>
        public LoadableContentDescription Description { get; protected set; }

        /// <summary>
        /// 所有已经加载的AssetBundle;
        /// </summary>
        private readonly List<KeyValuePair<string, AssetBundle>> assetBundles = new List<KeyValuePair<string, AssetBundle>>();

        protected LoadableContent(LoadableContentDescription description)
        {
            Description = description;
        }

        /// <summary>
        /// 根据资源更新实例信息,若无法更新则返回异常;
        /// </summary>
        public virtual void Update(LoadableContentFactory factory)
        {
            Description = factory.ReadDescription(this);
        }

        /// <summary>
        /// 卸载此资源,仅在Unity线程调用;
        /// </summary>
        public virtual void Unload()
        {
            XiaGu.ThrowIfNotUnityThread();

            foreach (var assetBundle in assetBundles)
            {
                assetBundle.Value.Unload(true);
            }
        }

        /// <summary>
        /// 枚举所有文件信息;
        /// </summary>
        public abstract IEnumerable<ILoadableEntry> EnumerateFiles();

        /// <summary>
        /// 枚举所有文件;(参考 DirectoryInfo.EnumerateFiles 方法 (String, SearchOption))
        /// </summary>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<ILoadableEntry> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (ILoadableEntry entry in EnumerateFiles())
                    {
                        string fileName = PathHelper.GetFileName(entry.RelativePath);
                        if (PathHelper.IsMatch(fileName, searchPattern))
                        {
                            yield return entry;
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (ILoadableEntry entry in EnumerateFiles())
                    {
                        if (PathHelper.IsFileName(entry.RelativePath))
                        {
                            string fileName = PathHelper.GetFileName(entry.RelativePath);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry;
                            }
                        }
                    }
                    break;

                default:
                    throw new ArgumentException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
        }

        /// <summary>
        /// 枚举所有文件;(参考 DirectoryInfo.EnumerateFiles 方法 (String, SearchOption))
        /// </summary>
        /// <param name="directoryName">指定目录名称</param>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<ILoadableEntry> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            string fileName;
            directoryName = PathHelper.Normalize(directoryName);

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (ILoadableEntry entry1 in EnumerateFiles())
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry1.RelativePath))
                        {
                            fileName = PathHelper.GetFileName(entry1.RelativePath);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry1;
                            }
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (ILoadableEntry entry2 in EnumerateFiles())
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry2.RelativePath))
                        {
                            fileName = PathHelper.GetFileName(entry2.RelativePath);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry2;
                            }
                        }
                    }
                    break;

                default:
                    throw new ArgumentException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
        }

        /// <summary>
        /// 获取到对应入口,若不存在则返回null;
        /// </summary>
        public virtual ILoadableEntry GetEntry(string relativePath)
        {
            foreach (var entry in EnumerateFiles())
            {
                if (entry.RelativePath == relativePath)
                {
                    return entry;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取到只读的流;
        /// </summary>
        public virtual Stream GetInputStream(string relativePath)
        {
            ILoadableEntry entry = GetEntry(relativePath);
            if (entry != null)
            {
                return GetInputStream(entry);
            }
            else
            {
                throw new FileNotFoundException(relativePath);
            }
        }

        /// <summary>
        /// 获取到只读的流;(支持多线程操作)
        /// </summary>
        public abstract Stream GetInputStream(ILoadableEntry entry);


        /// <summary>
        /// 在更改内容之前需要先调用;
        /// </summary>
        public abstract void BeginUpdate();

        /// <summary>
        /// 在更改内容之后,需要调用此方法来完成内容更新;
        /// </summary>
        public abstract void CommitUpdate();

        /// <summary>
        /// 添加流到资源,若不存在该文件则加入到,若已经存在该文件,则更新其;
        /// </summary>
        public abstract void AddOrUpdate(string relativePath, Stream stream);

        /// <summary>
        /// 移除指定资源;
        /// </summary>
        public abstract bool Remove(string relativePath);

        /// <summary>
        /// 移除指定资源;
        /// </summary>
        public abstract void Remove(ILoadableEntry entry);

        /// <summary>
        /// 获取到输出流,若文件已经存在则返回该流,否则返回空的用于写的流;
        /// </summary>
        public abstract Stream GetOutStream(string relativePath);

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流;
        /// </summary>
        public abstract Stream CreateOutStream(string relativePath);

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取或不存在则返回null;(仅在Unity线程调用,无需异步锁)
        /// </summary>
        public AssetBundle GetAssetBundle(string name)
        {
            XiaGu.ThrowIfNotUnityThread();

            int index = assetBundles.FindIndex(pair => pair.Key == name);
            if (index >= 0)
            {
                return assetBundles[index].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取,则读取并返回,若不存在则返回null;(仅在Unity线程调用,无需异步锁)
        /// </summary>
        public AssetBundle GetOrLoadAssetBundle(string name)
        {
            XiaGu.ThrowIfNotUnityThread();

            int index = assetBundles.FindIndex(pair => pair.Key == name);
            if (index >= 0)
            {
                return assetBundles[index].Value;
            }
            else
            {
                return InternalLoadAssetBundle(name);
            }
        }

        /// <summary>
        /// 读取到 AssetBundle,若未找到,则返回null;
        /// </summary>
        private AssetBundle InternalLoadAssetBundle(string name)
        {
            foreach (var descr in GetAssetBundlesDescription())
            {
                if (descr.Name == name)
                {
                    AssetBundle assetBundle = AssetBundle.LoadFromFile(descr.Path);
                    if (assetBundles != null)
                    {
                        assetBundles.Add(new KeyValuePair<string, AssetBundle>(name, assetBundle));
                        return assetBundle;
                    }
                    else
                    {
                        Debug.Log("无法加载AssetBundle;");
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取到 AssetBundles 名称和完整路径;
        /// </summary>
        protected abstract IEnumerable<AssetBundleDescription> GetAssetBundlesDescription();
    }
}
