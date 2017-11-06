using System;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 游戏控制台(方法线程安全);
    /// </summary>
    public static class GameConsole
    {
        /// <summary>
        /// 异步锁;
        /// </summary>
        private static readonly object asyncLock = new object();

        /// <summary>
        /// 控制台事件观察者合集;
        /// </summary>
        private static readonly ObserverCollection<ConsoleEvent> observerCollection = new ObserverList<ConsoleEvent>();

        /// <summary>
        /// 控制台方法合集;
        /// </summary>
        public static ConsoleMethodSchema MethodSchema { get; private set; } = new ConsoleMethodSchema();

        /// <summary>
        /// 订阅控制台事件;
        /// </summary>
        public static IDisposable Subscribe(IObserver<ConsoleEvent> observer)
        {
            lock (asyncLock)
            {
                return observerCollection.Add(observer);
            }
        }

        /// <summary>
        /// 取消订阅;
        /// </summary>
        public static bool Unsubscribe(IObserver<ConsoleEvent> observer)
        {
            lock (asyncLock)
            {
                return observerCollection.Remove(observer);
            }
        }

        /// <summary>
        /// 通知观察者;
        /// </summary>
        private static void NotifyNext(ConsoleEvent consoleEvent)
        {
            lock (asyncLock)
            {
                observerCollection.NotifyNext(consoleEvent);
            }
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
        /// 输入的方法命令间隔符;
        /// </summary>
        private static readonly char[] methodSeparator = new char[] { ' ' };

        /// <summary>
        /// 执行指定控制台方法;
        /// </summary>
        public static void Do(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ThrowMethodStringIncorrect(message);
            }

            string[] valueArray = message.Split(methodSeparator, StringSplitOptions.RemoveEmptyEntries);
            var methodName = valueArray[0];

            if (valueArray.Length == 0)
            {
                ThrowMethodStringIncorrect(message);
            }
            else if (valueArray.Length == 1)
            {
                ConsoleMethod consoleMethod;
                if (MethodSchema.TryGetMethod(methodName, 0, out consoleMethod))
                {
                    consoleMethod.Invoke(null);
                }
                else
                {
                    ThrowMethodNotFound(methodName, 0);
                }
            }
            else if (valueArray.Length > 1)
            {
                int parameterCount = valueArray.Length - 1;
                ConsoleMethod consoleMethod;

                if (MethodSchema.TryGetMethod(methodName, parameterCount, out consoleMethod))
                {
                    string[] parameters = new string[parameterCount];
                    Array.Copy(valueArray, 1, parameters, 0, parameterCount);

                    consoleMethod.Invoke(parameters);
                }
                else
                {
                    ThrowMethodNotFound(methodName, parameterCount);
                }
            }
        }

        /// <summary>
        /// 抛出传入方法命令不正确异常;
        /// </summary>
        private static void ThrowMethodStringIncorrect(string methodName)
        {
            throw new ArgumentException(string.Format("不合法的命令[{0}];", methodName));
        }

        private static void ThrowMethodNotFound(string methodName, int parameterCount)
        {
            throw new KeyNotFoundException(string.Format("未找到参数为[{1}],方法名为:[{0}]的方法", methodName, parameterCount));
        }
    }
}
