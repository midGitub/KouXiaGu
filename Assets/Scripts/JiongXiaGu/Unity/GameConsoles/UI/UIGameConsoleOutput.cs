using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.GameConsoles.UI
{

    /// <summary>
    /// UI 控制台输出;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIGameConsoleOutput : MonoBehaviour
    {
        private UIGameConsoleOutput()
        {
        }

        [SerializeField]
        private bool displayUnityDebugLog = false;

        [SerializeField]
        [Range(100, 10000)]
        private int maxCapacity = 5000;

        [SerializeField]
        private ConsoleStringRichTextFormat format;

        [SerializeField]
        private Text output;

        [SerializeField]
        private Scrollbar scrollbarVertical;

        private bool updateScrollbarVerticalValue;
        private ConsoleStringBuilder consoleStringBuilder;
        private IDisposable unityDebugLogEventUnsubscriber;

        private void Awake()
        {
            StringBuilder stringBuilder = new StringBuilder(maxCapacity, maxCapacity);
            stringBuilder.Append(output.text);
            consoleStringBuilder = new ConsoleStringBuilder(stringBuilder, format);
            GameConsole.Subscribe(consoleStringBuilder);
            IsDisplayUnityDebugLog(displayUnityDebugLog);
        }

        private void LateUpdate()
        {
            string nowText;
            if (consoleStringBuilder.TryGetText(out nowText))
            {
                updateScrollbarVerticalValue = true;
                output.text = nowText;
            }
            else if(updateScrollbarVerticalValue)
            {
                updateScrollbarVerticalValue = false;
                scrollbarVertical.value = 0;
            }
        }

        private void OnValidate()
        {
            if (consoleStringBuilder != null)
            {
                IsDisplayUnityDebugLog(displayUnityDebugLog);
            }
        }

        public void IsDisplayUnityDebugLog(bool isDisplay)
        {
            if (isDisplay)
            {
                if (unityDebugLogEventUnsubscriber == null)
                {
                    unityDebugLogEventUnsubscriber = UnityDebug.Subscribe(consoleStringBuilder);
                }
            }
            else
            {
                if (unityDebugLogEventUnsubscriber != null)
                {
                    unityDebugLogEventUnsubscriber.Dispose();
                }
            }
        }

        [ContextMenu("ColorTest")]
        private void ColorTest()
        {
            GameConsole.Write("这是一条正常信息;");
            GameConsole.WriteError("这是一条错误信息;");
            GameConsole.WriteSuccessful("这是一条成功信息;");
            GameConsole.WriteWarning("这是一条警告信息;");
            GameConsole.WriteMethod("这是一条方法信息;");
        }
    }
}
