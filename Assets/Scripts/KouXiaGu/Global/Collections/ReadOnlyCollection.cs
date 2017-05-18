using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{


    public interface IReadOnlyCollection<T> : IEnumerable<T>, IEnumerable
    {
        int Count { get; }
    }

    public interface IReadOnlyDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    {
        TValue this[TKey key] { get; }
        IEnumerable<TKey> Keys { get; }
        IEnumerable<TValue> Values { get; }

        bool ContainsKey(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }

    public static class ReadOnlyExtensions
    {

        /// <summary>
        /// 转转换到只读接口;
        /// </summary>
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this ICollection<T> collection)
        {
            return new _ReadOnlyCollection<T>(collection);
        }

        class _ReadOnlyCollection<T> : IReadOnlyCollection<T>
        {
            public _ReadOnlyCollection(ICollection<T> collection)
            {
                this.collection = collection;
            }

            ICollection<T> collection;

            public int Count
            {
                get { return collection.Count; }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return collection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        public static IReadOnlyCollection<TResult> AsReadOnlyCollection<TSource, TResult>(this ICollection<TSource> collection, Func<TSource, TResult> selector)
        {
            return new _ReadOnlyCollection<TSource, TResult>(collection, selector);
        }

        class _ReadOnlyCollection<TSource, TResult> : IReadOnlyCollection<TResult>
        {
            public _ReadOnlyCollection(ICollection<TSource> collection, Func<TSource, TResult> selector)
            {
                this.collection = collection;
                this.selector = selector;
            }

            readonly ICollection<TSource> collection;
            readonly Func<TSource, TResult> selector;

            public int Count
            {
                get { return collection.Count; }
            }

            public IEnumerator<TResult> GetEnumerator()
            {
                return collection.Cast<TResult>().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        /// <summary>
        /// 转换到只读接口;
        /// </summary>
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new _ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        class _ReadOnlyDictionary<TKey, TValue> : _ReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>
        {
            public _ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
                : base(dictionary)
            {
                this.dictionary = dictionary;
            }

            IDictionary<TKey, TValue> dictionary;

            public IEnumerable<TKey> Keys
            {
                get { return dictionary.Keys; }
            }

            public IEnumerable<TValue> Values
            {
                get { return dictionary.Values; }
            }

            public TValue this[TKey key]
            {
                get { return dictionary[key]; }
            }

            public bool ContainsKey(TKey key)
            {
                return dictionary.ContainsKey(key);
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                return dictionary.TryGetValue(key, out value);
            }

        }


        public static IReadOnlyDictionary<TKey, TResult> AsReadOnlyDictionary<TKey, TSource, TResult>(this IDictionary<TKey, TSource> collection, Func<TSource, TResult> selector)
        {
            return new _ReadOnlyDictionary<TKey, TSource, TResult>(collection, selector);
        }

        class _ReadOnlyDictionary<TKey, TSource, TResult> : IReadOnlyDictionary<TKey, TResult>
        {
            public _ReadOnlyDictionary(IDictionary<TKey, TSource> dictionary, Func<TSource, TResult> selector)
            {
                if (dictionary == null)
                    throw new ArgumentNullException("dictionary");
                if (selector == null)
                    throw new ArgumentNullException("selector");

                this.dictionary = dictionary;
                this.selector = selector;
            }

            readonly IDictionary<TKey, TSource> dictionary;
            readonly Func<TSource, TResult> selector;

            public TResult this[TKey key]
            {
                get
                {
                    TSource source = dictionary[key];
                    TResult result = selector(source);
                    return result;
                }
            }

            public IEnumerable<TKey> Keys
            {
                get
                {
                    return dictionary.Keys;
                }
            }

            public IEnumerable<TResult> Values
            {
                get { return dictionary.Values.Select(item => selector(item)); }
            }

            public int Count
            {
                get { return dictionary.Count; }
            }

            public bool ContainsKey(TKey key)
            {
                return dictionary.ContainsKey(key);
            }

            public bool TryGetValue(TKey key, out TResult value)
            {
                TSource source;
                if (dictionary.TryGetValue(key, out source))
                {
                    value = selector(source);
                    return true;
                }
                else
                {
                    value = default(TResult);
                    return false;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<KeyValuePair<TKey, TResult>> GetEnumerator()
            {
                return dictionary.Select(delegate (KeyValuePair<TKey, TSource> item)
                {
                    TResult result = selector(item.Value);
                    return new KeyValuePair<TKey, TResult>(item.Key, result);
                }).GetEnumerator();
            }
        }
    }
}
