using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.World
{

    public interface IDataInitializer
    {
        Task StartInitialize(Archive archive, CancellationToken token);
    }

    /// <summary>
    /// 游戏场景信息初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DataInitializer : SceneSington<DataInitializer>
    {
        DataInitializer()
        {
        }

        /// <summary>
        /// 存档信息,在初始化之前赋值,若未Null则初始化异常;
        /// </summary>
        public static Archive Archive { get; set; }

        /// <summary>
        /// 默认的存档,在未指定存档时使用的存档;
        /// </summary>
        [SerializeField]
        Archive defaultArchive;

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
        public CancellationTokenSource TokenSource { get; private set; }

        void Awake()
        {
            tasks = new List<Task>();
            initializers = GetComponentsInChildren<IDataInitializer>();
            TokenSource = new CancellationTokenSource();
        }

        void Start()
        {
            StartInitialize();
        }

        /// <summary>
        /// 开始进行初始化;
        /// </summary>
        public async void StartInitialize()
        {
            if (IsCompleted)
            {
                return;
            }

            var archive = Archive;
            if (archive == null)
            {
                archive = defaultArchive;
            }

            IsRunning = true;

            GameInitializer gameInitializer = GameInitializer.Instance;
            while (!gameInitializer.IsCompleted)
            {
                await Task.Delay(500);
            }

            Debug.Log("[场景数据初始化]开始初始化;");
            foreach (var initializer in initializers)
            {
                Task task = initializer.StartInitialize(archive, TokenSource.Token);
                tasks.Add(task);
            }

            try
            {
                InitializeTask = Task.WhenAll(tasks);
                await InitializeTask;
                Debug.Log("[场景数据初始化]完成;");
            }
            catch
            {
                Debug.LogError("[场景数据初始化]时遇到错误:" + InitializeTask.Exception);
            }
            finally
            {
                IsRunning = false;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TokenSource.Cancel();
            Archive = null;
        }
    }
}
