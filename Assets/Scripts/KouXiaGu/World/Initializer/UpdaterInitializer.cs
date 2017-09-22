using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 场景更新器初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class UpdaterInitializer : Initializer<IUpdaterInitializeHandle>
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

        protected override Task WaitPrepare(CancellationToken token)
        {
            return WaitInitializer(componentInitializer, token);
        }

        protected override Task GetTask(IUpdaterInitializeHandle initializer)
        {
            return initializer.StartInitialize(TokenSource.Token);
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            var handles = GetComponentsInChildren<IWorldCompletedHandle>();
            foreach (var handle in handles)
            {
                handle.OnWorldCompleted();
            }
        }
    }
}
