﻿using JiongXiaGu.Unity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.InputControl
{


    public class KeyInputInitializer : MonoBehaviour, IGameInitializeHandle
    {
        KeyInputInitializer()
        {
        }

        Task IGameInitializeHandle.StartInitialize(CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                KeyInput.Initialize();
                OnCustomInputCompleted();
            }, token);
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
