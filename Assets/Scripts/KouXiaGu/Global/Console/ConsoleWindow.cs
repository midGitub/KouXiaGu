using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.KeyInput;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KouXiaGu
{

    [Serializable]
    class ConsoleUI
    {
        public ScrollRect OutputScrollRect;
        public Text OutputConsoleText;
        public InputField InputField;

        public string OutputText
        {
            get { return OutputConsoleText.text; }
            set { OutputConsoleText.text = value; }
        }

        public string InputText
        {
            get { return InputField.text; }
            set { InputField.text = value; }
        }

        public void ScrollToBottom()
        {
            OutputScrollRect.verticalNormalizedPosition = 0;
        }

    }

    /// <summary>
    /// 运行状态日志输出窗口;
    /// </summary>
    [DisallowMultipleComponent]
    sealed class ConsoleWindow : MonoBehaviour, ILogHandler
    {
        public static ConsoleWindow instance { get; private set; }

        [SerializeField]
        ConsoleUI ui;

        [SerializeField]
        ConsoleOutputTextStyle outputStype;

        [SerializeField]
        bool isShowUnityLog = true;

        [SerializeField]
        bool isDisplay = false;

        float uiScrollSize;
        ILogHandler defaultLogHandler;
        KeyDownObserver displayKeyObserver;
        public ConsoleInput Input { get; private set; }
        public ConsoleOutput Output { get; private set; }

        public ConsoleOutputTextStyle OutputStype
        {
            get { return outputStype; }
            set { outputStype = value; }
        }

        public bool IsShowUnityLog
        {
            get { return isShowUnityLog; }
            set { Debug.logger.logEnabled = value; isShowUnityLog = value; }
        }

        public bool IsDisplay
        {
            get { return isDisplay = gameObject.activeSelf; }
            private set { isDisplay = value; }
        }

        void Awake()
        {
            instance = this;
            InitOutput();
            InitInput();

            defaultLogHandler = Debug.logger.logHandler;
            Debug.logger.logHandler = this;
            Debug.logger.logEnabled = isShowUnityLog;

            displayKeyObserver = new KeyDownObserver(KeyFunction.Console_DisplayOrHide, OnDisplayKeyDown);
            displayKeyObserver.SubscribeUpdate();
        }

        void InitOutput()
        {
            Output = new ConsoleOutput(outputStype);
            Output.Text = ui.OutputText;
        }

        void InitInput()
        {
            Input = new ConsoleInput();
        }

        void Update()
        {
            UpdateOutputText();
            UpdateInput();
            UpdateScroll();
        }

        void UpdateOutputText()
        {
            if (ui.OutputText != Output.Text)
            {
                ui.OutputText = Output.Text;
            }
        }

        void UpdateInput()
        {
            if (ui.InputText != string.Empty
                && (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                try
                {
                    Input.Operate(ui.InputText);
                    ui.InputText = string.Empty;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    GameConsole.LogError("未知命令;");
                }
                finally
                {
                    ui.InputField.ActivateInputField();
                }
            }
        }

        void UpdateScroll()
        {
            float size = ui.OutputScrollRect.verticalScrollbar.size;
            if (uiScrollSize != size)
            {
                ui.ScrollToBottom();
                uiScrollSize = size;
            }
        }

        void OnEnable()
        {
            ui.InputField.ActivateInputField();
            ui.InputField.Select();
        }

        void OnValidate()
        {
            Debug.logger.logEnabled = isShowUnityLog;
            SetDisplay(isDisplay);
        }

        void OnDestroy()
        {
            Debug.logger.logHandler = defaultLogHandler;
            instance = null;
        }

        void OnDisplayKeyDown()
        {
            SetDisplay(!IsDisplay);
        }

        void SetDisplay(bool isDisplay)
        {
            gameObject.SetActive(isDisplay);
            IsDisplay = isDisplay;
        }

        void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
        {
            (Output as ILogHandler).LogException(exception, context);
            defaultLogHandler.LogException(exception, context);
        }

        void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            (Output as ILogHandler).LogFormat(logType, context, format, args);
            defaultLogHandler.LogFormat(logType, context, format, args);
        }

    }

}
