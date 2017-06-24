using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{
    
    /// <summary>
    /// 合集拓展方法;
    /// </summary>
    public static class CollectionHelper
    {

        static void ValidateCollection<T>(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
        }

        static void ValidateNull(object item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
        }


        /// <summary>
        /// 移除指定下标的元素,同 List 的 RemoveAt();
        /// </summary>
        public static void RemoveAt<T>(ref T[] array, int index)
        {
            Array.Copy(array, index + 1, array, index, array.Length - index - 1);
            Array.Resize(ref array, array.Length - 1);
        }

        #region IList

        /// <summary>
        /// 移除符合要求的第一个元素;
        /// </summary>
        public static bool Remove<T>(this IList<T> list, Func<T, bool> func)
        {
            int index = list.FindIndex(func);
            if (index >= 0)
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除元素;
        /// </summary>
        /// <param name="item">要在序列中定位的值</param>
        /// <param name="comparer">一个对值进行比较的相等比较器;</param>
        public static bool Remove<T>(IList<T> collection, T item, IEqualityComparer<T> comparer)
        {
            ValidateCollection(collection);
            ValidateNull(comparer);
            int index = FindIndex(collection, item, comparer);
            if (index >= 0)
            {
                collection.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        public static int FindIndex<T>(this IList<T> collection, T item)
        {
            return FindIndex(collection, item, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        /// <param name="item">要在序列中定位的值</param>
        /// <param name="comparer">一个对值进行比较的相等比较器;</param>
        public static int FindIndex<T>(this IList<T> collection, T item, IEqualityComparer<T> comparer)
        {
            ValidateCollection(collection);
            ValidateNull(comparer);
            for (int i = 0; i < collection.Count; i++)
            {
                T original = collection[i];
                if (comparer.Equals(original, item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        public static int FindIndex<T>(this IList<T> collection, Func<T, bool> func)
        {
            ValidateCollection(collection);
            ValidateNull(func);
            for (int i = 0; i < collection.Count; i++)
            {
                T original = collection[i];
                if (func(original))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 这两个合集是否拥有相同的内容?
        /// </summary>
        public static bool IsSameContent<T>(this IList<T> c1, IList<T> c2)
        {
            if (c1 == c2)
                return true;
            if (c1 == null || c2 == null)
                return false;
            if (c1.Count != c2.Count)
                return false;

            for (int i = 0; i < c1.Count; i++)
            {
                if (c1[i].Equals(c2[i]))
                    return false;
            }
            return true;
        }

        #endregion

        #region IDictionary

        /// <summary>
        /// 将合集加入到dest,若出现相同的Key则替换;
        /// </summary>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dest, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (var item in items)
            {
                TKey key = item.Key;
                TValue value = item.Value;
                if (dest.ContainsKey(key))
                {
                    dest[key] = value;
                }
                else
                {
                    dest.Add(item);
                }
            }
        }

        #endregion

    }
}
