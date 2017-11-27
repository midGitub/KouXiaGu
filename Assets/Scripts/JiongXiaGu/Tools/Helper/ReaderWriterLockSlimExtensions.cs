using System;
using System.Threading;

namespace JiongXiaGu
{

    /// <summary>
    /// 使用 using() 语法进行锁操作的拓展方法;
    /// </summary>
    public static class ReaderWriterLockSlimExtensions
    {

        /// <summary>
        /// 提供 using语句 读锁;
        /// </summary>
        public static IDisposable ReadLock(this ReaderWriterLockSlim readerWriterLockSlim)
        {
            return new ReaderWriterLockSlimReadLock(readerWriterLockSlim);
        }

        private class ReaderWriterLockSlimReadLock : IDisposable
        {
            private bool isDisposed;
            private ReaderWriterLockSlim readerWriterLockSlim;

            public ReaderWriterLockSlimReadLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                isDisposed = false;
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterReadLock();
            }

            public void Dispose()
            {
                if (!isDisposed)
                {
                    readerWriterLockSlim.ExitReadLock();
                    readerWriterLockSlim = null;
                    isDisposed = true;
                }
            }
        }

        /// <summary>
        /// 提供 using语句 可升级读锁;
        /// </summary>
        public static IDisposable UpgradeableReadLock(this ReaderWriterLockSlim readerWriterLockSlim)
        {
            return new ReaderWriterLockSlimUpgradeableReadLock(readerWriterLockSlim);
        }

        private class ReaderWriterLockSlimUpgradeableReadLock : IDisposable
        {
            private bool isDisposed;
            private ReaderWriterLockSlim readerWriterLockSlim;

            public ReaderWriterLockSlimUpgradeableReadLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                isDisposed = false;
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterUpgradeableReadLock();
            }

            public void Dispose()
            {
                if (!isDisposed)
                {
                    readerWriterLockSlim.ExitUpgradeableReadLock();
                    readerWriterLockSlim = null;
                    isDisposed = true;
                }
            }
        }

        /// <summary>
        /// 提供 using语句 写入锁;
        /// </summary>
        public static IDisposable WriteLock(this ReaderWriterLockSlim readerWriterLockSlim)
        {
            return new ReaderWriterLockSlimWriteLock(readerWriterLockSlim);
        }

        private class ReaderWriterLockSlimWriteLock : IDisposable
        {
            private bool isDisposed;
            ReaderWriterLockSlim readerWriterLockSlim;

            public ReaderWriterLockSlimWriteLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                isDisposed = false;
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterWriteLock();
            }

            public void Dispose()
            {
                if (!isDisposed)
                {
                    readerWriterLockSlim.ExitWriteLock();
                    readerWriterLockSlim = null;
                    isDisposed = true;
                }
            }
        }
    }
}
