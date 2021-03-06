﻿using System.Linq;
using System;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 合集拓展方法;
    /// </summary>
    public static partial class CollectionExtensions
    {

        #region AddRange

        /// <summary>
        /// 添加多个合集内容到链表;(此方法会改变list.Capacity的值)
        /// </summary>
        public static void AddRange<T>(this List<T> list, params ICollection<T>[] collections)
        {
            int capacity = list.Capacity;

            for (int i = 0; i < collections.Length; i++)
            {
                var collection = collections[i];

                if (collection == null)
                {
                    throw new ArgumentNullException(string.Format("合集第{0}个元素为Null", i));
                }
                else
                {
                    capacity += collection.Count;
                }
            }

            list.Capacity = capacity;

            foreach (var collection in collections)
            {
                list.AddRange(collection);
            }
        }

        /// <summary>
        /// 添加多个合集内容到链表;(此方法会改变list.Capacity的值)
        /// </summary>
        public static void AddRange<T>(this List<T> list, params IReadOnlyCollection<T>[] collections)
        {
            int capacity = list.Capacity;

            for (int i = 0; i < collections.Length; i++)
            {
                IReadOnlyCollection<T> collection = collections[i];

                if (collection == null)
                {
                    throw new ArgumentNullException(string.Format("合集第{0}个元素为Null", i));
                }
                else
                {
                    capacity += collection.Count;
                }
            }

            list.Capacity = capacity;

            foreach (var collection in collections)
            {
                list.AddRange(collection);
            }
        }

        #endregion


        #region AddOrUpdate

        /// <summary>
        /// 若合集内不存在则加入到,否者更新合集内的元素;
        /// </summary>
        public static AddOrUpdateStatus AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> pair)
        {
            return AddOrUpdate(dictionary, pair.Key, pair.Value);
        }

        /// <summary>
        /// 若合集内不存在则加入到,否者更新合集内的元素;
        /// </summary>
        public static AddOrUpdateStatus AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return AddOrUpdateStatus.Updated;
            }
            else
            {
                dictionary.Add(key, value);
                return AddOrUpdateStatus.Added;
            }
        }

        /// <summary>
        /// 将合集加入到dest,若出现相同的Key则替换;
        /// </summary>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dest, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (var item in items)
            {
                TKey key = item.Key;
                TValue value = item.Value;
                if (dest.ContainsKey(key))
                {
                    dest[key] = value;
                }
                else
                {
                    dest.Add(item);
                }
            }
        }

        #endregion


        #region FindIndex

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        public static int FindIndex<T>(this IList<T> collection, T item)
        {
            return FindIndex(collection, value => EqualityComparer<T>.Default.Equals(value, item));
        }

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static int FindIndex<T>(this IList<T> collection, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            for (int i = 0; i < collection.Count; i++)
            {
                T original = collection[i];
                if (match.Invoke(original))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        /// <param name="startIndex">从零开始的搜索的起始索引</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static int FindIndex<T>(this IList<T> collection, int startIndex, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (startIndex > collection.Count)
                throw new IndexOutOfRangeException(nameof(startIndex));
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            for (int i = startIndex; i < collection.Count; i++)
            {
                T original = collection[i];
                if (match.Invoke(original))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        /// <param name="startIndex">从零开始的搜索的起始索引</param>
        /// <param name="count">要搜索的部分中的元素数</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static int FindIndex<T>(this IList<T> collection, int startIndex, int count, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (startIndex > collection.Count)
                throw new IndexOutOfRangeException(nameof(startIndex));
            if (count < 0 || startIndex > collection.Count - count)
                throw new IndexOutOfRangeException(nameof(count));
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            int endIndex = startIndex + count;
            for (int i = startIndex; i < endIndex; i++)
            {
                T original = collection[i];
                if (match.Invoke(original))
                {
                    return i;
                }
            }
            return -1;
        }


        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        public static int FindIndex<T>(this IReadOnlyList<T> collection, T item)
        {
            return FindIndex(collection, value => EqualityComparer<T>.Default.Equals(value, item));
        }

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static int FindIndex<T>(this IReadOnlyList<T> collection, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            for (int i = 0; i < collection.Count; i++)
            {
                T original = collection[i];
                if (match.Invoke(original))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        /// <param name="startIndex">从零开始的搜索的起始索引</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static int FindIndex<T>(this IReadOnlyList<T> collection, int startIndex, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (startIndex > collection.Count)
                throw new IndexOutOfRangeException(nameof(startIndex));
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            for (int i = startIndex; i < collection.Count; i++)
            {
                T original = collection[i];
                if (match.Invoke(original))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取到对应元素下标,若不存在则返回-1;
        /// </summary>
        /// <param name="startIndex">从零开始的搜索的起始索引</param>
        /// <param name="count">要搜索的部分中的元素数</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static int FindIndex<T>(this IReadOnlyList<T> collection, int startIndex, int count, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (startIndex > collection.Count)
                throw new IndexOutOfRangeException(nameof(startIndex));
            if (count < 0 || startIndex > collection.Count - count)
                throw new IndexOutOfRangeException(nameof(count));
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            int endIndex = startIndex + count;
            for (int i = startIndex; i < endIndex; i++)
            {
                T original = collection[i];
                if (match.Invoke(original))
                {
                    return i;
                }
            }
            return -1;
        }


        #endregion


        #region TryAdd

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.ContainsKey(key))
            {
                return false;
            }
            else
            {
                dictionary.Add(key, value);
                return true;
            }
        }

        #endregion


        /// <summary>
        /// 确认合集内是否存在符合条件的元素;
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="match">若存在则返回true</param>
        public static bool Contains<T>(this IEnumerable<T> collection, Func<T, bool> match)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (match == null)
                throw new ArgumentNullException("comparer");

            foreach (var item in collection)
            {
                if (match(item))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 移除符合要求的第一个元素;
        /// </summary>
        public static bool Remove<T>(this IList<T> list, Predicate<T> predicate)
        {
            if (list == null)
                throw new ArgumentNullException("collection");
            if (predicate == null)
                throw new ArgumentNullException("comparer");

            int index = list.FindIndex(predicate);
            if (index >= 0)
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        ///// <summary>
        ///// 移除元素;
        ///// </summary>
        ///// <param name="item">要在序列中定位的值</param>
        ///// <param name="comparer">一个对值进行比较的相等比较器;</param>
        //public static bool Remove<T>(IList<T> collection, T item, IEqualityComparer<T> comparer)
        //{
        //    if (collection == null)
        //        throw new ArgumentNullException("collection");
        //    if (comparer == null)
        //        throw new ArgumentNullException("comparer");

        //    int index = FindIndex(collection, item, comparer);
        //    if (index >= 0)
        //    {
        //        collection.RemoveAt(index);
        //        return true;
        //    }
        //    return false;
        //}



        /// <summary>
        /// 移除指定下标的元素,同 List 的 RemoveAt();
        /// </summary>
        public static void RemoveAt<T>(ref T[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Array.Copy(array, index + 1, array, index, array.Length - index - 1);
            Array.Resize(ref array, array.Length - 1);
        }



        /// <summary>
        /// 移除范围内条件相匹配的所有元素;
        /// </summary>
        /// <param name="startIndex">包含此下标的开始下标</param>
        /// <param name="match">返回ture则移除</param>
        public static void RemoveAll<T>(this IList<T> list, int startIndex, Func<T, bool> match)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (match == null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0 || startIndex >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            int index = startIndex;
            while (index < list.Count)
            {
                T item = list[index];
                if (match(item))
                {
                    list.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }



        /// <summary>
        /// 移除合集内相同的元素;
        /// 若存在相同的元素,则保留第一个元素,其余的都移除;
        /// </summary>
        public static void RemoveSame<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            int i = 0;
            while (true)
            {
                T item = list[i];
                i++;
                if (i < list.Count)
                {
                    list.RemoveAll(i, other => item.Equals(other));
                }
                else
                {
                    break;
                }
            }
        }




        /// <summary>
        /// 这两个合集是否拥有相同的内容?尽管顺序和包含个数不同;
        /// </summary>
        public static bool IsSameContent<T>(this ICollection<T> collection, IEnumerable<T> other)
        {
            if (collection == null)
                throw new ArgumentNullException("list");
            if (other == null)
                throw new ArgumentNullException("otherList");

            if (collection == other)
                return true;

            foreach (var item in other)
            {
                if (!collection.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 这两个合集是否拥有相同的内容?尽管顺序和包含个数不同;
        /// </summary>
        public static bool IsSameContent<TKey, TValue>(this IDictionary<TKey, TValue> collection, IEnumerable<KeyValuePair<TKey, TValue>> other)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (other == null)
                throw new ArgumentNullException("other");
            if (collection == other)
                return true;

            foreach (var pair in other)
            {
                TValue value;
                if (collection.TryGetValue(pair.Key, out value))
                {
                    if (pair.Value.Equals(value))
                    {
                        continue;
                    }
                }
                return false;
            }
            return true;
        }


        /// <summary>
        /// 这两个合集返回内容和返回次序是否完全相同?
        /// </summary>
        public static bool IsSame<T>(this IEnumerable<T> collection, IEnumerable<T> other)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (other == null)
                throw new ArgumentNullException("otherList");
            if (collection == other)
                return true;

            IEnumerator<T> otherEnumerator = other.GetEnumerator();
            foreach (var item in collection)
            {
                if (otherEnumerator.MoveNext())
                {
                    if (item.Equals(otherEnumerator.Current))
                    {
                        continue;
                    }
                }
                return false;
            }
            return true;
        }


        /// <summary>
        /// 根据条件转换为数组合集;
        /// </summary>
        public static TResult[] ToArray<T, TResult>(this IReadOnlyCollection<T> collection, Func<T, TResult> func)
        {
            TResult[] array = new TResult[collection.Count];
            int i = 0;
            foreach (var item in collection)
            {
                array[i] = func(item);
                i++;
            }
            return array;
        }

        #region IEnumerable

        /// <summary>
        /// 对每个元素执行指定操作;
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var item in collection)
            {
                action.Invoke(item);
            }
        }

        /// <summary>
        /// 区别于 Linq.Min();返回转换结果最小的元素;
        /// </summary>
        public static TSource MinSource<T, TSource>(this IEnumerable<TSource> collection, Func<TSource, T> selector)
        {
            return MinSource(collection, selector, Comparer<T>.Default);
        }

        /// <summary>
        /// 区别于 Linq.Min();返回转换结果最小的元素;
        /// </summary>
        public static TSource MinSource<T, TSource>(this IEnumerable<TSource> collection, Func<TSource, T> selector, IComparer<T> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            bool isFirst = true;
            TSource minSource = default(TSource);
            T min = default(T);
            foreach (var item in collection)
            {
                if (isFirst)
                {
                    minSource = item;
                    min = selector(item);
                    isFirst = false;
                    continue;
                }

                T value = selector(item);
                if (comparer.Compare(min, value) > 0)
                {
                    minSource = item;
                    min = value;
                }
            }
            return minSource;
        }


        /// <summary>
        /// 区别于 Linq.Min();返回转换结果最大的元素;
        /// </summary>
        public static TSource MaxSource<T, TSource>(this IEnumerable<TSource> collection, Func<TSource, T> selector)
        {
            return MaxSource(collection, selector, Comparer<T>.Default);
        }

        /// <summary>
        /// 区别于 Linq.Min();返回转换结果最大的元素;
        /// </summary>
        public static TSource MaxSource<T, TSource>(this IEnumerable<TSource> collection, Func<TSource, T> selector, IComparer<T> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            bool isFirst = true;
            TSource source = default(TSource);
            T max = default(T);
            foreach (var item in collection)
            {
                if (isFirst)
                {
                    source = item;
                    max = selector(item);
                    isFirst = false;
                    continue;
                }

                T value = selector(item);
                if (comparer.Compare(max, value) < 0)
                {
                    source = item;
                    max = value;
                }
            }
            return source;
        }

        #endregion
    }
}
