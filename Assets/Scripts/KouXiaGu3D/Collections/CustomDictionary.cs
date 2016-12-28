using System.Collections.Generic;

namespace KouXiaGu.Collections
{

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
