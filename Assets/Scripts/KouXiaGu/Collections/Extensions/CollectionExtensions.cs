using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Collections
{


    public static class CollectionExtensions
    {

        public static void Add<T>(this ICollection<T> collections, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collections.Add(item);
            }
        }


    }

}
