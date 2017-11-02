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
                List<ConsoleMethodReflected> faultList = new List<ConsoleMethodReflected>();

                foreach (var consoleMethodState in consoleMethodStates)
                {
                    if (consoleMethodState.IsFaulted)
                    {
                        faultList.Add(consoleMethodState);
                    }
                    else
                    {
                        if (!GameConsole.MethodSchema.TryAdd(consoleMethodState.ConsoleMethod))
                        {
                            throw new NotImplementedException();
                        }
                    }
                }

                OnComplete(faultList);
            });
        }

        private void OnComplete(List<ConsoleMethodReflected> faultList)
        {
            EditorHelper.LogComplete("控制台",() => GetConsoleInfo(faultList));
        }

        public string GetConsoleInfo(List<ConsoleMethodReflected> faultList)
        {
            string info = string.Format("方法总数:{0}", GameConsole.MethodSchema.Count);
            return info;
        }
    }
}
