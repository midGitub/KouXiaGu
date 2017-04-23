using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace KouXiaGu
{

    public abstract class AsyncInitializer : AsyncOperation
    {
        protected const string InitializationCompletedStr = "初始化完毕;";

        public abstract string Prefix { get; }

        string _prefix
        {
            get { return "------" + Prefix; }
        }

        protected void StartInitialize()
        {
            Debug.Log(_prefix + "  开始初始化;");
        }

        protected void OnCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted();
            Debug.Log(_prefix + "  所有内容初始化完毕;");
        }

        protected void OnFaulted(IList<IAsyncOperation> operations)
        {
            AggregateException ex = operations.ToAggregateException();
            OnFaulted(ex);
            Debug.LogError(_prefix + "  初始化失败;");
        }

        protected void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError(_prefix + "  初始化时遇到错误:" + operation.Exception);
        }
    }


    public abstract class AsyncInitializer<T> : AsyncOperation<T>, IObservable<T>
    {
        protected const string InitializationCompletedStr = "初始化完毕;";

        public AsyncInitializer()
        {
            tracker = new LinkedListTracker<T>();
        }

        LinkedListTracker<T> tracker;
        public abstract string Prefix { get; }

        string _prefix
        {
            get { return "------" + Prefix; }
        }

        protected void StartInitialize()
        {
            Debug.Log(_prefix + "  开始初始化;");
        }

        protected void OnCompleted(IList<IAsyncOperation> operations, T result)
        {
            OnCompleted(result);
            Debug.Log(_prefix + "  所以内容初始化完毕;");
        }

        protected void OnFaulted(IList<IAsyncOperation> operations)
        {
            AggregateException ex = operations.ToAggregateException();
            OnFaulted(ex);
            Debug.LogError(_prefix + "  初始化失败;");
        }

        protected void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError(_prefix + "  初始化时遇到错误:" + operation.Exception);
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            tracker.Track(Result);
        }

        void OnCompleted(IObserver<T> observer)
        {
            observer.OnNext(Result);
        }

        protected override void OnFaulted(Exception ex)
        {
            base.OnFaulted(ex);
            tracker.TrackError(ex);
        }

        void OnFaulted(IObserver<T> observer, Exception ex)
        {
            observer.OnError(ex);
        }


        protected override void OnCanceled()
        {
            base.OnCanceled();
            tracker.TrackError(new OperationCanceledException());
        }

        void OnCanceled(IObserver<T> observer)
        {
            observer.OnError(new OperationCanceledException());
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (IsCompleted)
            {
                if (IsFaulted)
                    OnFaulted(observer, Exception);
                else if (IsCanceled)
                    OnCanceled(observer);
                else
                    OnCompleted(observer);
            }
            return tracker.Subscribe(observer);
        }

    }

}
