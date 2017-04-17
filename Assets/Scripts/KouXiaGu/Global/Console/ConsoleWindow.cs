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
        public static ConsoleWindow instance { get; private set; }


        [SerializeField]
        ScrollRect outputScrollRect;

        [SerializeField]
        Text outputConsoleText;

        [SerializeField]
        InputField inputField;

        [SerializeField]
        ConsoleOutput output;

        ILogHandler defaultLogHandler;
        float scrollbarVertical_Size;

        public ConsoleInput Input { get; private set; }

        public ConsoleOutput Output
        {
            get { return output; }
        }

        void Awake()
        {
            instance = this;

            defaultLogHandler = Debug.logger.logHandler;
            Debug.logger.logHandler = this;

            scrollbarVertical_Size = outputScrollRect.verticalScrollbar.size;
            output.Text = outputConsoleText.text;

            Input = new ConsoleInput();
        }

        void Update()
        {
            if (outputConsoleText.text != output.Text)
            {
                outputConsoleText.text = output.Text;
            }

            float currentSize = outputScrollRect.verticalScrollbar.size;
            if (scrollbarVertical_Size != currentSize)
            {
                ScrollToBottom();
                scrollbarVertical_Size = currentSize;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) && inputField.text != string.Empty)
            {
                try
                {
                    Input.Operate(inputField.text);
                }
                catch (Exception ex)
                {
                    GameConsole.LogError(ex);
                }
                inputField.text = string.Empty;
                inputField.ActivateInputField();
            }
        }

        void OnEnable()
        {
            ScrollToBottom();
        }

        void OnDestroy()
        {
            Debug.logger.logHandler = defaultLogHandler;
            instance = null;
        }

        void OnScrollValueChanged(Vector2 v2)
        {
            float currentSize = outputScrollRect.verticalScrollbar.size;
            if (scrollbarVertical_Size != currentSize)
            {
                ScrollToBottom();
                scrollbarVertical_Size = currentSize;
            }
        }

        void ScrollToBottom()
        {
            outputScrollRect.verticalNormalizedPosition = 0;
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
