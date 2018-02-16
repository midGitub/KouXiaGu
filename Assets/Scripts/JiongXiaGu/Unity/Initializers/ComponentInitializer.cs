using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{
    /// <summary>
    /// 游戏组件初始化接口;如 按键配置,游戏控制台
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
        public static ComponentInitializer Instance => singleton.GetInstance();
        private IComponentInitializeHandle[] initializeHandlers;

        private void Awake()
        {
            singleton.SetInstance(this);
            initializeHandlers = GetComponentsInChildren<IComponentInitializeHandle>();
        }

        public void Initialize()
        {
            foreach (var handler in initializeHandlers)
            {
                handler.Initialize();
            }
        }
    }
}
