using System;
using UnityEngine;
using UniRx;
using KouXiaGu.Terrain3D;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
{

    /// <summary>
    /// 负责初始化游戏场景;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldInitializer : MonoBehaviour, IWorld, IObservable<IWorld>, IResultSend<IWorld>
    {
        const string Name = "WorldInitializer";
        /// <summary>
        /// 初始化时使用的世界信息;
        /// </summary>
        public static IAsyncOperation<WorldInfo> WorldInfoReader { get; set; }


        WorldInitializer()
        {
        }

        DataInitializer worldDataInitialize;
        SceneInitializer sceneInitializer;
        public IGameData GameData { get; private set; }
        public WorldInfo Info { get; private set; }
        public IWorldData Data { get; private set; }
        public IWorldScene Component { get; private set; }

        LinkedListTracker<IWorld> worldTracker;

        void Awake()
        {
            worldTracker = new LinkedListTracker<IWorld>();
            worldDataInitialize = new DataInitializer();
            sceneInitializer = new SceneInitializer();
            GameInitializer.Instance.GameDataInitialize.SubscribeCompleted(Name + "等待游戏数据初始化完毕;", Initialize);
        }

        IDisposable IResultSend<IWorld>.SubscribeResult(Action<IWorld> action)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<IWorld> observer)
        {
            return worldTracker.Subscribe(observer);
        }

        void Initialize(IAsyncOperation<IGameData> operation)
        {
            GameData = operation.Result;
            WorldInfoReader.SubscribeCompleted(Name + "等待游戏世界信息读取完毕;", OnWorldInfoReadCompleted);
        }

        void OnWorldInfoReadCompleted(IAsyncOperation<WorldInfo> operation)
        {
            Info = operation.Result;
            worldDataInitialize.Start(GameData, Info, this)
                .SubscribeCompleted(Name + "等待游戏世界数据初始化;", OnWorldDataCompleted);
        }

        void OnWorldDataCompleted(IAsyncOperation<IWorldData> operation)
        {
            Data = operation.Result;
            sceneInitializer.Start(Data, this)
                .SubscribeCompleted(Name + "等待游戏世界组件初始化;", OnSceneCompleted);
        }

        void OnSceneCompleted(IAsyncOperation<IWorldScene> operation)
        {
            Component = operation.Result;
            _OnInitializeCompleted();
        }

        void _OnInitializeCompleted()
        {
            worldTracker.Track(this);
            Debug.Log("游戏开始!");
        }


        class AsyncInitializer : AsyncOperation<IWorld>, IWorld, IObservable<IWorld>
        {
            public AsyncInitializer()
            {
                worldSender = new ResultSend<IWorld>(this);
                worldDataInitialize = new DataInitializer();
                sceneInitializer = new SceneInitializer();
            }

            readonly ResultSend<IWorld> worldSender;
            readonly DataInitializer worldDataInitialize;
            readonly SceneInitializer sceneInitializer;
            public IGameData GameData { get; private set; }
            public WorldInfo Info { get; private set; }
            public IWorldData Data { get; private set; }
            public IWorldScene Component { get; private set; }

            public IDisposable Subscribe(IObserver<IWorld> observer)
            {
                return worldSender.Subscribe(observer);
            }

            void Initialize(IAsyncOperation<IGameData> operation)
            {
                GameData = operation.Result;
                WorldInfoReader.Subscribe(Name + "等待游戏世界信息读取完毕;", OnWorldInfoReadCompleted, OnInitializeFaulted);
            }

            void OnWorldInfoReadCompleted(IAsyncOperation<WorldInfo> operation)
            {
                Info = operation.Result;
                worldDataInitialize.Start(GameData, Info, this)
                    .Subscribe(Name + "等待游戏世界数据初始化;", OnWorldDataCompleted, OnInitializeFaulted);
            }

            void OnWorldDataCompleted(IAsyncOperation<IWorldData> operation)
            {
                Data = operation.Result;
                sceneInitializer.Start(Data, this)
                    .Subscribe(Name + "等待游戏世界组件初始化;", OnSceneCompleted, OnInitializeFaulted);
            }

            void OnSceneCompleted(IAsyncOperation<IWorldScene> operation)
            {
                Component = operation.Result;
                OnInitializeCompleted();
            }

            void OnInitializeCompleted()
            {
                OnCompleted(this);
                Debug.Log("游戏开始!");
            }

            void OnInitializeFaulted(IAsyncOperation operation)
            {
                OnFaulted(operation.Exception);
                Debug.Log("游戏初始化失败!");
            }

        }

        class ResultSend<T> : Sender<T>
            where T : class
        {
            public ResultSend(IAsyncOperation<T> asyncOperation)
            {
                this.asyncOperation = asyncOperation;
            }

            public IAsyncOperation<T> asyncOperation;

            /// <summary>
            /// 订阅到,若已经存在结果则返回Null,并且调用委托;
            /// </summary>
            public override IDisposable Subscribe(IObserver<T> observer)
            {
                if (asyncOperation.IsFaulted)
                {
                    observer.OnError(asyncOperation.Exception);
                }
                else if (asyncOperation.IsCompleted)
                {
                    observer.OnNext(asyncOperation.Result);
                }
                else
                {
                    return base.Subscribe(observer);
                }
                observer.OnCompleted();
                return null;
            }
        }


        [ContextMenu("输出模版文件")]
        void Test()
        {
            WorldElementTemplate item = new WorldElementTemplate();
            item.WriteToDirectory(GameFile.MainDirectory, false);
        }

        [ContextMenu("检查")]
        void Test2()
        {
            WorldElementResource item = WorldElementResource.Read();

            RoadInfo info;
            if (item.RoadInfos.TryGetValue(2, out info))
            {
                Debug.Log((info.Terrain == null).ToString());
            }
        }

    }


}
