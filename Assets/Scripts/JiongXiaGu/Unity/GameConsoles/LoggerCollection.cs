//using System;

//namespace JiongXiaGu.Unity.GameConsoles
//{

//    /// <summary>
//    /// 可订阅的日志记录器合集;
//    /// </summary>
//    public class LoggerCollection : IConsoleLogger
//    {
//        private IObserverCollection<IConsoleLogger> observers;

//        public LoggerCollection()
//        {
//            observers = new ObserverLinkedList<IConsoleLogger>();
//        }

//        public LoggerCollection(IObserverCollection<IConsoleLogger> observers)
//        {
//            this.observers = observers;
//        }

//        /// <summary>
//        /// 订阅到;
//        /// </summary>
//        public IDisposable Subscribe(IConsoleLogger observer)
//        {
//            if (observers.Contains(observer))
//                throw new ArgumentException("重复订阅;");

//            return observers.Subscribe(observer);
//        }

//        public void Write(string message)
//        {
//            foreach (var observer in observers.EnumerateObserver())
//            {
//                observer.Write(message);
//            }
//        }

//        public void WriteSuccessful(string message)
//        {
//            foreach (var observer in observers.EnumerateObserver())
//            {
//                observer.WriteSuccessful(message);
//            }
//        }

//        public void WriteWarning(string message)
//        {
//            foreach (var observer in observers.EnumerateObserver())
//            {
//                observer.WriteWarning(message);
//            }
//        }

//        public void WriteError(string message)
//        {
//            foreach (var observer in observers.EnumerateObserver())
//            {
//                observer.WriteError(message);
//            }
//        }

//        public void Do(string message)
//        {
//            foreach (var observer in observers.EnumerateObserver())
//            {
//                observer.Do(message);
//            }
//        }
//    }
//}
