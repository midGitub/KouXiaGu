using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public interface IObservableDictionary<TKey, TValue>
    {
        IDisposable Subscribe(IDictionaryObserver<TKey, TValue> observer);
    }

    public interface IDictionaryObserver<TKey, TValue>
    {
        void OnAdded(TKey key, TValue newValue);
        void OnRemoved(TKey key, TValue originalValue);
        void OnUpdated(TKey key, TValue originalValue, TValue newValue);
    }

    public interface IObservableSet<T>
    {

    }

    public interface ISetObserver<T>
    {

    }

}
