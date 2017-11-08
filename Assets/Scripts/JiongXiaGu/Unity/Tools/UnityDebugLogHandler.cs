using System;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 提供 Unity.Debug.unityLogger.logHandler 为观察模式;
    /// </summary>
    public class UnityDebugLogHandler : ILogHandler
    {
        private static object asyncLock = new object();
        private static readonly ObserverList<UnityDebugLogEvent> observers = new ObserverList<UnityDebugLogEvent>();
        private static ILogHandler defaultLogHandler;

        private UnityDebugLogHandler()
        {
        }

        /// <summary>
        /// 添加 观察者 到 Unity.Debug.unityLogger.logHandler(线程安全);
        /// </summary>
        public static IDisposable Subscribe(IObserver<UnityDebugLogEvent> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            lock (asyncLock)
            {
                if (defaultLogHandler == null)
                {
                    defaultLogHandler = Debug.unityLogger.logHandler;
                    Debug.unityLogger.logHandler = new UnityDebugLogHandler();
                }
                return observers.Add(observer);
            }
        }

        /// <summary>
        /// 移除观察者;(线程安全)
        /// </summary>
        public static bool Unsubscribe(IObserver<UnityDebugLogEvent> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            lock (asyncLock)
            {
                return observers.Remove(observer);
            }
        }

        void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
        {
            UnityDebugLogEvent unityDebugLogEvent = new UnityDebugLogEvent(LogType.Error, context, exception.ToString());
            lock (asyncLock)
            {
                defaultLogHandler.LogException(exception, context);
                observers.NotifyNext(unityDebugLogEvent);
            }
        }

        void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            UnityDebugLogEvent unityDebugLogEvent = new UnityDebugLogEvent(logType, context, format, args);
            lock (asyncLock)
            {
                defaultLogHandler.LogFormat(logType, context, format, args);
                observers.NotifyNext(unityDebugLogEvent);
            }
        }
    }
}
