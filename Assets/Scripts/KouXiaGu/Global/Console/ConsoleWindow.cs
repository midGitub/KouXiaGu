using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu
{

    /// <summary>
    /// 运行状态日志输出窗口;
    /// </summary>
    [DisallowMultipleComponent]
    sealed class ConsoleWindow : MonoBehaviour, ILogHandler
    {

        [SerializeField]
        ScrollRect scrollRect;

        [SerializeField]
        Text consoleText;

        [SerializeField]
        ConsoleOutput output;

        ILogHandler defaultLogHandler;
        float scrollbarVertical_Size;

        public ConsoleOutput Output
        {
            get { return output; }
        }

        void Awake()
        {
            defaultLogHandler = Debug.logger.logHandler;
            Debug.logger.logHandler = this;

            scrollbarVertical_Size = scrollRect.verticalScrollbar.size;
            output.Text = consoleText.text;
        }

        void Update()
        {
            if (consoleText.text != output.Text)
            {
                consoleText.text = output.Text;
            }

            float currentSize = scrollRect.verticalScrollbar.size;
            if (scrollbarVertical_Size != currentSize)
            {
                ScrollToBottom();
                scrollbarVertical_Size = currentSize;
            }
        }

        void OnEnable()
        {
            ScrollToBottom();
        }

        void OnDestroy()
        {
            Debug.logger.logHandler = defaultLogHandler;
        }

        void OnScrollValueChanged(Vector2 v2)
        {
            float currentSize = scrollRect.verticalScrollbar.size;
            if (scrollbarVertical_Size != currentSize)
            {
                ScrollToBottom();
                scrollbarVertical_Size = currentSize;
            }
        }

        void ScrollToBottom()
        {
            scrollRect.verticalNormalizedPosition = 0;
        }

        void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
        {
            (output as ILogHandler).LogException(exception, context);
            defaultLogHandler.LogException(exception, context);
        }

        void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            (output as ILogHandler).LogFormat(logType, context, format, args);
            defaultLogHandler.LogFormat(logType, context, format, args);
        }

    }

}
