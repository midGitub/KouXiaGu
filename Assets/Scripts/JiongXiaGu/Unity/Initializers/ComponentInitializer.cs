using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{
    /// <summary>
    /// 游戏组件初始化接口;如 按键配置,基础语言包
    /// </summary>
    public interface IComponentInitializeHandle
    {
        void Initialize();
    }

    /// <summary>
    /// 游戏底层组件初始化器;
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class ComponentInitializer : InitializerBase
    {
        private ComponentInitializer()
        {
        }

        private static readonly GlobalSingleton<ComponentInitializer> singleton = new GlobalSingleton<ComponentInitializer>();
        public static ComponentInitializer Instance => singleton.GetInstance();
        private IComponentInitializeHandle[] initializeHandlers;

        private void Awake()
        {
            singleton.SetInstance(this);
            initializeHandlers = GetComponentsInChildren<IComponentInitializeHandle>();
        }

        public static Task StartInitialize()
        {
            return Instance.Initialize();
        }

        protected override List<Func<CancellationToken, string>> EnumerateHandler(object state)
        {
            var handlers = new List<Func<CancellationToken, string>>();

            foreach (var handler in initializeHandlers)
            {
                handlers.Add(delegate (CancellationToken token)
                {
                    token.ThrowIfCancellationRequested();

                    handler.Initialize();
                    return null;
                });
            }

            return handlers;
        }
    }
}
