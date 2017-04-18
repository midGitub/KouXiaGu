using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Rx;

namespace KouXiaGu
{

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
    class ConsoleOutput : ILogHandler
    {
        public ConsoleOutput(ConsoleOutputTextStyle style)
        {
            Text = string.Empty;
            Style = style;
        }

        public string Text { get; set; }
        public ConsoleOutputTextStyle Style { get; private set; }

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


        public void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(message);
        }

        public void LogWarning(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogWarning(message);
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
