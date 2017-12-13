using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 抽象类 表示可读写资源;(部分方法线程安全)
    /// </summary>
    public abstract class LoadableContent : ILoadableContent, IDisposable
    {
        private readonly object asyncLock = new object();

        /// <summary>
        /// 是否已经销毁;
        /// </summary>
        private volatile bool isDisposed;

        /// <summary>
        /// 在创建时使用的描述;
        /// </summary>
        public LoadableContentDescription Description { get; private set; }

        /// <summary>
        /// 最新的描述;
        /// </summary>
        public LoadableContentDescription? NewDescription { get; private set; }

        /// <summary>
        /// 分享资源;
        /// </summary>
        internal SharedContent SharedContent { get; private set; }

        protected LoadableContent(LoadableContentDescription description)
        {
            Description = description;
            SharedContent = new SharedContent(this);
        }

        protected LoadableContent(LoadableContentDescription description, SharedContentSource sharedContentSource)
        {
            Description = description;
            SharedContent = sharedContentSource.Add(this);
        }

        public void Dispose()
        {
            lock (asyncLock)
            {
                if (!isDisposed)
                {
                    isDisposed = true;
                    Dispose(true);
                }
            }
        }

        protected virtual void Dispose(bool isDisposing)
        {
            SharedContent.Dispose();
            SharedContent = null;
        }


        /// <summary>
        /// 根据资源更新实例信息,若无法更新则返回异常;
        /// </summary>
        public virtual void Update(LoadableContentFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            NewDescription = factory.ReadDescription(this);
        }



        /// <summary>
        /// 枚举所有文件信息;(线程安全)
        /// </summary>
        public virtual IEnumerable<string> ConcurrentEnumerateFiles()
        {
            lock (asyncLock)
            {
                return EnumerateFiles().ToList();
            }
        }

        /// <summary>
        /// 枚举所有文件;(线程安全)
        /// </summary>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<string> ConcurrentEnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            lock (asyncLock)
            {
                return EnumerateFiles(searchPattern, searchOption).ToList();
            }
        }

        /// <summary>
        /// 枚举所有文件;(线程安全)
        /// </summary>
        /// <param name="directoryName">指定目录名称</param>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<string> ConcurrentEnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            if(directoryName == null)
                throw new ArgumentNullException(nameof(directoryName));
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            lock (asyncLock)
            {
                return EnumerateFiles(directoryName, searchPattern, searchOption).ToList();
            }
        }

        /// <summary>
        /// 搜索指定路径,直到返回true停止,若未找到则返回 null;(线程安全)
        /// </summary>
        public virtual string ConcurrentFind(Func<string, bool> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            lock (asyncLock)
            {
                return Find(func);
            }
        }

        /// <summary>
        /// 获取到只读的流,若未能获取到则返回 FileNotFoundException 异常;(线程安全)
        /// </summary>
        public virtual Stream ConcurrentGetInputStream(string relativePath)
        {
            lock (asyncLock)
            {
                return GetInputStream(relativePath);
            }
        }


        /// <summary>
        /// 枚举所有文件路径;(允许为非线程安全方法)
        /// </summary>
        public abstract IEnumerable<string> EnumerateFiles();

        /// <summary>
        /// 枚举所有文件路径;(允许为非线程安全方法)
        /// </summary>
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
        /// 枚举所有文件路径;(允许为非线程安全方法)
        /// </summary>
        public virtual IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
            if (directoryName == null)
                throw new ArgumentNullException(nameof(directoryName));
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
        /// 搜索指定路径,直到返回true停止,若未找到则返回 null;(允许为非线程安全方法)
        /// </summary>
        public virtual string Find(Func<string, bool> func)
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
        /// 获取到只读的流,若未能获取到则返回 FileNotFoundException 异常;(允许为非线程安全方法)
        /// </summary>
        public abstract Stream GetInputStream(string relativePath);



        /// <summary>
        /// 在更改内容之前需要先调用;(非线程安全)
        /// </summary>
        public abstract void BeginUpdate();

        /// <summary>
        /// 在更改内容之后,需要调用此方法来完成内容更新;(非线程安全)
        /// </summary>
        public abstract void CommitUpdate();

        /// <summary>
        /// 添加到资源,若不存在该文件则加入到,若已经存在该文件,则更新其;(非线程安全)
        /// </summary>
        public abstract void AddOrUpdate(string relativePath, Stream stream);

        /// <summary>
        /// 移除指定资源;(非线程安全)
        /// </summary>
        public abstract bool Remove(string relativePath);

        /// <summary>
        /// 获取到输出流,若文件已经存在则返回该流,否则返回空的用于写的流;(非线程安全)
        /// </summary>
        public abstract Stream GetOutStream(string relativePath);

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流;(非线程安全)
        /// </summary>
        public abstract Stream CreateOutStream(string relativePath);



        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取则返回异常;(线程安全)
        /// </summary>
        public AssetBundle GetAssetBundle(AssetPath path)
        {
            return SharedContent.GetAssetBundle(path);
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取,则读取并返回,若不存在 AssetBundle 或读取失败则返回异常;(线程安全)
        /// </summary>
        public AssetBundle GetOrLoadAssetBundle(AssetPath path)
        {
            return SharedContent.GetOrLoadAssetBundle(path);
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取则返回异常;(线程安全)
        /// </summary>
        public AssetBundle GetAssetBundle(string assetBundleName)
        {
            return SharedContent.GetAssetBundle(assetBundleName);
        }

        /// <summary>
        /// 读取到指定的 AssetBundle,若未找到则返回异常;(线程安全)
        /// </summary>
        public AssetBundle GetOrLoadAssetBundle(string assetBundleName)
        {
            return SharedContent.GetOrLoadAssetBundle(assetBundleName);
        }


        /// <summary>
        /// 若该实例已经被销毁,则返回异常;
        /// </summary>
        protected void ThrowIfObjectDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }
    }
}
