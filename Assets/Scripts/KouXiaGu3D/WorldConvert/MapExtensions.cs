//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace KouXiaGu
//{


//    public static class MapExtensions
//    {

//        /// <summary>
//        /// 将 IEnumerable<KeyValuePair<TKey, TValue>> 内的元素加入到 IMap<TKey, TValue>,若已经存在,则进行替换;
//        /// ArgumentNullException : key 为 null
//        /// NotSupportedException : IDictionary<TKey, TValue> 为只读。
//        /// </summary>
//        public static void AddOrUpdate<TKey, TValue>(this IMap<TKey, TValue> map,
//            IEnumerable<KeyValuePair<TKey, TValue>> collection)
//        {
//            foreach (var pair in collection)
//            {
//                try
//                {
//                    map.Add(pair.Key, pair.Value);
//                }
//                catch (ArgumentException)
//                {
//                    map[pair.Key] = pair.Value;
//                }
//            }
//        }

//        /// <summary>
//        /// 将元素加入到 IMap<TKey, TValue>, 若已经存在,则进行替换;
//        /// 若为加入则返回true,替换返回false;
//        /// </summary>
//        public static bool AddOrUpdate<TKey, TValue>(this IMap<TKey, TValue> map,
//            TKey key, TValue value)
//        {
//            try
//            {
//                map.Add(key, value);
//                return true;
//            }
//            catch (ArgumentException)
//            {
//                map[key] = value;
//                return false;
//            }
//        }

//        /// <summary>
//        /// 将Key从合集中移除,并且返回其值,若不存在则返回异常;
//        /// </summary>
//        public static TValue Dequeue<TKey, TValue>(this IMap<TKey, TValue> map, TKey key)
//        {
//            TValue value;
//            if (map.TryGetValue(key, out value))
//            {
//                map.Remove(key);
//                return value;
//            }
//            else
//            {
//                throw new KeyNotFoundException();
//            }
//        }

//        /// <summary>
//        /// 尝试从合集中移除 Key ,并且返回其值;
//        /// </summary>
//        public static bool TryDequeue<TKey, TValue>(this IMap<TKey, TValue> map, TKey key, out TValue value)
//        {
//            if (map.TryGetValue(key, out value))
//            {
//                map.Remove(key);
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//    }

//}
