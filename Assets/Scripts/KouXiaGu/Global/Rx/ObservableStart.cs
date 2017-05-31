using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu
{

    public interface IObservableStart<T>
    {
        IDisposable Subscribe(IStateObserver<T> observer);
    }

    public interface IStateObserver<T>
    {
        /// <summary>
        /// 完成时调用,并且移除;
        /// </summary>
        void OnCompleted(T item);

        /// <summary>
        /// 失败时调用;
        /// </summary>
        void OnFailed(Exception ex);
    }

    public class ObservableStart<T> : IObservableStart<T>
    {
        public ObservableStart() : this(new ObserverHashSet<IStateObserver<T>>())
        {
        }

        public ObservableStart(IObserverCollection<IStateObserver<T>> observerCollection)
        {
            this.observerCollection = observerCollection;
        }

        readonly IObserverCollection<IStateObserver<T>> observerCollection;
        public T Value { get; private set; }
        public Exception Ex { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsFailed { get; private set; }

        /// <summary>
        /// 当场景初始化完毕时调用,若已经设置了值则返回Null,并且调用委托;
        /// </summary>
        public IDisposable Subscribe(IStateObserver<T> observer)
        {
            if (IsCompleted)
            {
                if (IsFailed)
                {
                    observer.OnFailed(Ex);
                }
                else
                {
                    observer.OnCompleted(Value);
                }
                return null;
            }
            else
            {
                return observerCollection.Subscribe(observer);
            }
        }

        public void OnCompleted(T value)
        {
            foreach (var observer in observerCollection.EnumerateObserver())
            {
                observer.OnCompleted(Value);
            }
            observerCollection.Clear();

            Value = value;
            IsCompleted = true;
        }

        public void OnFailed(Exception ex)
        {
            foreach (var observer in observerCollection.EnumerateObserver())
            {
                observer.OnFailed(Ex);
            }
            observerCollection.Clear();

            Ex = ex;
            IsFailed = true;
            IsCompleted = true;
        }
    }
}
