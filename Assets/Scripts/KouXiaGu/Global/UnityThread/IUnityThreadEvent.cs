using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public interface IUnityThreadEvent
    {
        void OnNext();
        void OnError(Exception ex);
    }

    public abstract class UnityDispatcher : IUnityThreadEvent, IDisposable
    {
        static UnityThreadDispatcher instance
        {
            get { return UnityThreadDispatcher.Instance; }
        }

        IDisposable disposer;

        public abstract void OnNext();
        public abstract void OnError(Exception ex);

        public IDisposable SubscribeUpdate()
        {
            disposer = instance.SubscribeUpdate(this);
            return this;
        }

        public IDisposable SubscribeFixedUpdate()
        {
            disposer = instance.SubscribeFixedUpdate(this);
            return this;
        }

        public void Dispose()
        {
            disposer.Dispose();
        }
    }


    public class UnityThreadDelegate : UnityDispatcher
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

        public override void OnError(Exception ex)
        {
            onError(ex);
        }
    }

}
