using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu.Collections
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

}
