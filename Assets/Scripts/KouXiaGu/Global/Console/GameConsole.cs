using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 控制台,区别于 Unity.Debug;
    /// </summary>
    public sealed class GameConsole
    {
        static ConsoleWindow logger;

        /// <summary>
        /// 是否显示 Unity.Debug 的内容?
        /// </summary>
        public static bool IsShowUnityDebug { get; private set; }


        public static void Log(object message)
        {
            logger.Output.Log(message.ToString());
        }

        public static void LogFormat(string format, params object[] args)
        {

        }


        public static void LogWarning(object message)
        {
            logger.Output.LogWarning(message.ToString());
        }

        public static void LogWarningFormat(string format, params object[] args)
        {

        }


        public static void LogError(object message)
        {
            logger.Output.LogError(message.ToString());
        }

        public static void LogErrorFormat(string format, params object[] args)
        {

        }

    }

    /// <summary>
    /// 控制台输出;
    /// </summary>
    [Serializable]
    class ConsoleOutput : ILogHandler
    {
        public Color NormalColor = Color.black;
        public Color WarningColor = Color.yellow;
        public Color ErrorColor = Color.red;

        /// <summary>
        /// 使用RichText的文本内容;
        /// </summary>
        public string Text { get; internal set; }

        public void Log(string message)
        {
            message = SetColor(message, NormalColor);
            AddLog(message);
        }

        public void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(message);
        }


        public void LogWarning(string message)
        {
            message = SetColor(message, WarningColor);
            AddLog(message);
        }

        public void LogWarning(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogWarning(message);
        }


        public void LogError(string message)
        {
            message = SetColor(message, ErrorColor);
            AddLog(message);
        }

        public void LogError(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogError(message);
        }

        void AddLog(string message)
        {
            if (Text.Length >= 10000)
                Clear();

            Text += message + Environment.NewLine;
        }

        public void Clear()
        {
            Text = string.Empty;
        }

        string SetColor(string message, Color color)
        {
            string col = color.ToHex();
            message = "<color=" + col + ">" + message + "</color>";
            return message;
        }

        void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
        {
            LogError(exception.Message);
        }

        void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            switch (logType)
            {
                case LogType.Warning:
                    LogWarning(format, args);
                    break;

                case LogType.Error:
                    LogError(format, args);
                    break;

                default:
                    Log(format, args);
                    break;
            }
        }
    }

    /// <summary>
    /// 控制台输入;
    /// </summary>
    class ConsoleInput
    {
    }

}
