using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 合集;
    /// </summary>
    public class Set<T> : ICollection<T>
    {
        public Set()
        {
            collection = new List<T>();
            comparer = EqualityComparer<T>.Default;
        }

        public Set(IEqualityComparer<T> comparer)
        {
            collection = new List<T>();
            this.comparer = comparer;
        }

        readonly List<T> collection;
        readonly IEqualityComparer<T> comparer;

        public IEqualityComparer<T> Comparer
        {
            get { return comparer; }
        }

        public int Count
        {
            get { return collection.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 加入元素,若已经存在则返回异常;O(n)
        /// </summary>
        public void Add(T item)
        {
            if (collection.Contains(item, comparer))
            {
                throw new ArgumentException();
            }
            else
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// 移除元素;O(n)
        /// </summary>
        public bool Remove(T item)
        {
            return collection.Remove(item);
        }

        /// <summary>
        /// 确实是否存在此元素;O(n)
        /// </summary>
        public bool Contains(T item)
        {
            return collection.Contains(item);
        }

        public void Clear()
        {
            collection.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
