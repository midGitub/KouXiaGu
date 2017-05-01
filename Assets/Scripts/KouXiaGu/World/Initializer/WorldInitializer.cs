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
    public class WorldInitializer : MonoBehaviour, IObservable<IWorld>
    {
        const string Name = "WorldInitializer";

        /// <summary>
        /// 初始化时使用的世界信息;
        /// </summary>
        public static IAsyncOperation<WorldInfo> WorldInfoReader { get; set; }


        WorldInitializer() { }

        AsyncInitializer gameWorldInitializer;

        /// <summary>
        /// 游戏场景初始化程序;
        /// </summary>
        public IAsyncOperation<IWorld> GameWorldInitializer
        {
            get { return gameWorldInitializer; }
        }

        void Awake()
        {
            gameWorldInitializer = new AsyncInitializer(GameInitializer.Instance.GameDataInitialize);
        }

        public IDisposable Subscribe(IObserver<IWorld> observer)
        {
            return gameWorldInitializer.Subscribe(observer);
        }

        class AsyncInitializer : AsyncOperation<IWorld>, IWorld, IObservable<IWorld>
        {
            public AsyncInitializer(IAsyncOperation<IGameData> gameDataInitializer)
            {
                stateSender = new ResultSend<IWorld>(this);
                worldDataInitialize = new DataInitializer();
                sceneInitializer = new SceneInitializer();
                gameDataInitializer.Subscribe(Name + "等待游戏数据初始化完毕;", Initialize, OnInitializeFaulted);
            }

            readonly ResultSend<IWorld> stateSender;
            readonly DataInitializer worldDataInitialize;
            readonly SceneInitializer sceneInitializer;
            public IGameData GameData { get; private set; }
            public WorldInfo Info { get; private set; }
            public IWorldData Data { get; private set; }
            public IWorldScene Component { get; private set; }

            public IDisposable Subscribe(IObserver<IWorld> observer)
            {
                return stateSender.Subscribe(observer);
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
                IWorld world = this;
                OnCompleted(world);
                stateSender.Send(world);
                stateSender.SendCompleted();
                Debug.Log("游戏开始!");
            }

            void OnInitializeFaulted(IAsyncOperation operation)
            {
                Exception ex = operation.Exception;
                OnFaulted(ex);
                stateSender.SendError(ex);
                stateSender.SendCompleted();
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
