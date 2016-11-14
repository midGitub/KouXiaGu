using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public static class CollectionHelper
    {

        /// <summary>
        /// 将 IEnumerable<KeyValuePair<TKey, TValue>> 内的元素加入到 IDictionary<TKey, TValue>,若已经存在,则进行替换;
        /// ArgumentNullException : key 为 null
        /// NotSupportedException : IDictionary<TKey, TValue> 为只读。
        /// </summary>
        public static void AddOrReplace<TKey,TValue>(this IDictionary<TKey, TValue> dictionary, 
            IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (var pair in collection)
            {
                try
                {
                    dictionary.Add(pair.Key, pair.Value);
                }
                catch (ArgumentException)
                {
                    dictionary[pair.Key] = pair.Value;
                }
            }
        }

    }

}
