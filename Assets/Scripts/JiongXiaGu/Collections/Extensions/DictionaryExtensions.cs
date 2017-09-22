using System;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.Collections
{

    public static class DictionaryExtensions
    {

        /// <summary>
        /// 加入或者更新原来的值,若存在则更新值,并返回 true;
        /// </summary>
        public static bool AddOrUpdate<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            KeyValuePair<TKey, TValue> pair)
        {
            return AddOrUpdate(dictionary, pair.Key, pair.Value);
        }

        /// <summary>
        /// 加入或者更新原来的值,若存在则更新值,并返回 true;
        /// </summary>
        public static bool AddOrUpdate<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }

        /// <summary>
        /// 将元素加入到,若已经存在,则进行替换;
        /// </summary>
        public static void AddOrUpdate<TKey,TValue>(
            this IDictionary<TKey, TValue> dictionary, 
            IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (var pair in collection)
            {
                AddOrUpdate(dictionary, pair);
            }
        }

        /// <summary>
        /// 将元素加入到,若已经存在,则进行替换;
        /// </summary>
        public static void AddOrUpdate<TKey, TValue, T>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<T> collection,
            Func<T, KeyValuePair<TKey, TValue>> func)
        {
            foreach (var value in collection)
            {
                KeyValuePair<TKey, TValue> pair = func(value);
                AddOrUpdate(dictionary, pair);
            }
        }

        /// <summary>
        /// 将元素加入到,若已经存在,则进行替换;
        /// </summary>
        public static void AddOrUpdate<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<TValue> collection, 
            Func<TValue, TKey> func)
        {
            foreach (var value in collection)
            {
                TKey key = func(value);
                AddOrUpdate(dictionary, key, value);
            }
        }

        /// <summary>
        /// 获取到,若不存在则返回默认值;
        /// </summary>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            try
            {
                return dictionary[key];
            }
            catch (KeyNotFoundException)
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// 转换成字典,若存在相同的元素则返回异常;
        /// </summary>
        public static Dictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(
            this IEnumerable<T> collection,
            Func<T, KeyValuePair<TKey,TValue>> func)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            IDictionary<TKey, TValue> dictionaryI = dictionary;
            foreach (var key in collection)
            {
                var pair = func(key);
                dictionaryI.Add(pair);
            }
            return dictionary;
        }


    }

}
