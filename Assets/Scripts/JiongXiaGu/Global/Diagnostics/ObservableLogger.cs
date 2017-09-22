using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Diagnostics
{


    public class ObservableLogger : ILogger, ILogHandler
    {
        public ObservableLogger()
        {
            observers = new ObserverLinkedList<ILogger>();
        }

        public ObservableLogger(IObserverCollection<ILogger> observers)
        {
            this.observers = observers;
        }

        readonly IObserverCollection<ILogger> observers;
        readonly object asyncLock = new object();

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
            lock (asyncLock)
            {
                foreach (var observer in observers.EnumerateObserver())
                {
                    observer.Log(message);
                }
            }
        }

        public void LogError(string message)
        {
            lock (asyncLock)
            {
                foreach (var observer in observers.EnumerateObserver())
                {
                    observer.LogError(message);
                }
            }
        }

        public void LogWarning(string message)
        {
            lock (asyncLock)
            {
                foreach (var observer in observers.EnumerateObserver())
                {
                    observer.LogWarning(message);
                }
            }
        }

        void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            switch (logType)
            {
                case LogType.Warning:
                    LogWarning(string.Format(format, args));
                    break;

                case LogType.Error:
                    LogError(string.Format(format, args));
                    break;

                default:
                    Log(string.Format(format, args));
                    break;
            }
        }

        void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
        {
            LogError(exception.Message);
        }
    }
}
