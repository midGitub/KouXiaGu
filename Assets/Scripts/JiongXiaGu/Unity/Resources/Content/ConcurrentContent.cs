using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 线程安全的资源合集;
    /// </summary>
    public class ConcurrentContent : Content
    {
        internal object asyncLock { get; private set; } = new object();
        public Content Main { get; private set; }

        public override bool IsUpdating => Main.IsUpdating;
        public override bool CanRead => Main.CanRead;
        public override bool CanWrite => Main.CanWrite;
        public override bool IsDisposed => Main.IsDisposed;

        public ConcurrentContent(Content main)
        {
            if (main == null)
                throw new ArgumentNullException(nameof(main));

            Main = main;
        }

        public override void Dispose()
        {
            lock (asyncLock)
            {
                Main.Dispose();
            }
        }

        /// <summary>
        /// 返回所有文件路径;
        /// </summary>
        public override IEnumerable<string> EnumerateFiles()
        {
            ThrowIfObjectDisposed();
            lock (asyncLock)
            {
                return Main.EnumerateFiles().ToList();
            }
        }

        /// <summary>
        /// 返回所有文件路径;
        /// </summary>
        public override IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
            lock (asyncLock)
            {
                return Main.EnumerateFiles(searchPattern, searchOption).ToList();
            }
        }

        /// <summary>
        /// 返回所有文件路径;
        /// </summary>
        public override IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
            lock (asyncLock)
            {
                return Main.EnumerateFiles(directoryName, searchPattern, searchOption).ToList();
            }
        }

        /// <summary>
        /// 搜索指定路径,直到返回true停止,若未找到则返回 null;(该方法性能可能高于 EnumerateFiles() 方法,因为该方法并非需要获取到所以路径,而是采用枚举的方法)
        /// </summary>
        public override string Find(Func<string, bool> func)
        {
            ThrowIfObjectDisposed();
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            lock (asyncLock)
            {
                return Main.Find(func);
            }
        }

        /// <summary>
        /// 获取到对应数据的流,若未能获取到则返回异常;
        /// </summary>
        public override Stream GetInputStream(string relativePath)
        {
            ThrowIfObjectDisposed();
            lock (asyncLock)
            {
                return Main.GetInputStream(relativePath);
            }
        }


        /// <summary>
        /// 在更改内容之前需要先调用,并阻塞所有其它操作线程,直到调用 CommitUpdate();(推荐在Using语句内使用该方法,在using语句内使用,则无需手动调用 CommitUpdate(),否则调用 CommitUpdate() 时可能返回异常)
        /// </summary>
        public override IDisposable BeginUpdate()
        {
            try
            {
                Monitor.Enter(asyncLock);
                return Main.BeginUpdate();
            }
            catch (Exception ex)
            {
                Monitor.Exit(asyncLock);
                throw ex;
            }
        }

        /// <summary>
        /// 在更改内容之后,需要调用此方法来完成内容更新;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        public override void CommitUpdate()
        {
            Monitor.Exit(asyncLock);
            Main.CommitUpdate();
        }

        /// <summary>
        /// 添加到资源,若不存在该文件则加入到,若已经存在该文件,则更新其;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        public override void AddOrUpdate(string relativePath, Stream stream)
        {
            ThrowIfSynchronizationLockException();
            Main.AddOrUpdate(relativePath, stream);
        }

        /// <summary>
        /// 移除指定资源;在使用该方法时需要调用 BeginUpdate();
        /// </summary>
        public override bool Remove(string relativePath)
        {
            ThrowIfSynchronizationLockException();
            return Main.Remove(relativePath);
        }

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流;在使用该方法之前需要调用 BeginUpdate();(该stream推荐放在using语句内使用)
        /// </summary>
        public override Stream CreateOutputStream(string relativePath)
        {
            ThrowIfSynchronizationLockException();
            return Main.CreateOutputStream(relativePath);
        }

        /// <summary>
        /// 获取到输出流,若文件已经存在则返回该流,否则返回空的用于写的流;在使用该方法之前需要调用 BeginUpdate();(该stream推荐放在using语句内使用)
        /// </summary>
        public override Stream GetOutputStream(string relativePath)
        {
            ThrowIfSynchronizationLockException();
            return Main.GetOutputStream(relativePath);
        }

        /// <summary>
        /// 在未锁的线程执行该方法返回异常;
        /// </summary>
        private void ThrowIfSynchronizationLockException()
        {
            if (!Monitor.IsEntered(asyncLock))
            {
                throw new SynchronizationLockException("操作线程未获取到执行权限");
            }
        }
    }
}
