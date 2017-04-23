using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu
{


    public class AsyncOperationTracker<T>
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

        /// <summary>
        /// 订阅到,若操作已经完成则直接调用对应方法,返回Null;若未完成则加入到观察者合集;
        /// </summary>
        public IDisposable Subscribe(IAsyncOperation<T> operation, IObserver<T> observer)
        {
            if (operation.IsCompleted)
            {
                if (operation.IsFaulted)
                {
                    OnFaulted(operation.Exception, observer);
                }
                else if (operation.IsCanceled)
                {
                    OnCanceled(observer);
                }
                else
                {
                    OnCompleted(operation.Result, observer);
                }
                return null;
            }
            return tracker.Subscribe(observer);
        }

        void OnCompleted(T result, IObserver<T> observer)
        {
            observer.OnNext(result);
        }

        void OnFaulted(Exception ex, IObserver<T> observer)
        {
            observer.OnError(ex);
        }

        void OnCanceled(IObserver<T> observer)
        {
            var error = new OperationCanceledException();
            observer.OnError(error);
        }


        public void TrackCompleted(T result)
        {
            tracker.Track(result);
        }

        public void TrackFaulted(Exception ex)
        {
            tracker.TrackError(ex);
        }

        public void TrackCanceled()
        {
            var error = new OperationCanceledException();
            tracker.TrackError(error);
        }

        public void TrackCanceled(string message)
        {
            var error = new OperationCanceledException(message);
            tracker.TrackError(error);
        }

        /// <summary>
        /// 移除所有观察者,并调用观察者的  OnCompleted();
        /// </summary>
        public void ClearObserver()
        {
            tracker.TrackCompleted();
        }

    }

}
