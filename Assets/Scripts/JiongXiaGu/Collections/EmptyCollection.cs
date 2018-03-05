using System;
using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 空的迭代结构;
    /// </summary>
    public struct EmptyCollection<T> : ICollection<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, IEnumerable<T>, IEnumerable
    {
        public static EmptyCollection<T> Default => new EmptyCollection<T>();

        int IReadOnlyCollection<T>.Count => 0;
        int ICollection<T>.Count => 0;
        bool ICollection<T>.IsReadOnly => true;

        T IReadOnlyList<T>.this[int index]
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// 只返回 NotSupportedException;
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 只返回 NotSupportedException;
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 只返回 NotSupportedException;
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        bool ICollection<T>.Contains(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 只返回 NotSupportedException;
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 只返回 NotSupportedException;
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator();
        }

        private struct Enumerator : IEnumerator<T>
        {
            public T Current => default(T);
            object IEnumerator.Current => default(T);

            public void Dispose()
            {
                return;
            }

            public bool MoveNext()
            {
                return false;
            }

            public void Reset()
            {
                return;
            }
        }
    }
}
