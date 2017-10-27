using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace JiongXiaGu
{

    /// <summary>
    /// 观察者事件缓存(线程安全);
    /// </summary>
    public class ObserverEventBuffer<T> : IObserver<T>
    {
        private readonly ConcurrentQueue<T> eventQueue;

        public ObserverEventBuffer()
        {
            eventQueue = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// 尝试获取到最先传入的事件,并且移除该事件;
        /// </summary>
        public bool TryDequeue(out T value)
        {
            return eventQueue.TryDequeue(out value);
        }

        void IObserver<T>.OnNext(T value)
        {
            eventQueue.Enqueue(value);
        }

        void IObserver<T>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        void IObserver<T>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }
    }

    ///// <summary>
    ///// 观察字典变化,并且记录变化内容;
    ///// </summary>
    //public class DictionaryChangedBuffer<TKey, TValue> : IObserver<DictionaryEvent<TKey, TValue>>
    //{
    //    private readonly Queue<DictionaryEvent<TKey, TValue>> queue;

    //    void IObserver<DictionaryEvent<TKey, TValue>>.OnCompleted()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    void IObserver<DictionaryEvent<TKey, TValue>>.OnError(Exception error)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    void IObserver<DictionaryEvent<TKey, TValue>>.OnNext(DictionaryEvent<TKey, TValue> value)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    ///// <summary>
    ///// 观察字典内容变化,并且记录发生变化的Key;
    ///// </summary>
    //public class DictionaryKeyChangedBuffer<TKey, TValue> : IObserver<DictionaryEvent<TKey, TValue>>
    //{
    //    public DictionaryKeyChangedBuffer(ICollection<TKey> changedPositions)
    //    {
    //        ChangedPositions = changedPositions;
    //    }

    //    public ICollection<TKey> ChangedPositions { get; private set; }

    //    void IObserver<DictionaryEvent<TKey, TValue>>.OnNext(DictionaryEvent<TKey, TValue> value)
    //    {
    //        ChangedPositions.Add(value.Key);
    //    }

    //    void IObserver<DictionaryEvent<TKey, TValue>>.OnCompleted()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    void IObserver<DictionaryEvent<TKey, TValue>>.OnError(Exception error)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
