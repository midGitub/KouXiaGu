using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    //public interface IReadOnlyCollection<T> : IEnumerable<T>, IEnumerable
    //{
    //    int Count { get; }
    //}

    //public interface IReadOnlyList<T> : IReadOnlyCollection<T>, IEnumerable<T>
    //{
    //    T this[int index] { get; }
    //}

    //public interface IReadOnlyDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    //{
    //    TValue this[TKey key] { get; }
    //    IEnumerable<TKey> Keys { get; }
    //    IEnumerable<TValue> Values { get; }

    //    bool ContainsKey(TKey key);
    //    bool TryGetValue(TKey key, out TValue value);
    //}


    /// <summary>
    /// 解决在 .Net4.0 以下版本没有只读接口;
    /// </summary>
    public static class ReadOnlyExtensions
    {
        /// <summary>
        /// 转换到只读接口;
        /// </summary>
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this ICollection<T> collection)
        {
            IReadOnlyCollection<T> readOnly = collection as IReadOnlyCollection<T>;
            if (readOnly == null)
            {
                readOnly = new ReadOnlyCollection<T>(collection);
            }
            return readOnly;
        }

        class ReadOnlyCollection<T> : IReadOnlyCollection<T>
        {
            public ReadOnlyCollection(ICollection<T> collection)
            {
                this.collection = collection;
            }

            protected readonly ICollection<T> collection;

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

        /// <summary>
        /// 转换到只读接口;
        /// </summary>
        public static IReadOnlyCollection<TResult> AsReadOnlyCollection<TSource, TResult>(this ICollection<TSource> collection, Func<TSource, TResult> selector)
        {
            return new ReadOnlyCollection<TSource, TResult>(collection, selector);
        }

        class ReadOnlyCollection<TSource, TResult> : IReadOnlyCollection<TResult>
        {
            public ReadOnlyCollection(ICollection<TSource> collection, Func<TSource, TResult> selector)
            {
                this.collection = collection;
                this.selector = selector;
            }

            protected readonly ICollection<TSource> collection;
            protected readonly Func<TSource, TResult> selector;

            public int Count
            {
                get { return collection.Count; }
            }

            public IEnumerator<TResult> GetEnumerator()
            {
                return collection.Select(selector).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }




        /// <summary>
        /// 转换到只读接口;
        /// </summary>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IList<T> list)
        {
            IReadOnlyList<T> readOnly = list as IReadOnlyList<T>;
            if (readOnly == null)
            {
                readOnly = new ReadOnlyList<T>(list);
            }
            return readOnly;
        }

        class ReadOnlyList<T> : ReadOnlyCollection<T>, IReadOnlyList<T>
        {
            public ReadOnlyList(IList<T> list) : base(list)
            {
                this.list = list;
            }

            readonly IList<T> list;

            public T this[int index]
            {
                get { return list[index]; }
            }
        }

        /// <summary>
        /// 转换到只读接口;
        /// </summary>
        public static IReadOnlyList<TResult> AsReadOnlyList<TSource, TResult>(this IList<TSource> collection, Func<TSource, TResult> selector)
        {
            return new ReadOnlyList<TSource, TResult>(collection, selector);
        }

        class ReadOnlyList<TSource, TResult> : ReadOnlyCollection<TSource, TResult>, IReadOnlyList<TResult>
        {
            public ReadOnlyList(IList<TSource> list, Func<TSource, TResult> selector) : base(list, selector)
            {
                this.list = list;
            }

            protected readonly IList<TSource> list;

            public TResult this[int index]
            {
                get { return selector(list[index]); }
            }
        }





        /// <summary>
        /// 转换到只读接口;
        /// </summary>
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var readOnly = dictionary as IReadOnlyDictionary<TKey, TValue>;
            if (readOnly == null)
            {
                readOnly = new ReadOnlyDictionary<TKey, TValue>(dictionary);
            }
            return readOnly;
        }

        class ReadOnlyDictionary<TKey, TValue> : ReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>
        {
            public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
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

        /// <summary>
        /// 转换到只读接口;
        /// </summary>
        public static IReadOnlyDictionary<TKey, TResult> AsReadOnlyDictionary<TKey, TSource, TResult>(this IDictionary<TKey, TSource> dictionary, Func<TSource, TResult> selector)
        {
            return new ReadOnlyDictionary<TKey, TSource, TResult>(dictionary, selector);
        }

        class ReadOnlyDictionary<TKey, TSource, TResult> : IReadOnlyDictionary<TKey, TResult>
        {
            public ReadOnlyDictionary(IDictionary<TKey, TSource> dictionary, Func<TSource, TResult> selector)
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
