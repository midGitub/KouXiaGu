using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{


    public static class CollectionExtensions
    {

        /// <summary>
        /// 移除元素;
        /// </summary>
        /// <param name="item">要在序列中定位的值</param>
        /// <param name="comparer">一个对值进行比较的相等比较器;</param>
        public static bool Remove<T>(IList<T> collection, T item, IEqualityComparer<T> comparer)
        {
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
        /// <param name="item">要在序列中定位的值</param>
        /// <param name="comparer">一个对值进行比较的相等比较器;</param>
        public static int FindIndex<T>(this IList<T> collection, T item, IEqualityComparer<T> comparer)
        {
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
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        public static int FindIndex<T>(this IList<T> collection, T item)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                T original = collection[i];
                if (item.Equals(original))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
