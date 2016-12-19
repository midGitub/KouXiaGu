using System.Collections.Generic;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    public interface IObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
        IObservable<DictionaryChange<TKey, TValue>>
    {
        bool Contains(IObserver<DictionaryChange<TKey, TValue>> observer);
        void EndTransmission();
    }

}
