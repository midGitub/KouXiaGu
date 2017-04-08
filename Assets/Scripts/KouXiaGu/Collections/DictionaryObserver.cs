using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu.Collections
{

    public abstract class DictionaryObserver<TKey, TValue> : IObserver<DictionaryChange<TKey, TValue>>
    {

        IDisposable unsubscriber;

        public bool IsSubscribed
        {
            get { return unsubscriber != null; }
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
                    Add(value.Key, value.NewValue);
                    break;
                case Operation.Remove:
                    Remove(value.Key, value.OriginalValue);
                    break;
                case Operation.Update:
                    Update(value.Key, value.OriginalValue, value.NewValue);
                    break;
            }
        }

        protected abstract void Add(TKey key, TValue newValue);
        protected abstract void Remove(TKey key, TValue originalValue);
        protected abstract void Update(TKey key, TValue originalValue, TValue newValue);

        public void Subscribe(IObservable<DictionaryChange<TKey, TValue>> provider)
        {
            if (IsSubscribed)
                throw new ArgumentException("已经存在监视内容;");
            if (provider == null)
                throw new ArgumentNullException();

            unsubscriber = provider.Subscribe(this);
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
