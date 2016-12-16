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

        /// <summary>
        /// 枚举出所有元素内容信息;
        /// </summary>
        public static string ToEnumerableLog<T>(this IEnumerable<T> enumerable)
        {
            string log = enumerable.GetType().Name + "Count:" + enumerable.Count();
            int index = 0;
            foreach (var item in enumerable)
            {
                log += Log(index++, item);
            }
            return log;
        }

        public static string ToCollectionLog<T>(this ICollection<T> collection)
        {
            string log = collection.GetType().Name;
            log += "元素总数:" + collection.Count;
            return log;
        }

        public static string ToLog<T, TP>(this IMap<TP, T> map)
        {
            string log = map.GetType().Name;
            log += "元素总数:" + map.Count;
            return log;
        }

        public static string Log<T>(int index, T item)
        {
            return string.Concat("\n", "[", index, "]", "{", item.ToString(), "}");
        }

    }

}
