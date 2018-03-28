using System;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 提供 Unity.Debug.unityLogger.logHandler 为观察模式(仅限Unity线程);
    /// </summary>
    public class UnityDebugLogHandler : ILogHandler
    {
        private static readonly ObserverList<UnityDebugLogEvent> observers = new ObserverList<UnityDebugLogEvent>();
        private static ILogHandler defaultLogHandler;
        private static UnityDebugLogHandler instance;

        private UnityDebugLogHandler()
        {
        }

        /// <summary>
        /// 添加 观察者 到 Unity.Debug.unityLogger.logHandler;
        /// </summary>
        public static IDisposable Subscribe(IObserver<UnityDebugLogEvent> observer)
        {
            UnityThread.ThrowIfNotUnityThread();
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            if (defaultLogHandler == null)
            {
                defaultLogHandler = Debug.unityLogger.logHandler;
                Debug.unityLogger.logHandler = instance = new UnityDebugLogHandler();
            }

            return observers.Subscribe(observer);
        }

        void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
        {
            UnityDebugLogEvent unityDebugLogEvent = new UnityDebugLogEvent(LogType.Error, context, exception.ToString());
            defaultLogHandler.LogException(exception, context);
            observers.NotifyNext(unityDebugLogEvent);
        }

        void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            UnityDebugLogEvent unityDebugLogEvent = new UnityDebugLogEvent(logType, context, format, args);
            defaultLogHandler.LogFormat(logType, context, format, args);
            observers.NotifyNext(unityDebugLogEvent);
        }
    }
}
