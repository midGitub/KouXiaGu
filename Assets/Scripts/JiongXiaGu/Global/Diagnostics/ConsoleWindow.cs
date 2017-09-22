using UnityEngine;
using UnityEngine.UI;
using JiongXiaGu.InputControl;
using JiongXiaGu.UI;

namespace JiongXiaGu.Diagnostics
{

    /// <summary>
    /// 控制台UI;
    /// </summary>
    [RequireComponent(typeof(SelectablePanel))]
    [DisallowMultipleComponent]
    sealed class ConsoleWindow : UnitySington<ConsoleWindow>
    {
        ConsoleWindow()
        {
        }

        LogRecorder logRecorder;
        Recorder<string> inputRecorder;
        IKeyInput keyInput;
        bool isTextHasChanged = false;
        public ScrollRect OutputScrollRect = null;
        public Text OutputTextObject = null;
        public InputField InputField = null;
        public RichTextStyleConverter StyleConverter = null;
        public SelectablePanel Panel { get; private set; }

        public bool IsFocus
        {
            get { return Panel.IsFocus; }
        }

        void Awake()
        {
            SetInstance(this);

            logRecorder = new LogRecorder(StyleConverter);
            logRecorder.Append(OutputTextObject.text);
            logRecorder.OnTextChanged += OnTextChanged;
            XiaGuConsole.Subscribe(logRecorder);

            inputRecorder = new Recorder<string>();

            Panel = GetComponent<SelectablePanel>();
            Panel.OnFocusEvent += OnFocus;
            Panel.OnBlurEvent += OnBlur;
        }

        void OnFocus()
        {
            if (keyInput != null)
            {
                keyInput.Dispose();
            }
            keyInput = KeyInput.OccupiedInput.Subscribe(this);
        }

        void OnBlur()
        {
            if (keyInput != null)
            {
                keyInput.Dispose();
                keyInput = null;
            }
        }

        void LateUpdate()
        {
            if (isTextHasChanged)
            {
                OutputScrollRect.verticalScrollbar.value = 0;
                isTextHasChanged = false;
            }
            if (IsFocus)
            {
                if (keyInput.GetKeyDown(KeyCode.Return) || keyInput.GetKeyDown(KeyCode.KeypadEnter))
                {
                    Operation();
                }
                if (keyInput.GetKeyDown(KeyCode.UpArrow))
                {
                    if (inputRecorder.Count != 0)
                    {
                        InputField.text = inputRecorder.GetPrevious();
                        InputField.MoveTextEnd(false);
                    }
                }
                if (keyInput.GetKeyDown(KeyCode.DownArrow))
                {
                    if (inputRecorder.Count != 0)
                    {
                        InputField.text = inputRecorder.GetNext();
                        InputField.MoveTextEnd(false);
                    }
                }
                if (keyInput.GetKeyDown(KeyCode.Escape))
                {
                    Panel.Hide();
                }
            }
        }

        public void Operation()
        {
            string message = InputField.text;
            if (message != string.Empty)
            {
                inputRecorder.Add(message);
                XiaGuConsole.Operate(message);
                InputField.text = string.Empty;
            }
            InputField.ActivateInputField();
        }

        void OnTextChanged(LogRecorder recorder)
        {
            OutputTextObject.text = recorder.GetText();
            isTextHasChanged = true;
        }
    }
}
