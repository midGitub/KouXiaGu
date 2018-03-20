using System;
using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 不允许重复元素的链表;使用 IEqualityComparer<T> 检查重复项;
    /// </summary>
    public class Set<T> : ICollection<T>, IReadOnlyList<T>, IEnumerable<T>
    {
        private readonly List<T> list;
        private readonly IEqualityComparer<T> comparer;

        public int Count => list.Count;
        public IEqualityComparer<T> Comparer => comparer;
        public bool IsReadOnly => ((ICollection<T>)list).IsReadOnly;

        public T this[int index]
        {
            get { return list[index]; }
        }

        public Set()
        {
            list = new List<T>();
            comparer = EqualityComparer<T>.Default;
        }

        /// <summary>
        /// 添加元素;
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void Add(T item)
        {
            if(Contains(item))
                throw new ArgumentException();

            list.Add(item);
        }

        /// <summary>
        /// 移除元素;
        /// </summary>
        public bool Remove(T item)
        {
            int index = FindIndex(item);
            if (index >= 0)
            {
                list.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 确认是否存在元素;
        /// </summary>
        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            for (int i = 0; i < list.Count; i++)
            {
                var original = list[i];
                if (Comparer.Equals(original, item))
                {
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            list.Clear();
        }

        public int FindIndex(T item)
        {
            return list.FindIndex(value => Comparer.Equals(item, value));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
