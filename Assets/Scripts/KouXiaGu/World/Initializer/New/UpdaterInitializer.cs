using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.World
{

    public interface IUpdaterInitializer
    {
        Task StartInitialize(CancellationToken token);
    }

    /// <summary>
    /// 场景更新器初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class UpdaterInitializer : Initializer<IUpdaterInitializer>
    {
        UpdaterInitializer()
        {
        }

        [SerializeField]
        ComponentInitializer componentInitializer;

        protected override string InitializerName
        {
            get { return "[场景更新器初始化]"; }
        }

        protected override bool Prepare()
        {
            if (componentInitializer.IsCompleted)
            {
                if (!componentInitializer.IsFaulted)
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException("componentInitializer 失败!");
                }
            }
            return false;
        }

        protected override Task GetTask(IUpdaterInitializer initializer)
        {
            return initializer.StartInitialize(TokenSource.Token);
        }
    }
}
