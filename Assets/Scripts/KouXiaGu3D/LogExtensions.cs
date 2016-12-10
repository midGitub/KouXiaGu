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

        public static string ToLog<T>(this IEnumerable<T> collection)
        {
            string log = collection.GetType().Name + "Count:" + collection.Count();
            int index = 0;
            foreach (var item in collection)
            {
                log += Log(index, item);
            }
            return log;
        }

        public static string Log<T>(int index, T item)
        {
            return string.Concat("\n", "[", index, "]", "{", item.ToString(), "}");
        }

    }

}
