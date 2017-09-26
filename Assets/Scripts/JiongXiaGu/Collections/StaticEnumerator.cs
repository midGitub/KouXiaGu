using System;
using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 空的迭代结构;
    /// </summary>
    public struct StaticEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        public static StaticEnumerator<T> Default
        {
            get { return new StaticEnumerator<T>(); }
        }

        T IEnumerator<T>.Current
        {
            get { return default(T); }
        }

        object IEnumerator.Current
        {
            get { return default(T); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this;
        }

        bool IEnumerator.MoveNext()
        {
            return false;
        }

        void IEnumerator.Reset()
        {
        }

        void IDisposable.Dispose()
        {
        }
    }
}
