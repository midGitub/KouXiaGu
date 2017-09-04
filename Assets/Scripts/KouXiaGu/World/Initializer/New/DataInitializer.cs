using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.World
{

    public interface IDataInitializer
    {
        Task StartInitialize(ArchiveFile archive, IOperationState state);
    }

    /// <summary>
    /// 游戏场景信息初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DataInitializer : SceneSington<DataInitializer>, IOperationState
    {
        DataInitializer()
        {
        }

        List<Task> tasks;
        IDataInitializer[] initializers;
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
            initializers = GetComponentsInChildren<IDataInitializer>();
        }

        /// <summary>
        /// 开始进行初始化;
        /// </summary>
        public async void StartInitialize(ArchiveFile archive)
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
                Task task = initializer.StartInitialize(archive, this);
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
