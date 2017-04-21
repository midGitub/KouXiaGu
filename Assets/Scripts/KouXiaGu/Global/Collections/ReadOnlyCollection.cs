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
            return collection.GetEnumerator();
        }
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

    public static class ReadOnlyExtensions
    {
        /// <summary>
        /// 转转换到只读接口;
        /// </summary>
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this ICollection<T> collection)
        {
            return new _ReadOnlyCollection<T>(collection);
        }

        /// <summary>
        /// 转换到只读接口;
        /// </summary>
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new _ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }

}
