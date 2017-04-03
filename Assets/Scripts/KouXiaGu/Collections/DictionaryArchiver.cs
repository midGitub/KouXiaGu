using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using UniRx;

namespace KouXiaGu.Collections
{

    /// <summary>
    /// 监视字典结构,对存在变化的内容进行记录;
    /// </summary>
    [ProtoContract]
    public class DictionaryArchiver<TKey, TValue> : Dictionary<TKey, TValue>, IObserver<DictionaryChange<TKey, TValue>>
    {

        IDisposable unsubscriber;

        public DictionaryArchiver() : base()
        {
        }

        public DictionaryArchiver(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public DictionaryArchiver(int capacity) : base(capacity)
        {
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
            Add(value.Key, value.NewValue);
        }

        void Remove(DictionaryChange<TKey, TValue> value)
        {
            Remove(value.Key);
        }

        void Update(DictionaryChange<TKey, TValue> value)
        {
            this.AddOrUpdate(value.Key, value.NewValue);
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
            if (unsubscriber != null)
            {
                unsubscriber.Dispose();
                unsubscriber = null;
            }
        }

    }

}
