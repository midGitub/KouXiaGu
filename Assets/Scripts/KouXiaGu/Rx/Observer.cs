using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{

    /// <summary>
    /// 简化的观察者结构;
    /// </summary>
    public abstract class Observer<T> : IXiaGuObserver<T>
    {

        public Observer()
        {
        }


        IDisposable unsubscriber;


        public void Subscribe(IXiaGuObservable<T> observable)
        {
            if (unsubscriber != null)
                throw new ArgumentException("已订阅");

            unsubscriber = observable.Subscribe(this);
        }

        public void Unsubscribe()
        {
            if (unsubscriber != null)
            {
                unsubscriber.Dispose();
                unsubscriber = null;
            }
        }


        public virtual void OnCompleted()
        {
            Unsubscribe();
        }

        public virtual void OnError(Exception error)
        {
            return;
        }

        public abstract void OnNext(T item);

    }

}
