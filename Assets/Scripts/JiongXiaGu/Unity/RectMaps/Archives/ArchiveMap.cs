using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 
    /// </summary>
    public class ArchiveMap<TKey, TValue> : IObserver<DictionaryEvent<TKey, TValue>>
    {


        void IObserver<DictionaryEvent<TKey, TValue>>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        void IObserver<DictionaryEvent<TKey, TValue>>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        void IObserver<DictionaryEvent<TKey, TValue>>.OnNext(DictionaryEvent<TKey, TValue> value)
        {
            throw new NotImplementedException();
        }
    }
}
