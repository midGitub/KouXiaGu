using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu
{

    /// <summary>
    /// 用于调试输出的拓展;
    /// </summary>
    public static class LogExtensions
    {

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToEnumerableLog<T>(this IEnumerable<T> enumerable)
        {
            return ToEnumerableLog(enumerable, enumerable.GetType().Name);
        }

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToEnumerableLog<T>(this IEnumerable<T> enumerable, string descr)
        {
            string log = "";
            int index = 0;
            foreach (var item in enumerable)
            {
                log += Log(index++, item);
            }
            return descr + ",Count:" + index + log;
        }

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToLog<T>(this ICollection<T> collection)
        {
            string log = collection.GetType().Name + ",Count:" + collection.Count;
            return collection.ToEnumerableLog(log);
        }

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToLog<T>(this ICollection<T> collection, string descr)
        {
            string log = descr + "\nCount:" + collection.Count;
            return collection.ToEnumerableLog(log);
        }

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToLog<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> collection, string descr)
        {
            string log = descr + "\nCount:" + collection.Count;
            return collection.ToEnumerableLog(log);
        }

        public static string Log<T>(int index, T item)
        {
            return string.Concat("\n", "[", index, "]", "{", item.ToString(), "}");
        }

    }

}
