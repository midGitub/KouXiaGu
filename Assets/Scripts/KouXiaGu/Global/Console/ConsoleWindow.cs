using System;
using KouXiaGu.KeyInput;
using UnityEngine;
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
    [ConsoleClass]
    sealed class ConsoleWindow : MonoBehaviour, ILogHandler
    {
        public static ConsoleWindow instance { get; private set; }


        #region 控制台命令;
        const string consoleDisplayUnityLog_KeyWord = "unityLog";

        [ConsoleMethod(consoleDisplayUnityLog_KeyWord, "输出 是否在控制台窗口显示Unity.Debug的日志;")]
        public static void ConsoleDisplayUnityLog()
        {
            bool isDisplay = instance.IsDisplayUnityLog;
            ConsoleDisplayUnityLog(isDisplay);
        }

        [ConsoleMethod(consoleDisplayUnityLog_KeyWord, "设置 是否在控制台窗口显示Unity.Debug的日志;")]
        public static void ConsoleDisplayUnityLog(string isDisplayStr)
        {
            bool isDisplay = Convert.ToBoolean(isDisplayStr);
            ConsoleDisplayUnityLog(isDisplay);
        }

        static void ConsoleDisplayUnityLog(bool isDisplay)
        {
            instance.IsDisplayUnityLog = isDisplay;
            GameConsole.Log(consoleDisplayUnityLog_KeyWord + " " + isDisplay);
        }
        #endregion


        [SerializeField]
        ConsoleUI ui;

        [SerializeField]
        ConsoleOutputTextStyle outputStype;

        [SerializeField]
        bool isShowUnityLog = true;

        [SerializeField]
        bool isDisplay = false;

        int inputContentRecordIndex;
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

        public bool IsDisplayUnityLog
        {
            get { return isShowUnityLog; }
            set { Debug.logger.logEnabled = value; isShowUnityLog = value; }
        }

        public bool IsDisplay
        {
            get { return isDisplay = gameObject.activeSelf; }
            private set { isDisplay = value; }
        }

        public string FinalInputContent
        {
            get { return Input.FinalInputContent; }
        }

        void Awake()
        {
            instance = this;
            InitOutput();
            InitInput();

            ResetInputContent();

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

        void LateUpdate()
        {
            UpdateOutputText();
            UpdateInput();
            UpdateInputMovement();
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
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                try
                {
                    if (ui.InputText != string.Empty)
                    {
                        Input.Operate(ui.InputText);
                        ResetInputContent();
                    }
                    else
                    {
                        ui.InputText = FinalInputContent;
                    }
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


        void UpdateInputMovement()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputContentRecordIndex = Math.Max(--inputContentRecordIndex, 0);
                InputContentUpdate();
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputContentRecordIndex = Math.Min(++inputContentRecordIndex, Input.RecordCount);
                InputContentUpdate();
            }
        }

        void ResetInputContent()
        {
            inputContentRecordIndex = Input.RecordCount;
            ui.InputText = string.Empty;
        }

        void InputContentUpdate()
        {
            if (inputContentRecordIndex < Input.RecordCount)
            {
                ui.InputText = Input[inputContentRecordIndex];
            }
            ui.InputField.MoveTextEnd(false);
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
