using System;
using System.Threading;

namespace JiongXiaGu.Concurrent
{

    /// <summary>
    /// 使用 using() 语法进行锁操作的拓展方法;
    /// </summary>
    public static class ReaderWriterLockSlimExtensions
    {

        /// <summary>
        /// 提供 using语句 进行锁读取;
        /// </summary>
        public static IDisposable ReadLock(this ReaderWriterLockSlim readerWriterLockSlim)
        {
            return new ReaderWriterLockSlimReadLock(readerWriterLockSlim);
        }

        class ReaderWriterLockSlimReadLock : IDisposable
        {
            ReaderWriterLockSlim readerWriterLockSlim;

            public ReaderWriterLockSlimReadLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterReadLock();
            }

            ~ReaderWriterLockSlimReadLock()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            void Dispose(bool disposing)
            {
                if (readerWriterLockSlim != null)
                {
                    readerWriterLockSlim.ExitReadLock();
                    readerWriterLockSlim = null;
                }
            }
        }

        /// <summary>
        /// 提供 using语句 进行锁写入;
        /// </summary>
        public static IDisposable WriteLock(this ReaderWriterLockSlim readerWriterLockSlim)
        {
            return new ReaderWriterLockSlimWriteLock(readerWriterLockSlim);
        }

        class ReaderWriterLockSlimWriteLock : IDisposable
        {
            ReaderWriterLockSlim readerWriterLockSlim;

            public ReaderWriterLockSlimWriteLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterWriteLock();
            }

            ~ReaderWriterLockSlimWriteLock()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            void Dispose(bool disposing)
            {
                if (readerWriterLockSlim != null)
                {
                    readerWriterLockSlim.ExitWriteLock();
                    readerWriterLockSlim = null;
                }
            }
        }
    }
}
