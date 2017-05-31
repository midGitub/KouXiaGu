using System;
using UnityEngine;
using UniRx;
using KouXiaGu.Terrain3D;
using KouXiaGu.World.Map;
using KouXiaGu.Resources;

namespace KouXiaGu.World
{

    /// <summary>
    /// 负责初始化游戏场景;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldInitializer : MonoBehaviour, IObservable<IWorld>
    {
        WorldInitializer()
        {
        }

        static AsyncInitializer worldInitializer;

        /// <summary>
        /// 初始化时使用的世界信息;
        /// </summary>
        internal static IAsyncOperation<WorldInfo> WorldInfoReader { get; set; }

        /// <summary>
        /// 场景数据,若未初始化则为Null;
        /// </summary>
        public static IAsyncOperation<IWorld> WorldAsync
        {
            get { return worldInitializer; }
        }

        void Awake()
        {
            if (worldInitializer != null)
                throw new ArgumentException("存在多个初始化实例");

            worldInitializer = new AsyncInitializer(GameInitializer.Instance.GameDataInitialize);
        }

        void OnDestroy()
        {
            if (worldInitializer == null)
                throw new ArgumentException("场景实例已被销毁?");

            worldInitializer = null;
        }

        /// <summary>
        /// 当场景初始化完毕时调用,若已经初始化完毕则返回Null,并且调用委托;
        /// </summary>
        public IDisposable Subscribe(IObserver<IWorld> observer)
        {
            return worldInitializer.Subscribe(observer);
        }

        ///// <summary>
        ///// 异步初始化结构;
        ///// </summary>
        //class AsyncInitializer : AsyncOperation<IWorld>, IWorld, IObservable<IWorld>
        //{
        //    public AsyncInitializer(IAsyncOperation<BasicResource> gameDataInitializer)
        //    {
        //        stateSender = new AsyncResultSender<IWorld>(this);
        //        gameDataInitializer.Subscribe(Name + "等待游戏数据初始化完毕;", Initialize, OnInitializeFaulted);
        //    }

        //    readonly AsyncResultSender<IWorld> stateSender;
        //    public BasicResource BasicData { get; private set; }
        //    public WorldInfo Info { get; private set; }
        //    public IWorldData WorldData { get; private set; }
        //    public IWorldComponents Components { get; private set; }
        //    public IWorldUpdater Updater { get; private set; }

        //    /// <summary>
        //    /// 订阅到,若已经存在结果则返回Null,并且调用委托;
        //    /// </summary>
        //    public IDisposable Subscribe(IObserver<IWorld> observer)
        //    {
        //        return stateSender.Subscribe(observer);
        //    }

        //    void Initialize(IAsyncOperation<BasicResource> operation)
        //    {
        //        BasicData = operation.Result;
        //        WorldInfoReader.Subscribe(Name + "等待游戏世界信息读取完毕;", OnWorldInfoReadCompleted, OnInitializeFaulted);
        //    }

        //    void OnWorldInfoReadCompleted(IAsyncOperation<WorldInfo> operation)
        //    {
        //        Info = operation.Result;
        //        DataInitializer worldDataInitialize = new DataInitializer();
        //        worldDataInitialize.Start(BasicData, Info, this)
        //            .Subscribe(Name + "等待游戏世界数据初始化;", OnWorldDataCompleted, OnInitializeFaulted);
        //    }

        //    void OnWorldDataCompleted(IAsyncOperation<IWorldData> operation)
        //    {
        //        WorldData = operation.Result;
        //        Components = new WorldComponents(WorldData);
        //        StartSceneUpdater();
        //    }

        //    void StartSceneUpdater()
        //    {
        //        WorldUpdater updater = new WorldUpdater();
        //        Updater = updater;
        //        updater.Start(this).
        //            Subscribe(Name + "等待场景构建完成;", OnInitializeCompleted, OnInitializeFaulted);
        //    }

        //    void OnInitializeCompleted(IAsyncOperation operation)
        //    {
        //        IWorld world = this;
        //        OnCompleted(world);
        //        stateSender.Send(world);
        //        stateSender.SendCompleted();
        //        Debug.Log("游戏开始!");
        //    }

        //    void OnInitializeFaulted(IAsyncOperation operation)
        //    {
        //        Exception ex = operation.Exception;
        //        OnFaulted(ex);
        //        stateSender.SendError(ex);
        //        stateSender.SendCompleted();
        //        Debug.Log("游戏初始化失败!");
        //    }

        //}
    }
}
