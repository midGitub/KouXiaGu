using System;

namespace JiongXiaGu.Unity.Schedulers
{

    /// <summary>
    /// Unity数据锁;
    /// </summary>
    public class DataLockTokenSource
    {
        private volatile bool isReadOnly;

        /// <summary>
        /// 是否只读?
        /// </summary>
        public bool IsReadOnly => isReadOnly;

        public void EnterReadLock()
        {
            isReadOnly = true;
        }

        public void ExitReadLock()
        {
            isReadOnly = false;
        }

        public IDisposable ReadLock()
        {
            EnterReadLock();
            return new ExitReadLockDisposer(this);
        }
        
        private struct ExitReadLockDisposer : IDisposable
        {
            private DataLockTokenSource source;

            public ExitReadLockDisposer(DataLockTokenSource source)
            {
                this.source = source;
            }

            public void Dispose()
            {
                source.ExitReadLock();
            }
        }
    }
}
