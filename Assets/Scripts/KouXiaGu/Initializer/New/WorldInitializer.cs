using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏场景初始化器;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldInitializer : SceneSington<WorldInitializer>, IOperationState
    {
        WorldInitializer()
        {
        }

        List<Task> tasks;
        IInitializer[] initializers;
        internal Task InitializeTask { get; private set; }

        public bool IsCompleted
        {
            get { return InitializeTask != null ? InitializeTask.IsCompleted : false; }
        }

        public bool IsFaulted
        {
            get { return InitializeTask != null ? InitializeTask.IsFaulted : false; }
        }

        public AggregateException Exception
        {
            get { return InitializeTask != null ? InitializeTask.Exception : null; }
        }

        public bool IsRunning { get; private set; }
        public bool IsCanceled { get; private set; }

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

            IsRunning = true;

            GameInitializer gameInitializer = GameInitializer.Instance;
            while (!gameInitializer.IsCompleted)
            {
                await Task.Delay(500);
            }

            foreach (var initializer in initializers)
            {
                Task task = initializer.StartInitialize(this);
                tasks.Add(task);
            }

            InitializeTask = Task.WhenAll(tasks);
            await InitializeTask;

            if (InitializeTask.IsCompleted)
            {
                if (InitializeTask.IsFaulted)
                {
                    Debug.LogError("[场景初始化]时遇到错误:" + InitializeTask.Exception);
                }
                Debug.Log("[场景初始化]完成;");
            }

            IsRunning = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IsCanceled = true;
        }

        /// <summary>
        /// 取消初始化;
        /// </summary>
        public void CanceleInitialize()
        {
            IsCanceled = true;
        }
    }
}
