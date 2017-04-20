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

}
