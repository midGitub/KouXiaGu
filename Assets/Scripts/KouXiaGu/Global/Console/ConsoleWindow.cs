using System;
using KouXiaGu.KeyInput;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu
{

    [Serializable]
    class ConsoleUI
    {
        public GameObject consoleWindows = null;
        public ScrollRect OutputScrollRect = null;
        public Text OutputConsoleText = null;
        public InputField InputField = null;
        [SerializeField]
        bool isDisplay = false;

        public bool IsDisplay
        {
            get { return consoleWindows.activeSelf; }
        }

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

        public void OnValidate()
        {
            SetDisplay(isDisplay);
        }

        public void SetDisplay(bool isDisplay)
        {
            if(consoleWindows != null)
                consoleWindows.SetActive(isDisplay);
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
    sealed class ConsoleWindow : UnitySington<ConsoleWindow>, ILogHandler
    {

        #region 控制台命令;
        const string consoleDisplayUnityLog_KeyWord = "UnityLog";

        [ConsoleMethod(consoleDisplayUnityLog_KeyWord, "输出 是否在控制台窗口显示Unity.Debug的日志;")]
        public static void ConsoleDisplayUnityLog()
        {
            bool isDisplay = Instance.IsDisplayUnityLog;
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
            Instance.SetDisplayUnityLog(isDisplay);
            if (isDisplay)
                GameConsole.LogSuccessful("Display unity log;");
            else
                GameConsole.LogSuccessful("Hide unity log;");
        }
        #endregion

        #region 

        public static readonly ILogHandler defaultLogHandler = Debug.logger.logHandler;

        static void RecoveryDefaultLogHandler()
        {
            Debug.logger.logHandler = defaultLogHandler;
        }

        #endregion


        [SerializeField]
        ConsoleUI ui;
        [SerializeField]
        ConsoleOutputTextStyle outputStype;
        [SerializeField]
        bool isShowUnityLog = true;

        int inputContentRecordIndex;
        float uiScrollSize;
        KeyDownObserver displayKeyObserver;
        public ConsoleInput Input { get; private set; }
        public ConsoleOutput Output { get; private set; }

        public ConsoleOutputTextStyle OutputStype
        {
            get { return outputStype; }
            private set { outputStype = value; }
        }

        public bool IsDisplayUnityLog
        {
            get { return isShowUnityLog; }
            private set { isShowUnityLog = value; }
        }

        public string FinalInputContent
        {
            get { return Input.FinalInputContent; }
        }

        void Awake()
        {
            SetInstance(this);

            Output = new ConsoleOutput(outputStype);
            Output.Text = ui.OutputText;
            Input = new ConsoleInput();
            ResetInputContent();
            SetDisplayUnityLog(isShowUnityLog);

            displayKeyObserver = new KeyDownObserver(KeyFunction.Console_DisplayOrHide, OnDisplayKeyDown);
            displayKeyObserver.SubscribeUpdate(this);
        }

        void SetDisplayUnityLog(bool isDisplay)
        {
            if (isDisplay)
            {
                Debug.logger.logHandler = this;
            }
            else
            {
                RecoveryDefaultLogHandler();
            }
            IsDisplayUnityLog = isDisplay;
        }

        void OnDestroy()
        {
            RecoveryDefaultLogHandler();
        }

        void OnValidate()
        {
            ui.OnValidate();
            //SetDisplayUnityLog(IsDisplayUnityLog);
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
                    string errorStr = "命令出现异常:" + ex;
                    GameConsole.LogError(errorStr);
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

        void OnDisplayKeyDown()
        {
           ui.SetDisplay(!ui.IsDisplay);
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
