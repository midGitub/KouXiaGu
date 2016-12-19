using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 监视字典结构,对存在变化的内容进行记录;
    /// </summary>
    [ProtoContract]
    public class DictionaryArchiver<TKey, TValue> : IDictionary<TKey, TValue>, IObserver<DictionaryChange<TKey, TValue>>
    {

        Dictionary<TKey, TValue> archiveContent;

        IDisposable unsubscriber;

        public TValue this[TKey key]
        {
            get { return this.archiveContent[key]; }
            set { this.archiveContent[key] = value; }
        }

        public int Count
        {
            get { return this.archiveContent.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary<TKey, TValue>)this.archiveContent).IsReadOnly; }
        }

        public ICollection<TKey> Keys
        {
            get { return this.archiveContent.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return this.archiveContent.Values; }
        }


        public DictionaryArchiver()
        {
            this.archiveContent = new Dictionary<TKey, TValue>();
        }

        public DictionaryArchiver(IDictionary<TKey, TValue> dictionary)
        {
            this.archiveContent = new Dictionary<TKey, TValue>(dictionary);
        }

        public DictionaryArchiver(int capacity)
        {
            this.archiveContent = new Dictionary<TKey, TValue>(capacity);
        }


        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            ((IDictionary<TKey, TValue>)this.archiveContent).Add(key, value);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)this.archiveContent).Remove(item);
        }

        public bool Remove(TKey key)
        {
            return this.archiveContent.Remove(key);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.archiveContent.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return this.archiveContent.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.archiveContent.TryGetValue(key, out value);
        }

        public void Clear()
        {
            this.archiveContent.Clear();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)this.archiveContent).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.archiveContent.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.archiveContent.GetEnumerator();
        }


        void IObserver<DictionaryChange<TKey, TValue>>.OnCompleted()
        {
            Unsubscribe();
        }

        void IObserver<DictionaryChange<TKey, TValue>>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        void IObserver<DictionaryChange<TKey, TValue>>.OnNext(DictionaryChange<TKey, TValue> value)
        {
            switch (value.Operation)
            {
                case Operation.Add:
                    Add(value);
                    break;
                case Operation.Remove:
                    Remove(value);
                    break;
                case Operation.Update:
                    Update(value);
                    break;
            }
        }

        void Add(DictionaryChange<TKey, TValue> value)
        {
            archiveContent.Add(value.Key, value.NewValue);
        }

        void Remove(DictionaryChange<TKey, TValue> value)
        {
            archiveContent.Remove(value.Key);
        }

        void Update(DictionaryChange<TKey, TValue> value)
        {
            archiveContent.AddOrUpdate(value.Key, value.NewValue);
        }

        public void Subscribe(IObservable<DictionaryChange<TKey, TValue>> provider)
        {
            if (this.unsubscriber != null)
                throw new ArgumentException("已经存在监视内容;");
            if (provider == null)
                throw new ArgumentNullException();

            this.unsubscriber = provider.Subscribe(this);
        }

        public void Unsubscribe()
        {
            unsubscriber.Dispose();
        }

    }

}
