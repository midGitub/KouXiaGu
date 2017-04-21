using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public static class EnumerableExtensions
    {

        /// <summary>
        /// 若存在符合条件的元素则返回true;
        /// </summary>
        public static bool Contains<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            foreach (var item in collection)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }










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

        /// <summary>
        /// 尝试获取到第一个元素,若不存在则返回false,否则返回true;
        /// </summary>
        public static bool TryGetFirst<T>(this IEnumerable<T> collection, out T item)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (var value in collection)
            {
                item = value;
                return true;
            }

            item = default(T);
            return false;
        }


        /// <summary>
        /// 批量释放;
        /// </summary>
        public static void DisposeAll<T>(this IEnumerable<T> collection)
            where T : IDisposable
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (var item in collection)
            {
                item.Dispose();
            }
        }

    }

}
