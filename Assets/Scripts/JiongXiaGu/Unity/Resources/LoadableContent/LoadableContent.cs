using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 抽象类 表示可读资源;(线程安全)
    /// </summary>
    public abstract class LoadableContent : IDisposable
    {
        /// <summary>
        /// 实例锁,对该类进行操作之前需要上锁;
        /// </summary>
        public object AsyncLock { get; private set; } = new object();

        protected bool IsDisposed { get; private set; } = false;

        /// <summary>
        /// 描述信息;
        /// </summary>
        public LoadableContentDescription Description { get; protected set; }

        /// <summary>
        /// 所有已经加载的AssetBundle;
        /// </summary>
        private List<KeyValuePair<string, AssetBundle>> assetBundles = new List<KeyValuePair<string, AssetBundle>>();

        protected LoadableContent(LoadableContentDescription description)
        {
            Description = description;
        }


        /// <summary>
        /// 需要在Unity线程处理的线程;
        /// </summary>
        protected virtual void DisposeInUnityThread()
        {
            List<KeyValuePair<string, AssetBundle>> _assetBundles = assetBundles;
            foreach (var assetBundle in _assetBundles)
            {
                assetBundle.Value.Unload(true);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    UnityThread.RunInUnityThread(DisposeInUnityThread);
                }
                assetBundles = null;
                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
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
                    throw new ArgumentException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
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
                    throw new ArgumentException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
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
        /// 获取到对应的 AssetBundle,若还未读取或不存在则返回null;(仅在Unity线程调用,无需异步锁)
        /// </summary>
        public AssetBundle GetAssetBundle(string name)
        {
            ThrowIfObjectDisposed();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

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
        /// 获取到对应的 AssetBundle,若还未读取,则读取并返回,若不存在 AssetBundle 或读取失败则返回异常;(仅在Unity线程调用,无需异步锁)
        /// </summary>
        public AssetBundle GetOrLoadAssetBundle(string name)
        {
            ThrowIfObjectDisposed();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            int index = assetBundles.FindIndex(pair => pair.Key == name);
            if (index >= 0)
            {
                return assetBundles[index].Value;
            }
            else
            {
                return LoadAssetBundle(name);
            }
        }

        /// <summary>
        /// 读取到 AssetBundle;
        /// </summary>
        public AssetBundle LoadAssetBundle(string name)
        {
            ThrowIfObjectDisposed();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

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
                            if (assetBundles != null)
                            {
                                assetBundles.Add(new KeyValuePair<string, AssetBundle>(name, assetBundle));
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
    }
}
