using JiongXiaGu.Unity.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 游戏控制台控制器;
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class GameConsoleController : MonoBehaviour, IGameComponentInitializeHandle
    {
        Task IGameComponentInitializeHandle.Initialize(CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                ConsoleMethodReflector reflector = new ConsoleMethodReflector();
                var consoleMethodStates = reflector.Search(typeof(GameConsoleController).Assembly);

                foreach (var consoleMethodState in consoleMethodStates)
                {
                    if (consoleMethodState.IsFaulted)
                    {
                        Debug.LogError(string.Format("控制台方法[{0}]出现异常:[{1}]", consoleMethodState.MethodInfo.Name, consoleMethodState.Exception));
                    }
                    else
                    {
                        if (!GameConsole.MethodSchema.TryAdd(consoleMethodState.ConsoleMethod))
                        {
                            Debug.LogError(string.Format("控制台方法[{0}]发生重复;", consoleMethodState.ConsoleMethod.Name));
                        }
                    }
                }

                OnComplete();
            });
        }

        private void OnComplete()
        {
            UnityDebugHelper.SuccessfulReport("控制台",() => GetConsoleInfo());
        }

        public string GetConsoleInfo()
        {
            string info = string.Format("方法总数:{0}", GameConsole.MethodSchema.Count);
            return info;
        }
    }
}
