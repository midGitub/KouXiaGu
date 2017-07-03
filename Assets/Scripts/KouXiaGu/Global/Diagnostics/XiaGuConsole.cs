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
    public static class XiaGuConsole
    {
        static XiaGuConsole()
        {
            CommandCollection = new CommandCollection();
            loggerGroup = new ObservableLogger();
        }

        static readonly ObservableLogger loggerGroup;
        public static CommandCollection CommandCollection { get; private set; }
        public static bool IsSubscribeUnityDebug { get; private set; }
        static readonly ILogHandler defaultLogHandler = Debug.logger.logHandler;

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
            ReflectionCommands.SearchMethod(CommandCollection);
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
                Debug.logger.logHandler = loggerGroup;
            }
            else
            {
                Debug.logger.logHandler = defaultLogHandler;
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
    }
}
