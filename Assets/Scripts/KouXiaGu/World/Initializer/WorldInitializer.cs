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
    public class WorldInitializer : MonoBehaviour, IWorld, IObservable<IWorld>
    {
        /// <summary>
        /// 初始化时使用的世界信息;
        /// </summary>
        public static IAsyncOperation<WorldInfo> WorldInfoReader { get; set; }


        WorldInitializer()
        {
        }

        LinkedListTracker<IWorld> worldTracker;
        event Action<IWorld> onInitializeCompleted;
        DataInitializer worldDataInitialize;
        ComponentInitializer sceneComponentInitializer;
        SceneInitializer sceneInitializer;
        public IGameData GameData { get; private set; }
        public WorldInfo Info { get; private set; }
        public IWorldData Data { get; private set; }
        public IWorldComponent Component { get; private set; }
        public IWorldScene Scene { get; private set; }

        public event Action<IWorld> OnInitializeCompleted
        {
            add { onInitializeCompleted += value; }
            remove { onInitializeCompleted -= value; }
        }

        void Awake()
        {
            worldTracker = new LinkedListTracker<IWorld>();
            worldDataInitialize = new DataInitializer();
            sceneComponentInitializer = new ComponentInitializer();
            sceneInitializer = new SceneInitializer();
            GameInitializer.Instance.GameDataInitialize.SubscribeCompleted(this, Initialize);
        }

        public IDisposable Subscribe(IObserver<IWorld> observer)
        {
            return worldTracker.Subscribe(observer);
        }

        void Initialize(IAsyncOperation<IGameData> operation)
        {
            GameData = operation.Result;
            WorldInfoReader.SubscribeCompleted(this, OnWorldInfoReadCompleted);
        }

        void OnWorldInfoReadCompleted(IAsyncOperation<WorldInfo> operation)
        {
            Info = operation.Result;
            worldDataInitialize.Start(GameData, Info, this).SubscribeCompleted(this, OnWorldDataCompleted);
        }

        void OnWorldDataCompleted(IAsyncOperation<IWorldData> operation)
        {
            Data = operation.Result;
            sceneComponentInitializer.Start(Data, this).SubscribeCompleted(this, OnSceneComponentCompleted);
        }

        void OnSceneComponentCompleted(IAsyncOperation<IWorldComponent> operation)
        {
            Component = operation.Result;
            sceneInitializer.Start(Data, Component, this).SubscribeCompleted(this, OnSceneCompleted);
        }

        void OnSceneCompleted(IAsyncOperation<IWorldScene> operation)
        {
            Scene = operation.Result;
            _OnInitializeCompleted();
        }

        void _OnInitializeCompleted()
        {
            worldTracker.Track(this);

            if (onInitializeCompleted != null)
                onInitializeCompleted(this);

            Debug.Log("游戏开始!");
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
