using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Rx;

namespace KouXiaGu
{

    [Serializable]
    class ConsoleUI : IObserver<string>
    {
        public ScrollRect OutputScrollRect;
        public Text OutputConsoleText;
        public InputField InputField;

        IDisposable outputDisposer;

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

        public void Subscribe(IObservable<string> output)
        {
            outputDisposer = output.Subscribe(this);
        }

        public void Unsubscribe()
        {
            if (outputDisposer != null)
            {
                outputDisposer.Dispose();
                outputDisposer = null;
            }
        }

        void IObserver<string>.OnNext(string item)
        {
            OutputConsoleText.text = item;
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            Unsubscribe();
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

        ILogHandler defaultLogHandler;
        public ConsoleInput Input { get; private set; }
        public ConsoleOutput Output { get; private set; }

        public ConsoleOutputTextStyle OutputStype
        {
            get { return outputStype; }
            set { outputStype = value; }
        }

        void Awake()
        {
            instance = this;
            InitOutput();
            InitInput();

            defaultLogHandler = Debug.logger.logHandler;
            Debug.logger.logHandler = this;
        }

        void InitOutput()
        {
            Output = new ConsoleOutput(outputStype);
            Output.Text = ui.OutputText;
            ui.Subscribe(Output);
        }

        void InitInput()
        {
            Input = new ConsoleInput();
        }

        void Update()
        {
            UpdateInput();
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
                    ui.InputField.ActivateInputField();
                    ScrollToBottom();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    GameConsole.LogError("未知命令;");
                }
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

        void ScrollToBottom()
        {
            ui.OutputScrollRect.verticalNormalizedPosition = 0;
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
