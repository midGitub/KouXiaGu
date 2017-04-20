using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    public abstract class UnityThreadEvent : IObserver<object>, IDisposable
    {
        static UnityThreadDispatcher instance
        {
            get { return UnityThreadDispatcher.Instance; }
        }


        public UnityThreadEvent()
        {
        }

        IDisposable disposer;

        public bool IsSubscribed
        {
            get { return disposer != null; }
        }

        public abstract void OnNext();

        public void OnNext(object none)
        {
            OnNext();
        }

        public void OnCompleted()
        {
            Dispose();
        }

        public void OnError(Exception error)
        {
            Debug.LogError(new NotImplementedException());
        }


        public IDisposable SubscribeUpdate()
        {
            if (IsSubscribed)
                throw new ArgumentException();

            disposer = instance.SubscribeUpdate(this);
            return this;
        }

        public IDisposable SubscribeFixedUpdate()
        {
            if (IsSubscribed)
                throw new ArgumentException();

            disposer = instance.SubscribeFixedUpdate(this);
            return this;
        }

        public void Dispose()
        {
            if (disposer != null)
            {
                disposer.Dispose();
                disposer = null;
            }
        }

    }


    public class UnityThreadDelegate : UnityThreadEvent
    {
        public UnityThreadDelegate(Action onNext, Action<Exception> onError)
        {
            if (onNext == null)
                throw new ArgumentNullException("action");

            this.onNext = onNext;
            this.onError = onError;
        }

        Action onNext;
        Action<Exception> onError;

        public override void OnNext()
        {
            onNext();
        }

    }

}
