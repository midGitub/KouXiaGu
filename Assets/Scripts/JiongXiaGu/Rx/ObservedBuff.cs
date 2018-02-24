using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace JiongXiaGu
{

    /// <summary>
    /// 用队列缓存观察得到的数据;
    /// </summary>
    public class ObservedBuff<T> : IObserver<T>
    {
        public ConcurrentQueue<T> Queue { get; private set; }
        public event Action OnCompletedEvent;
        public event Action<Exception> OnErrorEvent;

        public ObservedBuff()
        {
            Queue = new ConcurrentQueue<T>();
        }

        public ObservedBuff(IEnumerable<T> collection)
        {
            Queue = new ConcurrentQueue<T>(collection);
        }

        public ObservedBuff(Action onCompleted, Action<Exception> onError)
        {
            OnCompletedEvent = onCompleted;
            OnErrorEvent = onError;
        }

        void IObserver<T>.OnNext(T value)
        {
            Queue.Enqueue(value);
        }

        void IObserver<T>.OnCompleted()
        {
            OnCompletedEvent?.Invoke();
        }

        void IObserver<T>.OnError(Exception error)
        {
            OnErrorEvent?.Invoke(error);
        }
    }
}
