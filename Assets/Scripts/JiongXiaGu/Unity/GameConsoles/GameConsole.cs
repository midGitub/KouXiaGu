using System;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 游戏控制台(仅主线程调用);
    /// </summary>
    public static class GameConsole
    {
        /// <summary>
        /// 控制台事件观察者合集;
        /// </summary>
        private static readonly ObserverCollection<ConsoleEvent> observerCollection = new ObserverList<ConsoleEvent>();

        /// <summary>
        /// 控制台方法合集;
        /// </summary>
        public static MethodMap MethodMap { get; private set; } = new MethodMap();

        /// <summary>
        /// 观察者结构;
        /// </summary>
        public static IObservable<ConsoleEvent> ObserverCollection => observerCollection;

        /// <summary>
        /// 初始化控制台;
        /// </summary>
        internal static void Initialize()
        {
            foreach (var method in ReflectionImporter.EnumerateMethods(typeof(GameConsole).Assembly))
            {
                if (!MethodMap.TryAdd(method))
                {
                    Debug.LogError(string.Format("重复的控制台方法[{0}]", method.Description.ToString()));
                }
            }
        }

        /// <summary>
        /// 通知观察者;
        /// </summary>
        private static void NotifyNext(ConsoleEvent consoleEvent)
        {
            observerCollection.NotifyNext(consoleEvent);
        }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Write(string message)
        {
            ConsoleEvent consoleEvent = new ConsoleEvent()
            {
                EventType = ConsoleEventType.Normal,
                Message = message,
            };
            NotifyNext(consoleEvent);
        }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Write(object message)
        {
            Write(message.ToString());
        }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Write(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Write(message);
        }

        /// <summary>
        /// 记录成功条目;
        /// </summary>
        public static void WriteSuccessful(string message)
        {
            ConsoleEvent consoleEvent = new ConsoleEvent()
            {
                EventType = ConsoleEventType.Successful,
                Message = message,
            };
            NotifyNext(consoleEvent);
        }

        /// <summary>
        /// 记录成功条目;
        /// </summary>
        public static void WriteSuccessful(object message)
        {
            WriteSuccessful(message.ToString());
        }

        /// <summary>
        /// 记录成功条目;
        /// </summary>
        public static void WriteSuccessful(string format, params object[] args)
        {
            string message = string.Format(format, args);
            WriteSuccessful(message);
        }

        /// <summary>
        /// 记录警告条目;
        /// </summary>
        public static void WriteWarning(string message)
        {
            ConsoleEvent consoleEvent = new ConsoleEvent()
            {
                EventType = ConsoleEventType.Warning,
                Message = message,
            };
            NotifyNext(consoleEvent);
        }

        /// <summary>
        /// 记录警告条目;
        /// </summary>
        public static void WriteWarning(object message)
        {
            WriteWarning(message.ToString());
        }

        /// <summary>
        /// 记录警告条目;
        /// </summary>
        public static void WriteWarning(string format, params object[] args)
        {
            string message = string.Format(format, args);
            WriteWarning(message);
        }

        /// <summary>
        /// 记录异常条目;
        /// </summary>
        public static void WriteError(string message)
        {
            ConsoleEvent consoleEvent = new ConsoleEvent()
            {
                EventType = ConsoleEventType.Error,
                Message = message,
            };
            NotifyNext(consoleEvent);
        }

        /// <summary>
        /// 记录异常条目;
        /// </summary>
        public static void WriteError(object message)
        {
            WriteError(message.ToString());
        }

        /// <summary>
        /// 记录异常条目;
        /// </summary>
        public static void WriteError(string format, params object[] args)
        {
            string message = string.Format(format, args);
            WriteError(message);
        }

        /// <summary>
        /// 记录方法条目;
        /// </summary>
        public static void WriteMethod(string message)
        {
            ConsoleEvent consoleEvent = new ConsoleEvent()
            {
                EventType = ConsoleEventType.Method,
                Message = message,
            };
            NotifyNext(consoleEvent);
        }

        /// <summary>
        /// 记录方法条目;
        /// </summary>
        public static void WriteMethod(object message)
        {
            WriteMethod(message.ToString());
        }

        /// <summary>
        /// 记录方法条目;
        /// </summary>
        public static void WriteMethod(string format, params object[] args)
        {
            string message = string.Format(format, args);
            WriteMethod(message);
        }

        /// <summary>
        /// 执行指定控制台方法;
        /// </summary>
        public static void Run(string message)
        {
            MethodMap.Run(message);
        }
    }
}
