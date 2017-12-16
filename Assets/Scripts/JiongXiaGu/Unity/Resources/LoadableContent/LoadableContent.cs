using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 抽象类 表示可读写资源;(线程安全)
    /// </summary>
    public abstract class LoadableContent : IDisposable
    {
        /// <summary>
        /// 实例异步锁;
        /// </summary>
        internal readonly object asyncLock = new object();

        /// <summary>
        /// 是否已经销毁;
        /// </summary>
        private volatile bool isDisposed;

        /// <summary>
        /// 是否为核心内容?
        /// </summary>
        internal bool IsCoreContent { get; set; }

        /// <summary>
        /// 在创建时使用的描述;
        /// </summary>
        public LoadableContentDescription Description { get; private set; }

        private LoadableContentDescription newDescription;

        /// <summary>
        /// 最新的描述;
        /// </summary>
        public LoadableContentDescription NewDescription
        {
            get { return newDescription; }
            set
            {
                lock (asyncLock)
                {
                    newDescription = value;
                }
            }
        }

        /// <summary>
        /// 分享资源;
        /// </summary>
        internal SharedContent SharedContent { get; private set; }

        protected LoadableContent(LoadableContentDescription description)
        {
            Description = description;
            NewDescription = description;
            SharedContent = new SharedContent(this);
        }

        protected LoadableContent(LoadableContentDescription description, SharedContentSource sharedContentSource)
        {
            Description = description;
            NewDescription = description;
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
        /// 枚举所有文件信息;(线程安全)
        /// </summary>
        public virtual IEnumerable<string> EnumerateFiles()
        {
            ThrowIfObjectDisposed();
            lock (asyncLock)
            {
                return InternalEnumerateFiles().ToList();
            }
        }

        /// <summary>
        /// 枚举所有文件;(线程安全)
        /// </summary>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            lock (asyncLock)
            {
                return InternalEnumerateFiles(searchPattern, searchOption).ToList();
            }
        }

        /// <summary>
        /// 枚举所有文件;(线程安全)
        /// </summary>
        /// <param name="directoryName">指定目录名称</param>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
            if (directoryName == null)
                throw new ArgumentNullException(nameof(directoryName));
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            lock (asyncLock)
            {
                return InternalEnumerateFiles(directoryName, searchPattern, searchOption).ToList();
            }
        }

        /// <summary>
        /// 搜索指定路径,直到返回true停止,若未找到则返回 null;(线程安全)
        /// </summary>
        public virtual string Find(Func<string, bool> func)
        {
            ThrowIfObjectDisposed();
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            lock (asyncLock)
            {
                return InternalFind(func);
            }
        }

        /// <summary>
        /// 获取到只读的流,若未能获取到则返回 FileNotFoundException 异常;(线程安全)
        /// </summary>
        public virtual Stream GetInputStream(string relativePath)
        {
            ThrowIfObjectDisposed();
            lock (asyncLock)
            {
                return InternaltInputStream(relativePath);
            }
        }


        /// <summary>
        /// 枚举所有文件路径;(允许为非线程安全方法)
        /// </summary>
        internal abstract IEnumerable<string> InternalEnumerateFiles();

        /// <summary>
        /// 枚举所有文件路径;(允许为非线程安全方法)
        /// </summary>
        internal virtual IEnumerable<string> InternalEnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (string entry in InternalEnumerateFiles())
                    {
                        string fileName = PathHelper.GetFileName(entry);
                        if (PathHelper.IsMatch(fileName, searchPattern))
                        {
                            yield return entry;
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (string entry in InternalEnumerateFiles())
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
        internal virtual IEnumerable<string> InternalEnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
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
                    foreach (var entry in InternalEnumerateFiles())
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
                    foreach (var entry in InternalEnumerateFiles())
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
        internal virtual string InternalFind(Func<string, bool> func)
        {
            ThrowIfObjectDisposed();
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            foreach (var file in InternalEnumerateFiles())
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
        internal abstract Stream InternaltInputStream(string relativePath);

        /// <summary>
        /// 在更改内容之前需要先调用,并阻塞所以其它操作线程,直到调用 CommitUpdate();(推荐在Using语句内使用该方法)
        /// </summary>
        public IDisposable BeginUpdate()
        {
            try
            {
                Monitor.Enter(asyncLock);
                InternaltBeginUpdate();
                return new CommitUpdateDisposer(this);
            }
            catch (Exception ex)
            {
                Monitor.Exit(asyncLock);
                throw ex;
            }
        }

        /// <summary>
        /// 提供调用 CommitUpdate() 方法的处置接口;
        /// </summary>
        private struct CommitUpdateDisposer : IDisposable
        {
            public LoadableContent Parent { get; private set; }

            public CommitUpdateDisposer(LoadableContent loadableContent)
            {
                Parent = loadableContent;
            }

            public void Dispose()
            {
                if (Parent != null)
                {
                    Parent.CommitUpdate();
                    Parent = null;
                }
            }
        }

        /// <summary>
        /// 在更改内容之后,需要调用此方法来完成内容更新;
        /// </summary>
        private void CommitUpdate()
        {
            Monitor.Exit(asyncLock);
            InternaltCommitUpdate();
        }


        /// <summary>
        /// 添加到资源,若不存在该文件则加入到,若已经存在该文件,则更新其;
        /// </summary>
        public void AddOrUpdate(string relativePath, Stream stream)
        {
            ThrowIfSynchronizationLockException();
            InternaltAddOrUpdate(relativePath, stream);
        }

        /// <summary>
        /// 移除指定资源;
        /// </summary>
        public bool Remove(string relativePath)
        {
            ThrowIfSynchronizationLockException();
            return InternaltRemove(relativePath);
        }

        /// <summary>
        /// 获取到输出流,若文件已经存在则返回该流,否则返回空的用于写的流;(该stream推荐放在using语句内使用)
        /// </summary>
        public Stream GetOutStream(string relativePath)
        {
            ThrowIfSynchronizationLockException();
            return InternaltGetOutStream(relativePath);
        }

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流;(该stream推荐放在using语句内使用)
        /// </summary>
        public Stream CreateOutStream(string relativePath)
        {
            ThrowIfSynchronizationLockException();
            return CreateOutStream(relativePath);
        }

        private void ThrowIfSynchronizationLockException()
        {
            if (!Monitor.TryEnter(asyncLock))
            {
                throw new SynchronizationLockException("操作线程未获取到执行权限");
            }
        }



        /// <summary>
        /// 在更改内容之前需要先调用;(非线程安全)
        /// </summary>
        internal abstract void InternaltBeginUpdate();

        /// <summary>
        /// 在更改内容之后,需要调用此方法来完成内容更新;(非线程安全)
        /// </summary>
        internal abstract void InternaltCommitUpdate();

        /// <summary>
        /// 添加到资源,若不存在该文件则加入到,若已经存在该文件,则更新其;(非线程安全)
        /// </summary>
        internal abstract void InternaltAddOrUpdate(string relativePath, Stream stream);

        /// <summary>
        /// 移除指定资源;(非线程安全)
        /// </summary>
        internal abstract bool InternaltRemove(string relativePath);

        /// <summary>
        /// 获取到输出流,若文件已经存在则返回该流,否则返回空的用于写的流;(非线程安全)
        /// </summary>
        internal abstract Stream InternaltGetOutStream(string relativePath);

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流;(非线程安全)
        /// </summary>
        internal abstract Stream InternaltCreateOutStream(string relativePath);



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
