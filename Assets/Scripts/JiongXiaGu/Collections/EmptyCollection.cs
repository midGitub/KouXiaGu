using System;
using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 空的迭代结构;
    /// </summary>
    public struct EmptyCollection<T> : IReadOnlyCollection<T>, IReadOnlyList<T>, IEnumerator<T>, IEnumerable<T>
    {
        public static EmptyCollection<T> Default
        {
            get { return new EmptyCollection<T>(); }
        }

        T IEnumerator<T>.Current
        {
            get { return default(T); }
        }

        object IEnumerator.Current
        {
            get { return default(T); }
        }

        int IReadOnlyCollection<T>.Count
        {
            get { return 0; }
        }

        T IReadOnlyList<T>.this[int index]
        {
            get { throw new ArgumentOutOfRangeException("index"); }
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
