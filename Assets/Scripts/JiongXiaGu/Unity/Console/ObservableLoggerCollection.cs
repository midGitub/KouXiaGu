using System;

namespace JiongXiaGu.Unity
{
    /// <summary>
    /// 可订阅的日志记录器合集;
    /// </summary>
    public class ObservableLoggerCollection : ILogger
    {
        private IObserverCollection<ILogger> observers;

        public ObservableLoggerCollection()
        {
            observers = new ObserverLinkedList<ILogger>();
        }

        public ObservableLoggerCollection(IObserverCollection<ILogger> observers)
        {
            this.observers = observers;
        }

        /// <summary>
        /// 订阅到;
        /// </summary>
        public IDisposable Subscribe(ILogger observer)
        {
            if (observers.Contains(observer))
                throw new ArgumentException("重复订阅;");

            return observers.Subscribe(observer);
        }

        public void Log(string message)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.Log(message);
            }
        }

        public void LogSuccessful(string message)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.LogSuccessful(message);
            }
        }

        public void LogWarning(string message)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.LogWarning(message);
            }
        }

        public void LogError(string message)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.LogError(message);
            }
        }
    }
}
