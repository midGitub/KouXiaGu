using JiongXiaGu.Concurrent;
using JiongXiaGu.Unity.Resources.Archives;
using JiongXiaGu.Unity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.World
{

    /// <summary>
    /// 游戏场景信息初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DataInitializer : Initializer<IDataInitializeHandle>
    {
        DataInitializer()
        {
        }

        /// <summary>
        /// 存档信息,在初始化之前赋值,若未Null则初始化异常;
        /// </summary>
        public static Archive Archive { get; set; }

        GameInitializer gameInitializer;

        protected override string InitializerName
        {
            get { return "[场景数据初始化]"; }
        }

        void Awake()
        {
            gameInitializer = GlobalController.GetSington<GameInitializer>();
        }

        protected override Task WaitPrepare(CancellationToken token)
        {
            return WaitInitializer(gameInitializer, token);
        }

        protected override Task GetTask(IDataInitializeHandle initializer)
        {
            return initializer.StartInitialize(Archive, TokenSource.Token);
        }
    }
}
