using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JiongXiaGu.Concurrent
{

    /// <summary>
    /// 对 IDictionary 引用添加线程读写锁;
    /// </summary>
    public class ConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        public ConcurrentDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
            editorLock = new ReaderWriterLockSlim();
        }

        readonly IDictionary<TKey, TValue> dictionary;
        readonly ReaderWriterLockSlim editorLock;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return Keys; }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return Values; }
        }

        public ICollection<TKey> Keys
        {
            get { throw new InvalidOperationException("该合集不允许迭代;"); }
        }

        public ICollection<TValue> Values
        {
            get { throw new InvalidOperationException("该合集不允许迭代;"); }
        }

        public int Count
        {
            get
            {
                editorLock.EnterReadLock();
                try
                {
                    return dictionary.Count;
                }
                finally
                {
                    editorLock.ExitReadLock();
                }
            }
        }

        public bool IsReadOnly
        {
            get { return editorLock.IsWriteLockHeld; }
        }

        public ReaderWriterLockSlim EditorLock
        {
            get { return editorLock; }
        }

        public TValue this[TKey key]
        {
            get
            {
                editorLock.EnterReadLock();
                try
                {
                    return dictionary[key];
                }
                finally
                {
                    editorLock.ExitReadLock();
                }
            }
            set
            {
                editorLock.EnterWriteLock();
                try
                {
                    dictionary[key] = value;
                }
                finally
                {
                    editorLock.ExitWriteLock();
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            editorLock.EnterWriteLock();
            try
            {
                dictionary.Add(item);
            }
            finally
            {
                editorLock.ExitWriteLock();
            }
        }

        public void Add(TKey key, TValue value)
        {
            editorLock.EnterWriteLock();
            try
            {
                dictionary.Add(key, value);
            }
            finally
            {
                editorLock.ExitWriteLock();
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            editorLock.EnterWriteLock();
            try
            {
                return dictionary.Remove(item);
            }
            finally
            {
                editorLock.ExitWriteLock();
            }
        }

        public bool Remove(TKey key)
        {
            editorLock.EnterWriteLock();
            try
            {
                return dictionary.Remove(key);
            }
            finally
            {
                editorLock.ExitWriteLock();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            editorLock.EnterReadLock();
            try
            {
                return dictionary.Contains(item);
            }
            finally
            {
                editorLock.ExitReadLock();
            }
        }

        public bool ContainsKey(TKey key)
        {
            editorLock.EnterReadLock();
            try
            {
                return dictionary.ContainsKey(key);
            }
            finally
            {
                editorLock.ExitReadLock();
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            editorLock.EnterReadLock();
            try
            {
                return dictionary.TryGetValue(key, out value);
            }
            finally
            {
                editorLock.ExitReadLock();
            }
        }

        public void Clear()
        {
            editorLock.EnterWriteLock();
            try
            {
                dictionary.Clear();
            }
            finally
            {
                editorLock.ExitWriteLock();
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            editorLock.EnterReadLock();
            try
            {
                dictionary.CopyTo(array, arrayIndex);
            }
            finally
            {
                editorLock.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new InvalidOperationException("此合集不允许迭代;");
        }
    }
}
