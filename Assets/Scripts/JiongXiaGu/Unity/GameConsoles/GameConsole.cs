using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 游戏控制台(方法线程安全);
    /// </summary>
    public static class GameConsole
    {
        public const int MaxRecordCount = 500;
        private static readonly object asyncLock = new object();
        private static readonly IObserverCollection<IConsoleLogger> consoleObservers = new ObserverLinkedList<IConsoleLogger>();
        internal static ConsoleMethodSchema MethodSchema { get; private set; } = new ConsoleMethodSchema();
        
        /// <summary>
        /// 所有控制台订阅者;
        /// </summary>
        public static IReadOnlyCollection<IConsoleLogger> ConsoleObservers
        {
            get { return consoleObservers; }
        }

        /// <summary>
        /// 订阅到控制台日志;
        /// </summary>
        public static IDisposable Subscribe(IConsoleLogger observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            lock (asyncLock)
            {
                if (consoleObservers.Contains(observer))
                    throw new ArgumentException("重复订阅;");

                return consoleObservers.Subscribe(observer);
            }
        }

        /// <summary>
        /// 当调用观察者方法返回异常时的解决方案;
        /// </summary>
        private static void OnObserverFailure(IConsoleLogger observer, Exception ex)
        {
            Debug.LogWarning(string.Format("订阅者[{0}]出现异常[{1}];", observer, ex));
        }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Write(string message)
        {
            lock (asyncLock)
            {
                foreach (var observer in consoleObservers.EnumerateObserver())
                {
                    try
                    {
                        observer.Write(message);
                    }
                    catch(Exception ex)
                    {
                        OnObserverFailure(observer, ex);
                    }
                }
            }
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
            lock (asyncLock)
            {
                foreach (var observer in consoleObservers.EnumerateObserver())
                {
                    try
                    {
                        observer.WriteSuccessful(message);
                    }
                    catch (Exception ex)
                    {
                        OnObserverFailure(observer, ex);
                    }
                }
            }
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
            lock (asyncLock)
            {
                foreach (var observer in consoleObservers.EnumerateObserver())
                {
                    try
                    {
                        observer.WriteWarning(message);
                    }
                    catch (Exception ex)
                    {
                        OnObserverFailure(observer, ex);
                    }
                }
            }
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
            lock (asyncLock)
            {
                foreach (var observer in consoleObservers.EnumerateObserver())
                {
                    try
                    {
                        observer.WriteError(message);
                    }
                    catch (Exception ex)
                    {
                        OnObserverFailure(observer, ex);
                    }
                }
            }
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
        /// 输入的方法命令间隔符;
        /// </summary>
        private static readonly char[] methodSeparator = new char[] { ' ' };

        /// <summary>
        /// 执行指定控制台方法;
        /// </summary>
        public static void Do(string method)
        {
            lock (asyncLock)
            {
                foreach (var observer in consoleObservers.EnumerateObserver())
                {
                    try
                    {
                        observer.Do(method);
                    }
                    catch (Exception ex)
                    {
                        OnObserverFailure(observer, ex);
                    }
                }
            }
            DoMethod(method);
        }

        /// <summary>
        /// 执行指定控制台方法;
        /// </summary>
        private static void DoMethod(string method)
        {
            if (string.IsNullOrWhiteSpace(method))
            {
                ThrowMethodStringIncorrect(method);
            }

            string[] valueArray = method.Split(methodSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (valueArray.Length == 0)
            {
                ThrowMethodStringIncorrect(method);
            }
            else if (valueArray.Length == 1)
            {
                var methodName = valueArray[0];
                var consoleMethod = MethodSchema.GetMethod(methodName);
                consoleMethod.Invoke(null);
            }
            else if (valueArray.Length > 1)
            {
                var methodName = valueArray[0];
                int parameterCount = valueArray.Length - 1;
                var consoleMethod = MethodSchema.GetMethod(methodName, parameterCount);
                string[] parameters = new string[parameterCount];
                Array.Copy(valueArray, 1, parameters, 0, parameterCount);
                consoleMethod.Invoke(parameters);
            }
        }

        /// <summary>
        /// 抛出传入方法命令不正确异常;
        /// </summary>
        private static void ThrowMethodStringIncorrect(string method)
        {
            throw new ArgumentException(string.Format("不合法的命令[{0}];", method));
        }
    }
}
