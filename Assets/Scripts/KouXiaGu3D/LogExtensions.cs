using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 用于调试输出的拓展;
    /// </summary>
    public static class LogExtensions
    {

        public static string ToEnumerableLog<T>(this IEnumerable<T> enumerable)
        {
            return ToEnumerableLog(enumerable, "EnumerableLog:");
        }

        public static string ToEnumerableLog<T>(this IEnumerable<T> enumerable, string descr)
        {
            string log = descr;
            int index = 0;
            foreach (var item in enumerable)
            {
                log += Log(index++, item);
            }
            return log;
        }

        public static string ToLog<T>(this ICollection<T> collection)
        {
            string log = collection.GetType().Name + ",Count:" + collection.Count;
            return collection.ToEnumerableLog(log);
        }

        public static string ToLog<T>(this ICollection<T> collection, string descr)
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
