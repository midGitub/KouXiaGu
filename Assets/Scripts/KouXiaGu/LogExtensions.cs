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
        public static string ToLog<T>(this IEnumerable<T> enumerable)
        {
            return ToLog(enumerable, enumerable.GetType().Name);
        }

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToLog<T>(this IEnumerable<T> enumerable, Func<T, string> getLog)
        {
            return ToLog(enumerable, getLog, enumerable.GetType().Name);
        }

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToLog<T>(this IEnumerable<T> enumerable, string descr)
        {
            string log = "";
            int index = 0;
            foreach (var item in enumerable)
            {
                log += Log(index++, item);
            }
            return descr + ",Count:" + index + log;
        }

        public static string ToLog<T>(this IEnumerable<T> enumerable, Func<T, string> getLog, string descr)
        {
            string log = "";
            int index = 0;
            foreach (var item in enumerable)
            {
                log += Log(index++, item, getLog);
            }
            return descr + ",Count:" + index + log;
        }


        public static string Log<T>(int index, T item)
        {
            return string.Concat("\n", "[", index, "]", "{", item.ToString(), "}");
        }

        public static string Log<T>(int index, T item, Func<T, string> getLog)
        {
            return string.Concat("\n", "[", index, "]", "{", getLog(item), "}");
        }

    }

}
