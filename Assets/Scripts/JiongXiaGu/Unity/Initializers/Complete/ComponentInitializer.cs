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
    internal sealed class ComponentInitializer : InitializeScheduler
    {
        private static readonly GlobalSingleton<ComponentInitializer> singleton = new GlobalSingleton<ComponentInitializer>();
        private const string InitializerName = "游戏组件初始化";
        private IComponentInitializeHandle[] initializeHandles;

        public static ComponentInitializer Instance
        {
            get { return singleton.GetInstance(); }
        }

        protected override void Awake()
        {
            base.Awake();
            singleton.SetInstance(this);
            initializeHandles = GetComponentsInChildren<IComponentInitializeHandle>();
        }

        private void Start()
        {
            StartInitialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            singleton.RemoveInstance(this);
        }

        protected override IEnumerable<Task> EnumerateInitializeHandler()
        {
            foreach (var initializeHandle in initializeHandles)
            {
                Task task = default(Task);
                try
                {
                    task = initializeHandle.Initialize(CancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    task = Task.FromException(ex);
                }
                yield return task;
            }
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            UnityDebugHelper.SuccessfulReport(InitializerName);
        }

        protected override void OnFaulted(Exception ex)
        {
            base.OnFaulted(ex);
            UnityDebugHelper.FailureReport(InitializerName, ex);
        }
    }
}
