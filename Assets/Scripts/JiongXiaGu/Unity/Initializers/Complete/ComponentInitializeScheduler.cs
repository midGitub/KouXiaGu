using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{
    /// <summary>
    /// 游戏组件初始化接口;
    /// </summary>
    public interface IComponentInitializeHandle
    {
        /// <summary>
        /// 进行初始化,对于可进行异步的工作,通过异步Task进行;
        /// </summary>
        Task Initialize(CancellationToken token);
    }

    /// <summary>
    /// 游戏组件初始化调度器(仅初始化一次,若初始化失败意味着游戏无法运行);
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class ComponentInitializeScheduler : MonoBehaviour
    {
        private static readonly GlobalSingleton<ComponentInitializeScheduler> singleton = new GlobalSingleton<ComponentInitializeScheduler>();

        private const string InitializerName = "游戏组件初始化";
        private IComponentInitializeHandle[] initializeHandles;
        private CancellationTokenSource initializeCancellation;
        public Task InitializeTask { get; private set; }

        public static ComponentInitializeScheduler Instance
        {
            get { return singleton.GetInstance(); }
        }

        private void Awake()
        {
            singleton.SetInstance(this);
            initializeHandles = GetComponentsInChildren<IComponentInitializeHandle>();
            initializeCancellation = new CancellationTokenSource();
            InitializeTask = new Task(Initialize, initializeCancellation.Token);
        }

        private void Start()
        {
            InitializeTask.Start();
        }

        private void OnDestroy()
        {
            singleton.RemoveInstance(this);
            initializeCancellation.Cancel();
        }

        /// <summary>
        /// 进行初始化;
        /// </summary>
        private void Initialize()
        {
            try
            {
                InitializeScheduler.WaitAll(initializeHandles, item => item.Initialize(initializeCancellation.Token), initializeCancellation.Token);
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
                throw ex;
            }
        }

        private void OnCompleted()
        {
            UnityDebugHelper.SuccessfulReport(InitializerName);
        }

        private void OnFaulted(Exception ex)
        {
            UnityDebugHelper.FailureReport(InitializerName, ex);
            initializeCancellation.Cancel();
        }
    }
}
