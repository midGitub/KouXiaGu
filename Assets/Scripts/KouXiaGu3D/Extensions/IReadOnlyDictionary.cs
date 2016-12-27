using System;
using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu
{

    /// <summary>
    /// 表示键/值对的泛型只读集合。
    /// </summary>
    public interface IReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        int Count { get; }
        TValue this[TKey key] { get; }
        IEnumerable<TKey> Keys { get; }
        IEnumerable<TValue> Values { get; }
        bool ContainsKey(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }

    /// <summary>
    /// 实现IReadOnlyDictionary接口的字典结构;
    /// </summary>
    public class CustomDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        public CustomDictionary() : base() { }
        public CustomDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public CustomDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        public CustomDictionary(int capacity) : base(capacity) { }
        public CustomDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }
        public CustomDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return base.Keys; }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return base.Values; }
        }

    }


}
