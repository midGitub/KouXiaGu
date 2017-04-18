using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 控制台,区别于 Unity.Debug;
    /// </summary>
    public sealed class GameConsole
    {
        static ConsoleWindow logger
        {
            get { return ConsoleWindow.instance; }
        }

        /// <summary>
        /// 是否显示 Unity.Debug 的内容?
        /// </summary>
        public static bool IsShowUnityDebug { get; private set; }


        public static void Log(object message)
        {
            Log(message.ToString());
        }

        public static void Log(string message)
        {
            logger.Output.Log(message);
        }

        public static void Log(string format, params object[] args)
        {
            logger.Output.Log(format, args);
        }


        public static void LogWarning(object message)
        {
            LogWarning(message.ToString());
        }

        public static void LogWarning(string message)
        {
            logger.Output.LogWarning(message);
        }

        public static void LogWarning(string format, params object[] args)
        {
            logger.Output.LogWarning(format, args);
        }


        public static void LogError(object message)
        {
            LogError(message.ToString());
        }

        public static void LogError(string message)
        {
            logger.Output.LogError(message);
        }

        public static void LogError(string format, params object[] args)
        {
            logger.Output.LogError(format, args);
        }

    }

}
