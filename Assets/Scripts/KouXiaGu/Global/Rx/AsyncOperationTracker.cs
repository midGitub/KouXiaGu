using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu
{


    public class AsyncOperationTracker<T> : IObservable<T>
    {

        public AsyncOperationTracker()
        {
            tracker = new LinkedListTracker<T>();
        }

        public AsyncOperationTracker(TrackerBase<T> tracker)
        {
            this.tracker = tracker;
        }

        TrackerBase<T> tracker;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return tracker.Subscribe(observer);
        }

        public IDisposable Subscribe(IAsyncOperation<T> operation, IObserver<T> observer)
        {
            if (operation.IsCompleted)
            {
                if (operation.IsFaulted)
                {
                    TrackFaulted(operation.Exception, observer);
                }
                else if (operation.IsCanceled)
                {
                    TrackCanceled(observer);
                }
                else
                {
                    TrackCompleted(operation.Result, observer);
                }
            }
            return tracker.Subscribe(observer);
        }

        /// <summary>
        /// 将信息推送到观察者;
        /// </summary>
        public bool Track(IAsyncOperation<T> operation, IObserver<T> observer)
        {
            if (operation.IsCompleted)
            {
                if (operation.IsFaulted)
                    TrackFaulted(operation.Exception, observer);
                else if (operation.IsCanceled)
                    TrackCanceled(observer);
                else
                    TrackCompleted(operation.Result, observer);
                return true;
            }
            return false;
        }

        public void TrackCompleted(T result)
        {
            tracker.Track(result);
        }

        public void TrackCompleted(T result, IObserver<T> observer)
        {
            observer.OnNext(result);
        }


        public void TrackFaulted(Exception ex)
        {
            tracker.TrackError(ex);
        }

        public void TrackFaulted(Exception ex, IObserver<T> observer)
        {
            observer.OnError(ex);
        }


        public void TrackCanceled()
        {
            var error = new OperationCanceledException();
            tracker.TrackError(error);
        }

        public void TrackCanceled(IObserver<T> observer)
        {
            var error = new OperationCanceledException();
            observer.OnError(error);
        }


        public void TrackCanceled(string message)
        {
            var error = new OperationCanceledException(message);
            tracker.TrackError(error);
        }

        public void TrackCanceled(string message, IObserver<T> observer)
        {
            var error = new OperationCanceledException(message);
            observer.OnError(error);
        }

    }

}
