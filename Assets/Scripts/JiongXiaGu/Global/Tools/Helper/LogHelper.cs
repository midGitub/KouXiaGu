using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Collections;

namespace JiongXiaGu
{

    /// <summary>
    /// 用于文本生成的拓展方法;
    /// </summary>
    public static class TextHelper
    {
        public const string NullString = "Null";

        /// <summary>
        /// 返回所有元素内容的 StringBuilder;
        /// </summary>
        public static StringBuilder ToText<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            Func<T, string> getString = item => item != null ? item.ToString() : NullString;
            StringBuilder stringBuilder = new StringBuilder();
            return ToText_internal(collection, stringBuilder, getString);
        }

        /// <summary>
        /// 返回所有元素内容的 StringBuilder;
        /// </summary>
        public static StringBuilder ToText<T>(this IEnumerable<T> collection, Func<T, string> getString)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (getString == null)
                throw new ArgumentNullException(nameof(getString));

            StringBuilder stringBuilder = new StringBuilder();
            return ToText_internal(collection, stringBuilder, getString);
        }

        /// <summary>
        /// 将所有元素内容按顺序加入到 StringBuilder;
        /// </summary>
        public static StringBuilder ToText<T>(this IEnumerable<T> collection, StringBuilder stringBuilder, Func<T, string> getString)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (stringBuilder == null)
                throw new ArgumentNullException(nameof(stringBuilder));
            if (getString == null)
                throw new ArgumentNullException(nameof(getString));

            return ToText_internal(collection, stringBuilder, getString);
        }

        /// <summary>
        /// 将所有元素内容按顺序加入到 StringBuilder;
        /// </summary>
        private static StringBuilder ToText_internal<T>(IEnumerable<T> collection, StringBuilder stringBuilder, Func<T, string> getString)
        {
            int index = 0;
            foreach (var item in collection)
            {
                string itemString;
                try
                {
                    itemString = getString(item);
                }
                catch (Exception ex)
                {
                    itemString = "Error:" + ex.ToString();
                }
                stringBuilder.AppendFormat("[{0}]{1};", index, itemString);
                stringBuilder.AppendLine();
                index++;
            }
            return stringBuilder;
        }




        ///// <summary>
        ///// 输出所有元素内容;
        ///// </summary>
        //public static string ToLog<T>(this IEnumerable<T> enumerable)
        //{
        //    if (enumerable == null)
        //    {
        //        return "Count:" + NullString;
        //    }
        //    else
        //    {
        //        string log = string.Empty;
        //        int index = 0;
        //        foreach (var item in enumerable)
        //        {
        //            log += "\n" + Log(index++, item);
        //        }
        //        return "Count:" + index + log;
        //    }
        //}

        ///// <summary>
        ///// 输出所有元素内容;
        ///// </summary>
        //public static string ToLog<T>(this IEnumerable<T> enumerable, Func<T, string> getLog)
        //{
        //    if (enumerable == null)
        //    {
        //        return "Count:" + NullString;
        //    }
        //    else
        //    {
        //        string log = string.Empty;
        //        int index = 0;
        //        foreach (var item in enumerable)
        //        {
        //            log += "\n" + Log(index++, item, getLog);
        //        }
        //        return "Count:" + index + log;
        //    }
        //}

        ///// <summary>
        ///// 输出所有元素内容;
        ///// </summary>
        //public static string ToLog<T>(this IEnumerable<T> enumerable, string descr)
        //{
        //    if (enumerable == null)
        //    {
        //        return descr + ",Count:" + NullString;
        //    }
        //    else
        //    {
        //        string log = string.Empty;
        //        int index = 0;
        //        foreach (var item in enumerable)
        //        {
        //            log += "\n" + Log(index++, item);
        //        }
        //        return descr + ",Count:" + index + log;
        //    }
        //}

        //public static string ToLog<T>(this IEnumerable<T> enumerable, Func<T, string> getLog, string descr)
        //{
        //    if (enumerable == null)
        //    {
        //        return NullString;
        //    }
        //    else
        //    {
        //        string log = "";
        //        int index = 0;
        //        foreach (var item in enumerable)
        //        {
        //            log += Log(index++, item, getLog);
        //        }
        //        return descr + ",Count:" + index + log;
        //    }
        //}

        //private static string Log<T>(int index, T item)
        //{
        //    return "[" + index + "]" + "{" + item.ToString() + "}";
        //}

        //private static string Log<T>(int index, T item, Func<T, string> getLog)
        //{
        //    return "[" + index + "]" + "{" + getLog(item) + "}";
        //}
    }
}
