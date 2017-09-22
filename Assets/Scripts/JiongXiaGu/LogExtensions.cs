using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Collections;

namespace JiongXiaGu
{

    /// <summary>
    /// 用于调试输出的拓展;
    /// </summary>
    public static class LogExtensions
    {
        const string NullString = "Null";

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToLog<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return "Count:" + NullString;
            }
            else
            {
                string log = string.Empty;
                int index = 0;
                foreach (var item in enumerable)
                {
                    log += "\n" + Log(index++, item);
                }
                return "Count:" + index + log;
            }
        }

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToLog<T>(this IEnumerable<T> enumerable, Func<T, string> getLog)
        {
            if (enumerable == null)
            {
                return "Count:" + NullString;
            }
            else
            {
                string log = string.Empty;
                int index = 0;
                foreach (var item in enumerable)
                {
                    log += "\n" + Log(index++, item, getLog);
                }
                return "Count:" + index + log;
            }
        }

        /// <summary>
        /// 输出所有元素内容;
        /// </summary>
        public static string ToLog<T>(this IEnumerable<T> enumerable, string descr)
        {
            if (enumerable == null)
            {
                return descr + ",Count:" + NullString;
            }
            else
            {
                string log = string.Empty;
                int index = 0;
                foreach (var item in enumerable)
                {
                    log += "\n" + Log(index++, item);
                }
                return descr + ",Count:" + index + log;
            }
        }

        public static string ToLog<T>(this IEnumerable<T> enumerable, Func<T, string> getLog, string descr)
        {
            if (enumerable == null)
            {
                return NullString;
            }
            else
            {
                string log = "";
                int index = 0;
                foreach (var item in enumerable)
                {
                    log += Log(index++, item, getLog);
                }
                return descr + ",Count:" + index + log;
            }
        }

        public static string Log<T>(int index, T item)
        {
            return "[" + index + "]" + "{" + item.ToString() + "}";
        }

        public static string Log<T>(int index, T item, Func<T, string> getLog)
        {
            return "[" + index + "]" + "{" + getLog(item) + "}";
        }
    }
}
