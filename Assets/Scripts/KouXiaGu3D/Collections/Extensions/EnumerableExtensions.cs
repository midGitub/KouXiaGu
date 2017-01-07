using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Collections
{

    public static class EnumerableExtensions
    {

        public static T[] ToArray<T, TSource>(this IEnumerable<TSource> collection, Func<TSource, T> func)
        {
            List<T> results = new List<T>();

            foreach (var item in collection)
            {
                T result = func(item);
                results.Add(result);
            }

            return results.ToArray();
        }

        /// <summary>
        /// 加入到最后返回;
        /// </summary>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, T item)
        {
            foreach (var value in collection)
            {
                yield return value;
            }
            yield return item;
        }

        /// <summary>
        /// 加入到最后返回;
        /// </summary>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, IEnumerable<T> other)
        {
            foreach (var value in collection)
            {
                yield return value;
            }
            foreach (var value in other)
            {
                yield return value;
            }
        }

    }

}
