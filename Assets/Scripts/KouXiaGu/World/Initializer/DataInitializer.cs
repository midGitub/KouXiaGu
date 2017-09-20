using KouXiaGu.Concurrent;
using KouXiaGu.Resources.Archives;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.World
{

    public interface IDataInitializer
    {
        Task StartInitialize(Archive archive, CancellationToken token);
    }

    /// <summary>
    /// 游戏场景信息初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DataInitializer : Initializer<IDataInitializer>
    {
        DataInitializer()
        {
        }

        /// <summary>
        /// 存档信息,在初始化之前赋值,若未Null则初始化异常;
        /// </summary>
        public static Archive Archive { get; set; }

        /// <summary>
        /// 默认的存档,在未指定存档时使用的存档;
        /// </summary>
        [SerializeField]
        Archive defaultArchive;
        GameInitializer gameInitializer;

        protected override string InitializerName
        {
            get { return "[场景数据初始化]"; }
        }

        protected override void Awake()
        {
            base.Awake();
            gameInitializer = GlobalController.GetSington<GameInitializer>();
        }

        protected override bool Prepare()
        {
            if (gameInitializer.IsCompleted)
            {
                if (!gameInitializer.IsFaulted)
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException("gameInitializer 初始化失败;");
                }
            }
            return false;
        }

        protected override Task GetTask(IDataInitializer initializer)
        {
            return initializer.StartInitialize(Archive, TokenSource.Token);
        }
    }
}
