using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.InputControl
{


    public class KeyInputInitializer : MonoBehaviour, IInitializer
    {
        KeyInputInitializer()
        {
        }

        Task IInitializer.StartInitialize()
        {
            return Task.Run(delegate ()
            {
                KeyInput.Initialize();
                OnCustomInputCompleted();
            });
        }

        [Conditional("EDITOR_LOG")]
        void OnCustomInputCompleted()
        {
            const string prefix = "[输入组件]";
            var emptyKeys = KeyInput.KeyMap.GetEmptyKeys().ToList();
            if (emptyKeys.Count != 0)
            {
                UnityEngine.Debug.LogWarning(prefix + " 初始化完成;存在未定义的按键:" + emptyKeys.ToLog());
            }
            else
            {
                UnityEngine.Debug.Log(prefix + "初始化完成;");
            }
        }

    }
}
