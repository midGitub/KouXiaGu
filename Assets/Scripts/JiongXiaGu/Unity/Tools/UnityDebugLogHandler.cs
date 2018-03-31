using System;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// UnityEngine.Debug 便捷工具;
    /// </summary>
    public static class UnityDebug
    {

        #region TemporaryLogHandler

        /// <summary>
        /// 提供临时替换 ILogHandler 在using语法使用;
        /// </summary>
        public static IDisposable TemporaryLogHandler(ILogHandler logHandler)
        {
            var defaultLogHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = logHandler;
            return new TemporaryLogHandlerCanceler(defaultLogHandler);
        }

        private struct TemporaryLogHandlerCanceler : IDisposable
        {
            public ILogHandler logHandler;

            public TemporaryLogHandlerCanceler(ILogHandler logHandler)
            {
                this.logHandler = logHandler;
            }

            public void Dispose()
            {
                if (logHandler != null)
                {
                    Debug.unityLogger.logHandler = logHandler;
                    logHandler = null;
                }
            }
        }

        #endregion


        #region Observer 

        private static readonly ObserverCollection<UnityDebugLogEvent> observers = new ObserverLinkedList<UnityDebugLogEvent>();
        private static ILogHandler defaultLogHandler;

        /// <summary>
        /// 添加 观察者 到 Unity.Debug.unityLogger.logHandler;(仅Unity线程调用)
        /// </summary>
        public static IDisposable Subscribe(IObserver<UnityDebugLogEvent> observer)
        {
            UnityThread.ThrowIfNotUnityThread();
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            if (defaultLogHandler == null)
            {
                defaultLogHandler = Debug.unityLogger.logHandler;
                Debug.unityLogger.logHandler = new LogHandler();
            }

            return observers.Subscribe(observer);
        }

        /// <summary>
        /// 提供 Unity.Debug.unityLogger.logHandler 为观察模式(仅限Unity线程);
        /// </summary>
        private class LogHandler : ILogHandler
        {
            void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
            {
                UnityDebugLogEvent unityDebugLogEvent = new UnityDebugLogEvent(LogType.Error, context, exception.ToString());
                defaultLogHandler?.LogException(exception, context);
                observers.NotifyNext(unityDebugLogEvent);
            }

            void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                UnityDebugLogEvent unityDebugLogEvent = new UnityDebugLogEvent(logType, context, format, args);
                defaultLogHandler?.LogFormat(logType, context, format, args);
                observers.NotifyNext(unityDebugLogEvent);
            }
        }

        #endregion

    }

    /// <summary>
    /// Unity.Debug 事件;
    /// </summary>
    public struct UnityDebugLogEvent
    {
        public LogType EventType { get; set; }
        public UnityEngine.Object Context { get; set; }
        public string Message { get; set; }

        public UnityDebugLogEvent(LogType logType, UnityEngine.Object context, string message)
        {
            EventType = logType;
            Context = context;
            Message = message;
        }

        public UnityDebugLogEvent(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            EventType = logType;
            Context = context;
            Message = string.Format(format, args);
        }
    }
}
