using JiongXiaGu.Unity.Initializers;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// 输入组件控制;
    /// </summary>
    [DisallowMultipleComponent]
    class KeyInputController : MonoBehaviour, IGameComponentInitializeHandle
    {
        KeyInputController()
        {
        }

        Task IGameComponentInitializeHandle.Initialize(CancellationToken token)
        {
            return Task.Run(delegate()
            {
                token.ThrowIfCancellationRequested();
                KeyMapReader keyMapReader = new KeyMapReader();
                KeyInput.CurrentKeyMap = keyMapReader.Read();
                OnInitializeCompleted();
            }, token);
        }

        [System.Diagnostics.Conditional("EDITOR_LOG")]
        private void OnInitializeCompleted()
        {
            EditorHelper.LogComplete("按键映射组件", GetInfoLog());
        }

        private string GetInfoLog()
        {
            string log = "定义按键总数:" + KeyInput.CurrentKeyMap.Count;
            return log;
        }

        [ContextMenu("报告详细信息")]
        private void LogInfo()
        {
            Debug.Log(GetInfoLog());
        }

        [ContextMenu("输出默认按键模版")]
        void WriteTemplateDefaultKeyMap()
        {
            KeyMap keyMap = new KeyMap()
            {
                { "Up", new CombinationKey(KeyCode.UpArrow)},
                { "Down", new CombinationKey(KeyCode.DownArrow)},
                { "Save", new CombinationKey(KeyCode.LeftControl, KeyCode.S)}
            };

            DefaultKeyConfigReader reader = new DefaultKeyConfigReader();
            reader.Write(keyMap.ToArray(), true);
        }
    }
}
