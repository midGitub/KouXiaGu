using System;
using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 基于链表的键值结构;查询O(n),加入O(n),移除O(n);
    /// </summary>
    public class List<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        private int version = 0;
        private readonly Lazy<KeyCollection> keyCollection;
        private readonly Lazy<ValueCollection> valueCollection;
        private readonly List<Entry> entries;
        private readonly IEqualityComparer<TKey> comparer;

        public int Count => entries.Count;
        public ICollection<TKey> Keys => keyCollection.Value;
        public ICollection<TValue> Values => valueCollection.Value;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => keyCollection.Value;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => valueCollection.Value;
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
        public IEqualityComparer<TKey> Comparer => comparer;

        public TValue this[TKey key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                SetValue(key, value);
            }
        }

        public List()
        {
            keyCollection = new Lazy<KeyCollection>(() => new KeyCollection(this));
            valueCollection = new Lazy<ValueCollection>(() => new ValueCollection(this));
            entries = new List<Entry>();
            comparer = EqualityComparer<TKey>.Default;
        }

        public TValue GetValue(TKey key)
        {
            int index = FindIndexEntry(key);
            if (index >= 0)
            {
                var entry = entries[index];
                return entry.Value;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void SetValue(TKey key, TValue value)
        {
            int index = FindIndexEntry(key);
            entries[index] = new Entry(key, value);
            version++;
        }

        public void Add(TKey key, TValue value)
        {
            int index = FindIndexEntry(key);
            if (index >= 0)
            {
                throw new ArgumentException();
            }
            else
            {
                Entry entry = new Entry(key, value);
                entries.Add(entry);
                version++;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Remove(TKey key)
        {
            int index = FindIndexEntry(key);
            if (index >= 0)
            {
                entries.RemoveAt(index);
                version++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            int index = FindIndexEntry(item.Key);
            if (index >= 0)
            {
                Entry entry = entries[index];
                if (EqualityComparer<TValue>.Default.Equals(entry.Value, item.Value))
                {
                    entries.RemoveAt(index);
                    version++;
                    return true;
                }
            }
            return false;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int index = FindIndexEntry(item.Key);
            if (index >= 0)
            {
                Entry entry = entries[index];
                if (EqualityComparer<TValue>.Default.Equals(entry.Value, item.Value))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            int index = FindIndexEntry(key);
            return index >= 0;
        }

        private bool ContainsValue(TValue value)
        {
            int index = entries.FindIndex(entry => EqualityComparer<TValue>.Default.Equals(entry.Value, value));
            return index >= 0;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = FindIndexEntry(key);
            if (index >= 0)
            {
                value = entries[index].Value;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        public void Clear()
        {
            entries.Clear();
            version++;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (index < 0 || index > array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (array.Length - index < Count)
                throw new ArgumentException("数组太小;");

            for (int i = 0; i < Count; i++)
            {
                var entry = entries[i];
                array[index++] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }

        private int FindIndexEntry(TKey key)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                Entry entry = entries[i];
                if (comparer.Equals(entry.Key, key))
                {
                    return i;
                }
            }
            return -1;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private struct Entry
        {
            public TKey Key;
            public TValue Value;

            public Entry(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private List<TKey, TValue> dictionary;
            private int version;
            private int index;
            private KeyValuePair<TKey, TValue> current;

            public KeyValuePair<TKey, TValue> Current => current;
            object IEnumerator.Current => current;

            internal Enumerator(List<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
                version = dictionary.version;
                index = 0;
                current = new KeyValuePair<TKey, TValue>();
            }

            public bool MoveNext()
            {
                if (version != dictionary.version)
                    throw new InvalidOperationException();

                if ((uint)index < (uint)dictionary.Count)
                {
                    var entry = dictionary.entries[index];
                    current = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                    index++;
                    return true;
                }
                else
                {
                    index = dictionary.Count + 1;
                    current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                if (version != dictionary.version)
                    throw new InvalidOperationException();

                index = 0;
                current = default(KeyValuePair<TKey, TValue>);
            }
        }

        public sealed class KeyCollection : ICollection<TKey>, IReadOnlyCollection<TKey>
        {
            private List<TKey, TValue> dictionary;

            public int Count => dictionary.Count;
            bool ICollection<TKey>.IsReadOnly => true;

            public KeyCollection(List<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                    throw new ArgumentNullException(nameof(dictionary));

                this.dictionary = dictionary;
            }

            public void CopyTo(TKey[] array, int index)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (index < 0 || index > array.Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (array.Length - index < dictionary.Count)
                    throw new ArgumentException("数组太小;");

                int count = dictionary.Count;
                var entries = dictionary.entries;
                for (int i = 0; i < count; i++)
                {
                    array[index++] = entries[i].Key;
                }
            }

            void ICollection<TKey>.Add(TKey item)
            {
                throw new NotSupportedException();
            }

            void ICollection<TKey>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<TKey>.Contains(TKey item)
            {
                return dictionary.ContainsKey(item);
            }

            bool ICollection<TKey>.Remove(TKey item)
            {
                throw new NotSupportedException();
            }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                return new Enumerator(dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(dictionary);
            }

            [Serializable]
            public struct Enumerator : IEnumerator<TKey>
            {
                private List<TKey, TValue> dictionary;
                private int index;
                private int version;
                private TKey currentKey;

                public TKey Current => currentKey;
                object IEnumerator.Current => currentKey;

                internal Enumerator(List<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    version = dictionary.version;
                    index = 0;
                    currentKey = default(TKey);
                }

                public void Dispose()
                {
                    return;
                }

                public bool MoveNext()
                {
                    if (version != dictionary.version)
                    {
                        throw new InvalidOperationException();
                    }

                    if (index < dictionary.Count)
                    {
                        currentKey = dictionary.entries[index].Key;
                        index++;
                        return true;
                    }
                    else
                    {
                        index = dictionary.Count + 1;
                        currentKey = default(TKey);
                        return false;
                    }
                }

                public void Reset()
                {
                    if (version != dictionary.version)
                    {
                        throw new InvalidOperationException();
                    }

                    index = 0;
                    currentKey = default(TKey);
                }
            }
        }

        public sealed class ValueCollection : ICollection<TValue>, IReadOnlyCollection<TValue>
        {
            private List<TKey, TValue> dictionary;

            public int Count => dictionary.Count;
            bool ICollection<TValue>.IsReadOnly => true;

            public ValueCollection(List<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                    throw new ArgumentNullException(nameof(dictionary));

                this.dictionary = dictionary;
            }

            public void CopyTo(TValue[] array, int index)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (index < 0 || index > array.Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (array.Length - index < dictionary.Count)
                    throw new ArgumentException("数组太小;");

                int count = dictionary.Count;
                var entries = dictionary.entries;
                for (int i = 0; i < count; i++)
                {
                    array[index++] = entries[i].Value;
                }
            }

            void ICollection<TValue>.Add(TValue item)
            {
                throw new NotSupportedException();
            }

            void ICollection<TValue>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<TValue>.Contains(TValue item)
            {
                return dictionary.ContainsValue(item);
            }

            bool ICollection<TValue>.Remove(TValue item)
            {
                throw new NotSupportedException();
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return new Enumerator(dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(dictionary);
            }

            [Serializable]
            public struct Enumerator : IEnumerator<TValue>
            {
                private List<TKey, TValue> dictionary;
                private int index;
                private int version;
                private TValue currentKey;

                public TValue Current => currentKey;
                object IEnumerator.Current => currentKey;

                internal Enumerator(List<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    version = dictionary.version;
                    index = 0;
                    currentKey = default(TValue);
                }

                public void Dispose()
                {
                    return;
                }

                public bool MoveNext()
                {
                    if (version != dictionary.version)
                    {
                        throw new InvalidOperationException();
                    }

                    if (index < dictionary.Count)
                    {
                        currentKey = dictionary.entries[index].Value;
                        index++;
                        return true;
                    }
                    else
                    {
                        index = dictionary.Count + 1;
                        currentKey = default(TValue);
                        return false;
                    }
                }

                public void Reset()
                {
                    if (version != dictionary.version)
                    {
                        throw new InvalidOperationException();
                    }

                    index = 0;
                    currentKey = default(TValue);
                }
            }
        }
    }
}
