using System;
using System.Collections.Generic;
using UniRx;

namespace KouXiaGu.Collections
{

    [Obsolete]
    public interface OIObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
        IObservable<DictionaryChange<TKey, TValue>>
    {
        bool Contains(IObserver<DictionaryChange<TKey, TValue>> observer);
        void EndTransmission();
    }

}
