using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{
    /// <summary>
    /// 游戏组件初始化接口;
    /// </summary>
    public interface IComponentInitializeHandle
    {
        void Initialize(CancellationToken token);
    }

    /// <summary>
    /// 游戏组件初始化器;
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            singleton.RemoveInstance(this);
        }

        protected override IEnumerable<Action> EnumerateInitializeHandler(CancellationToken token)
        {
            foreach (var initializeHandle in initializeHandles)
            {
                yield return () => initializeHandle.Initialize(token);
            }
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            UnityDebugHelper.SuccessfulReport(InitializerName);
        }

        protected override void OnFaulted(AggregateException ex)
        {
            base.OnFaulted(ex);
            UnityDebugHelper.FailureReport(InitializerName, ex);
        }
    }
}
