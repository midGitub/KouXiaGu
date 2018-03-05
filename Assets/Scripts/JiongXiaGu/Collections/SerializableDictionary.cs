using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.Collections
{

    public struct SerializableKeyValuePair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public SerializableKeyValuePair(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Key = keyValuePair.Key;
            Value = keyValuePair.Value;
        }

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// 对 Dictionary 结构进行包装,用于提供序列化;
    /// </summary>
    public class SerializableDictionary<TKey, TValue> : IEnumerable<SerializableKeyValuePair<TKey, TValue>>
    {
        public Dictionary<TKey, TValue> Dictionary { get; private set; }

        public SerializableDictionary()
        {
            Dictionary = new Dictionary<TKey, TValue>();
        }

        public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            Dictionary = dictionary;
        }

        public void Add(TKey key, TValue value)
        {
            Dictionary.Add(key, value);
        }

        /// <summary>
        /// 提供序列化的加入方法;
        /// </summary>
        public void Add(SerializableKeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public IEnumerator<SerializableKeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Dictionary != null ? Dictionary.Select(item => new SerializableKeyValuePair<TKey, TValue>(item)).GetEnumerator() : EmptyCollection<SerializableKeyValuePair<TKey, TValue>>.Default.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
