using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
    /// 运行状态日志输出窗口;
    /// </summary>
    [DisallowMultipleComponent]
    sealed class ConsoleWindow : MonoBehaviour, ILogHandler
    {

        [SerializeField]
        Transform contentContainer;

        [SerializeField]
        ConsoleOutput output;

        ILogHandler defaultLogHandler;

        public ConsoleOutput Output
        {
            get { return output; }
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            defaultLogHandler = Debug.logger.logHandler;
            Debug.logger.logHandler = this;
        }

        void Start()
        {
            Debug.LogError(new Exception());
        }

        void OnDestroy()
        {
            Debug.logger.logHandler = defaultLogHandler;
        }

        void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
        {
            defaultLogHandler.LogException(exception, context);
            (output as ILogHandler).LogException(exception, context);
        }

        void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            defaultLogHandler.LogFormat(logType, context, format, args);
            (output as ILogHandler).LogFormat(logType, context, format, args);
        }

    }


    [Serializable]
    class ConsoleOutput : ILogHandler
    {
        [SerializeField]
        Color normalColor = Color.black;

        [SerializeField]
        Color warningColor = Color.yellow;

        [SerializeField]
        Color errorColor = Color.red;

        [SerializeField, Range(10, 500)]
        int maxLogCount = 20;

        [SerializeField]
        Text consoleText;

        public string Text
        {
            get { return consoleText.text; }
            private set { consoleText.text = value; }
        }

        public void Log(string message)
        {
            message = SetColor(message, normalColor);
            AddLog(message);
        }

        public void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Log(message);
        }


        public void LogWarning(string message)
        {
            message = SetColor(message, warningColor);
            AddLog(message);
        }

        public void LogWarning(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogWarning(message);
        }


        public void LogError(string message)
        {
            message = SetColor(message, errorColor);
            AddLog(message);
        }

        public void LogError(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogError(message);
        }

        void AddLog(string message)
        {
            Text += "\n" + message;
        }

        string SetColor(string message, Color color)
        {
            string colStr = color.ToHex();
            message = "<color=" + colStr + ">" + message + "</color>";
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

    class ConsoleInput
    {
    }

}
