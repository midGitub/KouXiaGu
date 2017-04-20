using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public abstract class DictionaryObserver<TKey, TValue> : IDictionaryObserver<TKey, TValue>
    {

        IDisposable unsubscriber;

        public bool IsSubscribed
        {
            get { return unsubscriber != null; }
        }

        public abstract void OnAdded(TKey key, TValue newValue);
        public abstract void OnRemoved(TKey key, TValue originalValue);
        public abstract void OnUpdated(TKey key, TValue originalValue, TValue newValue);

        public void Subscribe(IObservableDictionary<TKey, TValue> provider)
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
