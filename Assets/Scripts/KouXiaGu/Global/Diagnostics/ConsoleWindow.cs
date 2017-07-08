using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.InputControl;
using KouXiaGu.UI;

namespace KouXiaGu.Diagnostics
{

    /// <summary>
    /// 控制台UI;
    /// </summary>
    [RequireComponent(typeof(OrderedPanel))]
    [DisallowMultipleComponent]
    sealed class ConsoleWindow : UnitySington<ConsoleWindow>
    {
        ConsoleWindow()
        {
        }

        public GameObject consoleWindows = null;
        public ScrollRect OutputScrollRect = null;
        public Text OutputTextObject = null;
        public InputField InputField = null;
        public RichTextStyleConverter StyleConverter = null;
        LogRecorder logRecorder;
        Recorder<string> inputRecorder;
        IKeyInput keyInput;
        public OrderedPanel Panel { get; private set; }

        void Awake()
        {
            SetInstance(this);

            logRecorder = new LogRecorder(StyleConverter);
            logRecorder.Append(OutputTextObject.text);
            logRecorder.OnTextChanged += OnTextChanged;
            XiaGuConsole.Subscribe(logRecorder);

            inputRecorder = new Recorder<string>();

            Panel = GetComponent<OrderedPanel>();
            Panel.OnFocus.AddListener(OnFocus);
        }

        void OnEnable()
        {
            keyInput = KeyInput.OccupiedInput.Subscribe(this);
        }

        void OnFocus()
        {
            if (keyInput != null)
            {
                keyInput.Dispose();
            }
            keyInput = KeyInput.OccupiedInput.Subscribe(this);
        }

        void OnDisable()
        {
            if (keyInput != null)
            {
                keyInput.Dispose();
                keyInput = null;
            }
        }

        void Update()
        {
            if (keyInput.IsActivating)
            {
                if (keyInput.GetKeyDown(KeyCode.Return) || keyInput.GetKeyDown(KeyCode.KeypadEnter))
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
                if (keyInput.GetKeyDown(KeyCode.UpArrow))
                {
                    InputField.text = inputRecorder.GetPrevious();
                    InputField.MoveTextEnd(false);
                }
                if (keyInput.GetKeyDown(KeyCode.DownArrow))
                {
                    InputField.text = inputRecorder.GetNext();
                    InputField.MoveTextEnd(false);
                }
                if (keyInput.GetKeyDown(KeyCode.Escape)) 
                {
                    Panel.HidePanel();
                }
            }
        }

        void OnTextChanged(LogRecorder recorder)
        {
            OutputTextObject.text = recorder.GetText();
        }
    }
}
