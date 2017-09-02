using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Diagnostics
{

    /// <summary>
    /// 包装 UnityEngine.Debug,并且支持命令条目;
    /// </summary>
    [ConsoleMethodsClass]
    public static class XiaGuConsole
    {
        static XiaGuConsole()
        {
            CommandCollection = new ConsoleMethodCollection();
            loggerGroup = new ObservableLogger();
        }

        static readonly ObservableLogger loggerGroup;
        public static ConsoleMethodCollection CommandCollection { get; private set; }
        public static bool IsSubscribeUnityDebug { get; private set; }
        static readonly ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// 是否为开发者模式;
        /// </summary>
        public static bool IsDeveloperMode
        {
            get { return XiaGu.IsDeveloperMode; }
        }

        /// <summary>
        /// 初始化;
        /// </summary>
        internal static void Initialize()
        {
            if (!IsInitialized)
            {
                ReflectionConsoleMethods.SearchMethod(CommandCollection);
                IsInitialized = true;
            }
        }

        /// <summary>
        /// 订阅到日志消息;
        /// </summary>
        public static IDisposable Subscribe(ILogger logger)
        {
            return loggerGroup.Subscribe(logger);
        }

        /// <summary>
        /// 是否传送 UnityEngine.Debug 的消息到所有订阅者?
        /// </summary>
        public static void SubscribeUnityDebug(bool isSubscribe)
        {
            if (IsSubscribeUnityDebug == isSubscribe)
            {
                return;
            }

            if (isSubscribe)
            {
                Debug.unityLogger.logHandler = loggerGroup;
            }
            else
            {
                Debug.unityLogger.logHandler = defaultLogHandler;
            }
            IsSubscribeUnityDebug = isSubscribe;
        }

        public static void Log(string message)
        {
            loggerGroup.Log(message);
        }

        public static void Log(object message)
        {
            Log(message.ToString());
        }

        public static void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(message);
        }


        public static void LogWarning(string message)
        {
            loggerGroup.LogWarning(message);
        }

        public static void LogWarning(object message)
        {
            LogWarning(message.ToString());
        }

        public static void LogWarning(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogWarning(message);
        }


        public static void LogError(string message)
        {
            loggerGroup.LogError(message);
        }

        public static void LogError(object message)
        {
            LogError(message.ToString());
        }

        public static void LogError(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogError(message);
        }

        /// <summary>
        /// 执行对应操作;
        /// </summary>
        public static void Operate(string message)
        {
            try
            {
                string[] parameters;
                var operater = CommandCollection.Find(message, out parameters);
                if (operater == null || (operater.IsDeveloperMethod && !XiaGu.IsDeveloperMode))
                {
                    LogWarning("未知命令:" + message);
                }
                else
                {
                    operater.Operate(parameters);
                }

            }
            catch (Exception ex)
            {
                LogError("命令出现异常:" + ex);
            }
        }

        /// <summary>
        /// 转换预留消息;
        /// </summary>
        internal static string ConvertMassage(string key, string message, params string[] parameterTypes)
        {
            if (parameterTypes == null)
            {
                return "[" + key + "]" + message;
            }
            else
            {
                string str = "[" + key;
                foreach (var parameter in parameterTypes)
                {
                    str += " " + parameter;
                }
                str += "]" + message;
                return str;
            }
        }

        [ConsoleMethod("help", "显示帮助")]
        public static void HelpLog()
        {
            foreach (var methodGroup in CommandCollection.CommandDictionary.Values)
            {
                foreach (var method in methodGroup)
                {
                    if (XiaGu.IsDeveloperMode || !method.IsDeveloperMethod)
                    {
                        Log(method.Message);
                    }
                }
            }
        }
    }
}
