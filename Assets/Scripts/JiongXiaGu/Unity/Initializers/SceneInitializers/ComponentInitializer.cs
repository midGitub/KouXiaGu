using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.World
{

    /// <summary>
    /// 游戏场景初始化器;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ComponentInitializer : Initializer<IComponentInitializeHandle>
    {
        ComponentInitializer()
        {
        }

        [SerializeField]
        DataInitializer dataInitializer;

        protected override string InitializerName
        {
            get { return "[场景组件初始化]"; }
        }

        protected override Task WaitPrepare(CancellationToken token)
        {
            return WaitInitializer(dataInitializer, token);
        }

        protected override Task GetTask(IComponentInitializeHandle initializer)
        {
            return initializer.StartInitialize(TokenSource.Token);
        }
    }
}
