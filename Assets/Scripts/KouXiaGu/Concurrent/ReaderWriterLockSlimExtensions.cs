using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KouXiaGu.Concurrent
{


    /// <summary>
    /// 使用 using() 语法进行锁操作的拓展方法(存在轻微的性能损耗);
    /// </summary>
    public static class ReaderWriterLockSlimExtensions
    {

        public static IDisposable ReadLock(this ReaderWriterLockSlim readerWriterLockSlim)
        {
            return new ReaderWriterLockSlimReadLock(readerWriterLockSlim);
        }

        class ReaderWriterLockSlimReadLock : IDisposable
        {
            public ReaderWriterLockSlimReadLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterReadLock();
            }

            ReaderWriterLockSlim readerWriterLockSlim;

            public void Dispose()
            {
                if (readerWriterLockSlim != null)
                {
                    readerWriterLockSlim.ExitReadLock();
                    readerWriterLockSlim = null;
                }
                GC.SuppressFinalize(this);
            }
        }


        public static IDisposable WriteLock(this ReaderWriterLockSlim readerWriterLockSlim)
        {
            return new ReaderWriterLockSlimWriteLock(readerWriterLockSlim);
        }

        class ReaderWriterLockSlimWriteLock : IDisposable
        {
            public ReaderWriterLockSlimWriteLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                this.readerWriterLockSlim = readerWriterLockSlim;
                readerWriterLockSlim.EnterWriteLock();
            }

            ReaderWriterLockSlim readerWriterLockSlim;

            public void Dispose()
            {
                if (readerWriterLockSlim != null)
                {
                    readerWriterLockSlim.ExitWriteLock();
                    readerWriterLockSlim = null;
                }
                GC.SuppressFinalize(this);
            }
        }
    }
}
