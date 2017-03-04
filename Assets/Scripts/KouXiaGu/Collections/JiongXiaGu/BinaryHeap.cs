using System;
using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu.Collections
{

    /// <summary>
    /// 二叉堆,随机的迭代顺序;
    /// </summary>
    public class BinaryHeap<T> : ICollection<T>, IEnumerable<T>
    {

        public BinaryHeap()
        {
            this.Comparer = Comparer<T>.Default;
            this.collection = CreateCollection();
        }

        public BinaryHeap(IComparer<T> comparer)
        {
            this.Comparer = comparer;
            this.collection = CreateCollection();
        }

        public BinaryHeap(int capacity)
        {
            this.Comparer = Comparer<T>.Default;
            this.collection = CreateCollection();
        }

        /// <summary>
        /// 深拷贝;
        /// </summary>
        public BinaryHeap(BinaryHeap<T> other)
        {
            this.Comparer = other.Comparer;
            this.collection = new List<T>(other.collection);
        }


        /// <summary>
        /// 根索引值;
        /// </summary>
        const int rootIndex = 1;

        /// <summary>
        /// 元素合集,带头节点(为了方便计算);
        /// </summary>
        List<T> collection;

        /// <summary>
        /// 使用的比较器;
        /// </summary>
        public IComparer<T> Comparer { get; private set; }


        /// <summary>
        /// 结构存在有效的元素个数;
        /// </summary>
        public int Count
        {
            get { return collection.Count - 1; }
        }

        /// <summary>
        /// 根节点,根据排序的最大值或者最小值;
        /// </summary>
        public T Root
        {
            get { return collection[rootIndex]; }
        }

        /// <summary>
        /// 最后一个元素下标;
        /// </summary>
        int lastNodeIndex
        {
            get { return collection.Count - 1; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>)this.collection).IsReadOnly; }
        }


        /// <summary>
        /// 按循序加入到数据结构;
        /// </summary>
        public void Add(T item)
        {
            collection.Add(item);
            BubbleUp(lastNodeIndex);
        }

        /// <summary>
        /// 移除指定元素,若移除成功返回true,否则返回false;
        /// </summary>
        public bool Remove(T item)
        {
            if (Count == 0)
                return false;

            int index = collection.IndexOf(item, rootIndex, Count);
            if (index == -1)
                return false;

            collection[index] = collection[lastNodeIndex];
            BubbleDown(index);
            collection.RemoveAt(lastNodeIndex);

            return true;
        }

        /// <summary>
        /// 输出并且移除根节点;
        /// </summary>
        public T Extract()
        {
            if (Count == 0)
                throw new InvalidOperationException("已经不存在元素;");

            T result = collection[rootIndex];
            collection[rootIndex] = collection[lastNodeIndex];
            BubbleDown(rootIndex);
            collection.RemoveAt(lastNodeIndex);

            return result;
        }

        /// <summary>
        /// 确认是否存在此元素;
        /// </summary>
        public bool Contains(T item)
        {
            return collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 创建新的合集结构;
        /// </summary>
        List<T> CreateCollection()
        {
            List<T> collection = new List<T>();
            collection.Add(default(T));
            return collection;
        }

        /// <summary>
        /// 清空;
        /// </summary>
        public void Clear()
        {
            collection.Clear();
            collection.Add(default(T));
        }

        /// <summary>
        /// 比较 other 是否置于 current 之前;
        /// </summary>
        bool Compare(T current, T other)
        {
            return Comparer.Compare(current, other) >= 0;
        }

        void BubbleUp(int index)
        {
            int currentIndex = index;
            int parentIndex = GetParentIndex(index);
            T current = collection[currentIndex];

            while (currentIndex > rootIndex)
            {
                if (Compare(collection[parentIndex], current))
                {
                    collection[currentIndex] = collection[parentIndex];
                    currentIndex = parentIndex;
                    parentIndex = GetParentIndex(parentIndex);
                }
                else
                {
                    break;
                }
            }
            collection[currentIndex] = current;
        }

        void BubbleDown(int index)
        {
            int currentIndex = index;
            int leftChildIndex = GetLeftChildIndex(index);
            T current = collection[currentIndex];

            while (leftChildIndex <= lastNodeIndex)
            {
                if (leftChildIndex < lastNodeIndex && Compare(collection[leftChildIndex], collection[leftChildIndex + 1]))
                {
                    leftChildIndex++;
                }

                if (Compare(current, collection[leftChildIndex]))
                {
                    collection[currentIndex] = collection[leftChildIndex];
                    currentIndex = leftChildIndex;
                    leftChildIndex = GetLeftChildIndex(leftChildIndex);
                }
                else
                {
                    break;
                }
            }
            collection[currentIndex] = current;
        }

        /// <summary>
        /// 获取到父节点下标;
        /// </summary>
        int GetParentIndex(int index)
        {
            return (int)decimal.Floor(index / 2);
        }

        /// <summary>
        /// 获取到左孩子下标;
        /// </summary>
        int GetLeftChildIndex(int index)
        {
            return (2 * index);
        }

        /// <summary>
        /// 获取到右孩子下标;
        /// </summary>
        int GetRightChildIndex(int index)
        {
            return (2 * index + 1);
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
