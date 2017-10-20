using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 游戏控制台(线程安全);
    /// </summary>
    public static class Console
    {
        private static readonly object asyncLock = new object();
        private static readonly ConsoleMethodSchema methodSchema = new ConsoleMethodSchema();
        private static readonly ObservableLoggerCollection loggerCollection = new ObservableLoggerCollection();
        private static UnityLogListener unityLogListener = new UnityLogListener();
        private static IDisposable listenUnityDebugDisposer;

        /// <summary>
        /// 是否监听UnityDebug日志?
        /// </summary>
        public static bool IsListenUnityLog
        {
            get { return listenUnityDebugDisposer != null; }
        }

        /// <summary>
        /// 订阅到控制台日志;
        /// </summary>
        public static IDisposable Subscribe(ILogger logger)
        {
            lock (asyncLock)
            {
                return loggerCollection.Subscribe(logger);
            }
        }

        /// <summary>
        /// 监听 Unity.Debug 的输出内容;
        /// </summary>
        public static void ListenUnityLog()
        {
            lock (asyncLock)
            {
                if (listenUnityDebugDisposer == null)
                {
                    listenUnityDebugDisposer = UnityLogHandlerGroup.AddHandler(unityLogListener);
                }
            }
        }

        /// <summary>
        /// 取消监听 Unity.Debug 的输出内容;
        /// </summary>
        public static void UnListenUnityLog()
        {
            lock (asyncLock)
            {
                if (listenUnityDebugDisposer != null)
                {
                    listenUnityDebugDisposer.Dispose();
                    listenUnityDebugDisposer = null;
                }
            }
        }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Log(string message)
        {
            lock (asyncLock)
            {
                loggerCollection.Log(message);
            }
        }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Log(object message)
        {
            Log(message.ToString());
        }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(message);
        }

        /// <summary>
        /// 记录成功条目;
        /// </summary>
        public static void LogSuccessful(string message)
        {
            lock (asyncLock)
            {
                loggerCollection.LogSuccessful(message);
            }
        }

        /// <summary>
        /// 记录成功条目;
        /// </summary>
        public static void LogSuccessful(object message)
        {
            LogSuccessful(message.ToString());
        }

        /// <summary>
        /// 记录成功条目;
        /// </summary>
        public static void LogSuccessful(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogSuccessful(message);
        }

        /// <summary>
        /// 记录警告条目;
        /// </summary>
        public static void LogWarning(string message)
        {
            lock (asyncLock)
            {
                loggerCollection.LogWarning(message);
            }
        }

        /// <summary>
        /// 记录警告条目;
        /// </summary>
        public static void LogWarning(object message)
        {
            LogWarning(message.ToString());
        }

        /// <summary>
        /// 记录警告条目;
        /// </summary>
        public static void LogWarning(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogWarning(message);
        }

        /// <summary>
        /// 记录异常条目;
        /// </summary>
        public static void LogError(string message)
        {
            lock (asyncLock)
            {
                loggerCollection.LogError(message);
            }
        }

        /// <summary>
        /// 记录异常条目;
        /// </summary>
        public static void LogError(object message)
        {
            LogError(message.ToString());
        }

        /// <summary>
        /// 记录异常条目;
        /// </summary>
        public static void LogError(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogError(message);
        }

        private static readonly char[] separator = new char[] { ' ' };

        /// <summary>
        /// 执行指定控制台方法;
        /// </summary>
        public static void Do(string method)
        {
            string[] valueArray = method.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if (valueArray.Length == 0)
            {
                throw new ArgumentException(string.Format("不合法的命令[{0}];", method));
            }
            else if (valueArray.Length == 1)
            {
                var methodName = valueArray[0];
                var consoleMethod = methodSchema.GetMethod(methodName);
                consoleMethod.Invoke(null);
            }
            else if (valueArray.Length > 1)
            {
                var methodName = valueArray[0];
                int parameterCount = valueArray.Length - 1;
                var consoleMethod = methodSchema.GetMethod(methodName, parameterCount);
                string[] parameters = new string[parameterCount];
                Array.Copy(valueArray, 1, parameters, 0, parameterCount);
                consoleMethod.Invoke(parameters);
            }
        }

        /// <summary>
        /// 监听UnityLog,并且使用 Console 输出;
        /// </summary>
        public class UnityLogListener : ILogHandler
        {
            void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
            {
                throw new NotImplementedException();
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
        }
    }
}
