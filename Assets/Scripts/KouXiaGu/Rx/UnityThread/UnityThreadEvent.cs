using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{

    public abstract class UnityThreadEvent : IXiaGuObserver<UnityThreadDispatcher>, IDisposable
    {
        static UnityThreadDispatcher instance
        {
            get { return UnityThreadDispatcher.Instance; }
        }

        IDisposable disposer;

        public bool IsSubscribed
        {
            get { return disposer != null; }
        }

        public abstract void OnNext();
        public void OnError(Exception error) { }
        public void OnCompleted() { }

        public void OnNext(UnityThreadDispatcher none)
        {
            OnNext();
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
            if (IsSubscribed)
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
