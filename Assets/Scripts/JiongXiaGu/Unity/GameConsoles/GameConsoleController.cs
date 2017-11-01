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
                var consoleMethods = reflector.Search(typeof(GameConsoleController).Assembly);

                foreach (var consoleMethod in consoleMethods)
                {
                    Debug.Log(consoleMethod.MethodInfo.Name);
                }

                OnComplete();
            });
        }

        private void OnComplete()
        {
            EditorHelper.LogComplete("控制台", GetConsoleInfo);
        }

        public string GetConsoleInfo()
        {
            string info = string.Format("方法总数:{0}", GameConsole.MethodSchema.Count);
            return info;
        }
    }
}
