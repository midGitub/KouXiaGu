//using System;
//using UnityEngine;

//namespace JiongXiaGu.Unity.GameConsoles
//{

//    /// <summary>
//    /// 提供 Unity.Debug.unityLogger.logHandler 为合集加入方式;
//    /// </summary>
//    public class UnityLogHandlerGroup : ILogHandler
//    {
//        private IObserverCollection<ILogHandler> observers;

//        private UnityLogHandlerGroup()
//        {
//            observers = new ObserverLinkedList<ILogHandler>();
//        }

//        void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
//        {
//            lock (asyncLock)
//            {
//                foreach (var observer in observers.EnumerateObserver())
//                {
//                    observer.LogException(exception, context);
//                }
//            }
//        }

//        void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
//        {
//            lock (asyncLock)
//            {
//                foreach (var observer in observers.EnumerateObserver())
//                {
//                    observer.LogFormat(logType, context, format, args);
//                }
//            }
//        }


//        private static object asyncLock = new object();
//        private static ILogHandler defaultLogHandler;
//        private static UnityLogHandlerGroup unityLogHandlerGroup;

//        /// <summary>
//        /// 添加 自定义ILogHandler 到 Unity.Debug.unityLogger.logHandler(非线程安全);
//        /// </summary>
//        public static IDisposable AddHandler(ILogHandler logHandler)
//        {
//            if (logHandler == null)
//                throw new ArgumentNullException(nameof(logHandler));

//            lock (asyncLock)
//            {
//                if (unityLogHandlerGroup == null)
//                {
//                    defaultLogHandler = Debug.unityLogger.logHandler;
//                    unityLogHandlerGroup = new UnityLogHandlerGroup();
//                    Debug.unityLogger.logHandler = unityLogHandlerGroup;
//                }
//                return unityLogHandlerGroup.observers.Subscribe(logHandler);
//            }
//        }
//    }
//}
