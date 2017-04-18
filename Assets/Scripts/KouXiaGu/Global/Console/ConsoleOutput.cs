using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Rx;

namespace KouXiaGu
{

    public interface IConsoleOutput
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
    }

    public static class ConsoleOutputExtensions
    {
        public static void Log(this IConsoleOutput output, string format, params object[] args)
        {
            string message = string.Format(format, args);
            output.Log(message);
        }

        public static void LogWarning(this IConsoleOutput output, string format, params object[] args)
        {
            string message = string.Format(format, args);
            output.LogWarning(message);
        }

        public static void LogError(this IConsoleOutput output, string format, params object[] args)
        {
            string message = string.Format(format, args);
            output.LogError(message);
        }
    }

    /// <summary>
    /// 使用"RichText(富文本)"控制输出格式;
    /// </summary>
    [Serializable]
    class ConsoleOutputTextStyle
    {
        public Color NormalColor = Color.black;
        public Color WarningColor = Color.yellow;
        public Color ErrorColor = Color.red;

        public string GetNormalLog(string message)
        {
            return SetColor(message, NormalColor);
        }

        public string GetWarningLog(string message)
        {
            return SetColor(message, WarningColor);
        }

        public string GetErrorLog(string message)
        {
            return SetColor(message, ErrorColor);
        }

        string SetColor(string message, Color color)
        {
            string col = color.ToHex();
            message = "<color=" + col + ">" + message + "</color>";
            return message;
        }
    }

    /// <summary>
    /// 控制台输出;
    /// </summary>
    class ConsoleOutput : IConsoleOutput, ILogHandler, IObservable<string>
    {
        public ConsoleOutput(ConsoleOutputTextStyle style)
        {
            Text = string.Empty;
            Style = style;
            textTracker = new ListTracker<string>();
        }

        ListTracker<string> textTracker;
        public string Text { get; internal set; }
        public ConsoleOutputTextStyle Style { get; private set; }

        /// <summary>
        /// 当字符串发生变化时调用;
        /// </summary>
        public IDisposable Subscribe(IObserver<string> observer)
        {
            return textTracker.Subscribe(observer);
        }

        public void Log(string message)
        {
            message = Style.GetNormalLog(message);
            AddLog(message);
        }

        public void LogWarning(string message)
        {
            message = Style.GetWarningLog(message);
            AddLog(message);
        }

        public void LogError(string message)
        {
            message = Style.GetErrorLog(message);
            AddLog(message);
        }

        void AddLog(string message)
        {
            if (Text.Length >= 10000)
                Clear();

            Text += message + Environment.NewLine;
            textTracker.Track(Text);
        }

        public void Clear()
        {
            Text = string.Empty;
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
                    this.LogWarning(format, args);
                    break;

                case LogType.Error:
                    this.LogError(format, args);
                    break;

                default:
                    this.Log(format, args);
                    break;
            }
        }

    }

}
