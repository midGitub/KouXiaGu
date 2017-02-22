using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.WorldEvents
{

    //public interface IUnique
    //{

    //    /// <summary>
    //    /// 唯一标识;
    //    /// </summary>
    //    int ID { get; }
    //}


    /// <summary>
    /// 拓展方法;
    /// </summary>
    public static class UniqueExtensions
    {

        /// <summary>
        /// 根据给定方法获得 以唯一值为Key 的字典结构;
        /// </summary>
        public static CustomDictionary<int, T> ToDictionary<T>(this IEnumerable<int> identifications, Func<int, T> func)
        {
            var dictionary = new CustomDictionary<int, T>();

            foreach (var id in identifications)
            {
                T item = func(id);
                dictionary.Add(id, item);
            }

            return dictionary;
        }

        /// <summary>
        /// 根据给定方法获得 以唯一值为Key 的字典结构;
        /// </summary>
        public static CustomDictionary<int, T> ToDictionary<T>(this IEnumerable<int> identifications)
             where T : new()
        {
            var dictionary = new CustomDictionary<int, T>();

            foreach (var id in identifications)
            {
                T item = new T();
                dictionary.Add(id, item);
            }

            return dictionary;
        }


    }


}
