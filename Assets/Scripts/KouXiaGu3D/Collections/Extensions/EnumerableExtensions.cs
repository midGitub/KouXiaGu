using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Collections
{

    public static class EnumerableExtensions
    {

        public static T[] ToArray<T, TSOU>(this IEnumerable<TSOU> collection, Func<TSOU, T> func)
        {
            List<T> results = new List<T>();

            foreach (var item in collection)
            {
                T result = func(item);
                results.Add(result);
            }

            return results.ToArray();
        }

    }

}
