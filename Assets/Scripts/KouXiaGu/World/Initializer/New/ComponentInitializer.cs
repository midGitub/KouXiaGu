using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.World
{

    public interface IComponentInitializer
    {
        Task StartInitialize(CancellationToken token);
    }

    /// <summary>
    /// 游戏场景初始化器;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ComponentInitializer : Initializer<IComponentInitializer>
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

        protected override bool Prepare()
        {
            if (dataInitializer.IsCompleted)
            {
                if (!dataInitializer.IsFaulted)
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException("worldDataInitializer 失败!");
                }
            }
            return false;
        }

        protected override Task GetTask(IComponentInitializer initializer)
        {
            return initializer.StartInitialize(TokenSource.Token);
        }
    }
}
