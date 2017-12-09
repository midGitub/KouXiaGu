using System;
using System.Collections.Generic;

namespace JiongXiaGu
{

    /// <summary>
    /// 用队列缓存观察得到的数据;
    /// </summary>
    public class ObservedBuff<T> : Queue<T>, IObserver<T>
    {
        public event Action OnCompletedEvent;
        public event Action<Exception> OnErrorEvent;

        public ObservedBuff()
        {
        }

        public ObservedBuff(Action onCompleted, Action<Exception> onError)
        {
            OnCompletedEvent = onCompleted;
            OnErrorEvent = onError;
        }

        void IObserver<T>.OnNext(T value)
        {
            Enqueue(value);
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
