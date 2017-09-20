using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 负责游戏程序初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GameInitializer : Initializer<IGameInitializer>
    {
        GameInitializer()
        {
        }

        protected override string InitializerName
        {
            get { return "[游戏初始化]"; }
        }

        protected override Task GetTask(IGameInitializer initializer)
        {
            return initializer.StartInitialize(TokenSource.Token);
        }
    }
}
