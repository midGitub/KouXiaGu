using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{


    abstract class TrackeNode<T> : IObservable<T>, IObserver<T>
    {
        public TrackeNode(IObservable<T> parent)
        {

        }

        ListTracker<T> tracker;
        public IObservable<T> Parent { get; private set; }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }

        public void OnNext(T item)
        {
            tracker.Track(item);
        }

        public void OnError(Exception error)
        {
            tracker.TrackError(error);
        }

        public void OnCompleted()
        {
            tracker.TrackCompleted();
        }
    }


    public static class ObservableExtensions
    {

        public static IObservable<TRetrun> AsObservable<TRetrun>(this object item, Func<TRetrun> getValue)
        {
            return new Tracker<TRetrun>(getValue);
        }

        public static IObservable<TRetrun> AsObservable<TSource, TRetrun>(this TSource item, Func<TSource, TRetrun> getValue)
        {
            return new Tracker<TRetrun>(() => getValue(item));
        }

        class Tracker<T> : ListTracker<T>, IObservable<T>
        {
            public Tracker(Func<T> getValue)
            {
                this.getValue = getValue;
            }

            Func<T> getValue;

            public void Track()
            {
                T item = getValue();
                base.Track(item);
            }
        }

        /// <summary>
        /// 当返回ture时进入到下一步;
        /// </summary>
        public static IObservable<T> Where<T>(this IObservable<T> observable, Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public static IDisposable Subscrib<T>(this IObservable<T> observable)
        {
            throw new NotImplementedException();
        }

    }

}
