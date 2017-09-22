using JiongXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu
{

    /// <summary>
    /// 负责游戏程序初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GameInitializer : Initializer<IGameInitializeHandle>
    {
        GameInitializer()
        {
        }

        protected override string InitializerName
        {
            get { return "[游戏初始化]"; }
        }

        protected override Task GetTask(IGameInitializeHandle initializer)
        {
            return initializer.StartInitialize(TokenSource.Token);
        }
    }
}
