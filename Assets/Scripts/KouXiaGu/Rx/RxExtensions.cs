using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{

    abstract class ObservableBase<T> : IObservable<T>, IObserver<T>
    {
        public ObservableBase(IObservable<T> parent)
        {
            this.parent = parent;
            parent.Subscribe(this);
            tracker = new LinkedListTracker<T>();
        }

        IObservable<T> parent;
        LinkedListTracker<T> tracker;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return ((IObservable<T>)this.tracker).Subscribe(observer);
        }

        public virtual void OnNext(T item)
        {
            tracker.Track(item);
        }

        public virtual void OnCompleted()
        {
            tracker.TrackCompleted();
        }

        public virtual void OnError(Exception error)
        {
            tracker.TrackError(error);
        }
    }

    class WhereObservable<T> : ObservableBase<T>
    {
        public WhereObservable(IObservable<T> parent) : base(parent)
        {
        }

        public override void OnNext(T item)
        {
            throw new NotImplementedException();
        }
    }

    public static class RxExtensions
    {
        public static IDisposable SubscribUpdate(this IObserver<object> observer)
        {
            return UnityThreadDispatcher.Instance.SubscribeUpdate(observer);
        }

        public static IDisposable SubscribeFixedUpdate(this IObserver<object> observer)
        {
            return UnityThreadDispatcher.Instance.SubscribeFixedUpdate(observer);
        }

        public static IObservable<T> Where<T>(this IObservable<T> observable, Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

    }

}
