using System;
using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 不稳定的排序合集;使用二分插入排序;
    /// </summary>
    public class SortedList<T> : ICollection<T>, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>
    {

        public SortedList()
        {
            Comparer = Comparer<T>.Default;
            collection = new List<T>();
        }

        public SortedList(IComparer<T> comparer)
        {
            Comparer = comparer;
            collection = new List<T>();
        }

        public SortedList(IEnumerable<T> items)
        {
            Comparer = Comparer<T>.Default;
            collection = new List<T>(items);
            collection.Sort(Comparer);
        }

        public SortedList(IEnumerable<T> items, IComparer<T> comparer)
        {
            Comparer = comparer;
            collection = new List<T>(items);
            collection.Sort(Comparer);
        }

        /// <summary>
        /// 深拷贝;
        /// </summary>
        public SortedList(SortedList<T> other)
        {
            collection = new List<T>(other.collection);
            Comparer = other.Comparer;
        }


        /// <summary>
        /// 元素合集;
        /// </summary>
        List<T> collection;

        /// <summary>
        /// 使用的比较器;
        /// </summary>
        public IComparer<T> Comparer { get; private set; }

        /// <summary>
        /// 根据下标获取到元素;
        /// </summary>
        public T this[int index]
        {
            get { return collection[index]; }
        }

        /// <summary>
        /// 元素总数;
        /// </summary>
        public int Count
        {
            get { return collection.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>)collection).IsReadOnly; }
        }

        /// <summary>
        /// 对 合集 进行重新排序;
        /// </summary>
        public void Sort()
        {
            collection.Sort(Comparer);
        }

        /// <summary>
        /// 加入元素到合集;
        /// </summary>
        public void Add(T item)
        {
            int index = collection.BinarySearch(item, Comparer);
            if (index < 0)
                collection.Insert(~index, item);
            else
                collection.Insert(index, item);
        }

        /// <summary>
        /// 获取到插入下标;
        /// </summary>
        [Obsolete]
        int BinarySearch(T item)
        {
            int low = 0;
            int high = collection.Count - 1;

            while (low <= high)
            {
                int mid = (low + high) >> 1;

                if (Comparer.Compare(item, collection[mid]) < 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return low;
        }

        /// <summary>
        /// 移除指定元素;
        /// </summary>
        public bool Remove(T item)
        {
            return collection.Remove(item);
        }

        /// <summary>
        /// 使用 Comparer 对比,移除指定元素;
        /// </summary>
        public bool CompareRemove(T item)
        {
            int index = collection.BinarySearch(item, Comparer);
            if (index > 0)
            {
                collection.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 确认是否存在此元素;
        /// </summary>
        public bool Contains(T item)
        {
            return collection.Contains(item);
        }

        /// <summary>
        /// 使用 Comparer 对比,确认是否存在此元素;
        /// </summary>
        public bool ComparerContains(T item)
        {
            int index = collection.BinarySearch(item, Comparer);
            return index >= 0;
        }

        /// <summary>
        /// 清除所有元素;
        /// </summary>
        public void Clear()
        {
            collection.Clear();
        }

        /// <summary>
        /// 从目标数组的指定索引处开始，将整个 合集 复制到兼容的一维数组
        /// </summary>
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
            return collection.GetEnumerator();
        }

    }

}
