//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace KouXiaGu
//{

//    /// <summary>
//    /// 用于调试输出的拓展;
//    /// </summary>
//    public static class TestLog
//    {

//        public static string ToString<T>(this IEnumerable<T> collection)
//        {
//            string log = collection.GetType().Name;
//            int index = 0;
//            foreach (var item in collection)
//            {
//                log += Log(index, item);
//            }
//            return log;
//        }

//        public static string Log<T>(int index, T item)
//        {
//            return string.Concat("\n", "[", index, "]", "{", item.ToString(), "}");
//        }

//    }

//}
