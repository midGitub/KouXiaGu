using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 通过阻塞线程实现线程安全的资源合集;
    /// </summary>
    public class BlockingContent : Content
    {
        private readonly object asyncLock = new object();
        private bool isDisposed = false;
        public Content Content { get; private set; }
        public override bool IsUpdating => !isDisposed && Content.IsUpdating;
        public override bool CanRead => !isDisposed && Content.CanRead;
        public override bool CanWrite => !isDisposed && Content.CanWrite;
        public override bool IsDisposed => isDisposed;
        public override bool IsCompress => Content.IsCompress;

        public BlockingContent(Content content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            Content = content;
        }

        public override void Dispose()
        {
            if (!isDisposed)
            {
                Content.Dispose();
                Content = null;

                isDisposed = true;
            }
        }

        #region Read

        public override IEnumerable<IContentEntry> EnumerateEntries()
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
                return Content.EnumerateEntries().ToArray();
            }
        }

        public override IEnumerable<IContentEntry> EnumerateEntries(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
                return Content.EnumerateEntries(searchPattern, searchOption).ToArray();
            }
        }

        public override IEnumerable<IContentEntry> EnumerateEntries(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
                return Content.EnumerateEntries(directoryName, searchPattern, searchOption).ToArray();
            }
        }



        public override IEnumerable<string> EnumerateFiles()
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
                return Content.EnumerateFiles().ToArray();
            }
        }

        public override IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
                return Content.EnumerateFiles(searchPattern, searchOption).ToArray();
            }
        }

        public override IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
                return Content.EnumerateFiles(directoryName, searchPattern, searchOption).ToArray();
            }
        }

        public override IContentEntry Find(Func<IContentEntry, bool> func)
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
                return Content.Find(func);
            }
        }

        public override IContentEntry GetEntry(string name)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string relativePath)
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
                return Content.Contains(relativePath);
            }
        }

        public override Stream GetInputStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
               return Content.GetInputStream(relativePath);
            }
        }

        public override Stream GetInputStream(IContentEntry entry)
        {
            ThrowIfObjectDisposed();

            lock (asyncLock)
            {
                return Content.GetInputStream(entry);
            }
        }

        #endregion

        #region Write

        /// <summary>
        /// 独占的写入,当进行写入时,或阻塞其它线程对此类进行的所有操作;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public override IDisposable BeginUpdate()
        {
            ThrowIfObjectDisposed();

            Monitor.Enter(asyncLock);
            Content.BeginUpdate();
            return new ContentCommitUpdateDisposer(this);
        }

        /// <summary>
        /// 完成写入操作,仅拥有写入权限的线程调用;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="SynchronizationLockException">当前线程没有写入权限</exception>
        public override void CommitUpdate()
        {
            ThrowIfObjectDisposed();

            Monitor.Exit(asyncLock);
            Content.CommitUpdate();
        }

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流;在使用该方法之前需要调用 BeginUpdate();(推荐在using语法内使用)
        /// </summary>
        /// <exception cref="SynchronizationLockException">当前线程没有写入权限</exception>
        public override Stream GetOutputStream(string name, out IContentEntry entry)
        {
            ThrowIfObjectDisposed();

            if (Monitor.IsEntered(asyncLock))
            {
                return Content.GetOutputStream(name, out entry);
            }
            else
            {
                throw new SynchronizationLockException("当前线程没有写入权限;");
            }
        }

        public override Stream GetOutputStream(IContentEntry entry)
        {
            throw new NotImplementedException();
        }

        public override IContentEntry AddOrUpdate(string name, Stream source, bool isCloseStream)
        {
            throw new NotImplementedException();
        }

        public override void Remove(IContentEntry entry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除指定资源;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        /// <exception cref="SynchronizationLockException">当前线程没有写入权限</exception>
        public override bool Remove(string relativePath)
        {
            ThrowIfObjectDisposed();

            if (Monitor.IsEntered(asyncLock))
            {
                return Content.Remove(relativePath);
            }
            else
            {
                throw new SynchronizationLockException("当前线程没有写入权限;");
            }
        }

        #endregion
    }
}
