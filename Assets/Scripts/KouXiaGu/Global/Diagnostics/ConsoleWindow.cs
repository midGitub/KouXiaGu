using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Diagnostics
{

    /// <summary>
    /// 控制台UI;
    /// </summary>
    [DisallowMultipleComponent]
    sealed class ConsoleWindow : UnitySington<ConsoleWindow>
    {
        ConsoleWindow()
        {
        }

        public GameObject consoleWindows = null;
        public ScrollRect OutputScrollRect = null;
        public Text OutputConsoleText = null;
        public InputField InputField = null;
        public RichTextStyleConverter StyleConverter = null;
        LogRecorder logRecorder;

        void Awake()
        {
            SetInstance(this);
            logRecorder = new LogRecorder(StyleConverter);
            logRecorder.OnTextChanged += OnTextChanged;
            XiaGuConsole.Subscribe(logRecorder);
        }

        void OnTextChanged(LogRecorder recorder)
        {
            OutputConsoleText.text = recorder.GetText();
        }


    }
}
