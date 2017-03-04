using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu.Collections
{

    /// <summary>
    /// 排序合集;使用二分插入排序;
    /// </summary>
    public class SortedList<T> : ICollection<T>, IEnumerable<T>
    {

        public SortedList()
        {
            this.Comparer = Comparer<T>.Default;
            collection = new List<T>();
        }

        public SortedList(IComparer<T> comparer)
        {
            this.Comparer = comparer;
            collection = new List<T>();
        }

        public SortedList(IEnumerable<T> items)
        {
            this.Comparer = Comparer<T>.Default;
            collection = new List<T>();
            Add(items);
        }

        public SortedList(IEnumerable<T> items, IComparer<T> comparer)
        {
            this.Comparer = comparer;
            collection = new List<T>();
            Add(items);
        }

        /// <summary>
        /// 深拷贝;
        /// </summary>
        public SortedList(SortedList<T> other)
        {
            this.collection = new List<T>(other.collection);
            this.Comparer = other.Comparer;
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
        /// 元素总数;
        /// </summary>
        public int Count
        {
            get { return collection.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>)this.collection).IsReadOnly; }
        }


        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// 加入元素到合集;
        /// </summary>
        public void Add(T item)
        {
            int index = GetInsertIndex(item);
            collection.Insert(index, item);
        }

        /// <summary>
        /// 获取到插入下标;
        /// </summary>
        int GetInsertIndex(T item)
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
        /// 确认是否存在此元素;
        /// </summary>
        public bool Contains(T item)
        {
            return collection.Contains(item);
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
