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
    internal sealed class ComponentInitializer : MonoBehaviour
    {
        private ComponentInitializer()
        {
        }

        private static readonly GlobalSingleton<ComponentInitializer> singleton = new GlobalSingleton<ComponentInitializer>();
        private IComponentInitializeHandle[] initializeHandlers;

        public static ComponentInitializer Instance
        {
            get { return singleton.GetInstance(); }
        }

        private void Awake()
        {
            singleton.SetInstance(this);
            initializeHandlers = GetComponentsInChildren<IComponentInitializeHandle>();
        }

        private void OnDestroy()
        {
            singleton.RemoveInstance(this);
        }

        public static Task Initialize()
        {
            return singleton.GetInstance().InternalInitialize();
        }

        private Task InternalInitialize()
        {
            return Task.Run(delegate ()
            {
                foreach (var initializeHandler in initializeHandlers)
                {
                    initializeHandler.Initialize();
                }
            });
        }
    }
}
