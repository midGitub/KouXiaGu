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

        static ConsoleOutput output
        {
            get { return output; }
        }

        static ConsoleInput input
        {
            get { return logger.Input; }
        }


        public static void Log(object message)
        {
            Log(message.ToString());
        }

        public static void Log(string message)
        {
            output.Log(message);
        }

        public static void Log(string format, params object[] args)
        {
            output.Log(format, args);
        }


        public static void LogWarning(object message)
        {
            LogWarning(message.ToString());
        }

        public static void LogWarning(string message)
        {
            output.LogWarning(message);
        }

        public static void LogWarning(string format, params object[] args)
        {
            output.LogWarning(format, args);
        }


        public static void LogError(object message)
        {
            LogError(message.ToString());
        }

        public static void LogError(string message)
        {
            output.LogError(message);
        }

        public static void LogError(string format, params object[] args)
        {
            output.LogError(format, args);
        }


        /// <summary>
        /// 输入操作;
        /// </summary>
        public static void Operate(string message)
        {
            input.Operate(message);
        }

    }

}
