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

        class ReaderWriterLockSlimReadLock : IDisposable
        {
            private bool isDisposed = false;
            ReaderWriterLockSlim readerWriterLockSlim;

            public ReaderWriterLockSlimReadLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterReadLock();
            }

            //~ReaderWriterLockSlimReadLock()
            //{
            //    Dispose(false);
            //}

            public void Dispose()
            {
                Dispose(true);
                //GC.SuppressFinalize(this);
            }

            void Dispose(bool disposing)
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

        class ReaderWriterLockSlimUpgradeableReadLock : IDisposable
        {
            private bool isDisposed = false;
            ReaderWriterLockSlim readerWriterLockSlim;

            public ReaderWriterLockSlimUpgradeableReadLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterUpgradeableReadLock();
            }

            //~ReaderWriterLockSlimUpgradeableReadLock()
            //{
            //    Dispose(false);
            //}

            public void Dispose()
            {
                Dispose(true);
                //GC.SuppressFinalize(this);
            }

            void Dispose(bool disposing)
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

        class ReaderWriterLockSlimWriteLock : IDisposable
        {
            private bool isDisposed = false;
            ReaderWriterLockSlim readerWriterLockSlim;

            public ReaderWriterLockSlimWriteLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterWriteLock();
            }

            //~ReaderWriterLockSlimWriteLock()
            //{
            //    Dispose(false);
            //}

            public void Dispose()
            {
                Dispose(true);
                //GC.SuppressFinalize(this);
            }

            void Dispose(bool disposing)
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
