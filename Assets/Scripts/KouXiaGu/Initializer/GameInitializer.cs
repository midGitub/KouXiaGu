using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 负责游戏程序初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GameInitializer : UnitySington<GameInitializer>
    {
        GameInitializer()
        {
        }

        List<Task> tasks;
        IInitializer[] initializers;
        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public AggregateException Exception { get; private set; }

        void Awake()
        {
            tasks = new List<Task>();
            initializers = GetComponentsInChildren<IInitializer>();
        }

        void Start()
        {
            StartInitialize();
        }

        async void StartInitialize()
        {
            if (IsCompleted)
            {
                return;
            }

            foreach (var initializer in initializers)
            {
                Task task = initializer.StartInitialize();
                tasks.Add(task);
            }
            Task whenAll = Task.WhenAll(tasks);
            await whenAll;

            if (whenAll.IsCompleted)
            {
                IsCompleted = true;
                if (whenAll.IsFaulted)
                {
                    IsFaulted = true;
                    Exception = whenAll.Exception;
                    Debug.LogError("[游戏初始化]时遇到错误:" + whenAll.Exception);
                }
                Debug.Log("[游戏初始化]完成;");
            }
        }
    }
}
