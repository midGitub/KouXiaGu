using System;
using UnityEngine;
using UniRx;
using KouXiaGu.Terrain3D;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
{

    public interface IWorld
    {
        IWorldData Data { get; }
        IWorldComponent Component { get; }
        IWorldScene Scene { get; }
    }

    public interface IWorldData
    {
        IGameData GameData { get; }
        WorldInfo Info { get; }
        TimeManager Time { get; }
        MapResource Map { get; }
    }

    public interface IWorldComponent
    {
        Landform Landform { get; }
    }

    public interface IWorldScene
    {

    }

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

        [SerializeField]
        bool useEditorialInfo = false;

        [SerializeField]
        WorldInfo editorialInfo;

        ListTracker<IWorld> worldTracker;
        WorldDataInitializer worldDataInitialize;
        SceneComponentInitializer sceneComponentInitializer;
        SceneInitializer sceneInitializer;
        public IGameData GameData { get; private set; }
        public WorldInfo Info { get; private set; }
        public IWorldData Data { get; private set; }
        public IWorldComponent Component { get; private set; }
        public IWorldScene Scene { get; private set; }

        void Awake()
        {
            worldTracker = new ListTracker<IWorld>();
            worldDataInitialize = new WorldDataInitializer();
            sceneComponentInitializer = new SceneComponentInitializer();
            sceneInitializer = new SceneInitializer();

            if (useEditorialInfo)
                WorldInfoReader = new WorldInfoReader(editorialInfo);

            GameInitializer.GameDataInitialize.SubscribeCompleted(Initialize);
        }

        public IDisposable Subscribe(IObserver<IWorld> observer)
        {
            return worldTracker.Subscribe(observer);
        }

        void Initialize(IAsyncOperation<IGameData> operation)
        {
            GameData = operation.Result;
            WorldInfoReader.SubscribeCompleted(OnWorldInfoReadCompleted);
        }

        void OnWorldInfoReadCompleted(IAsyncOperation<WorldInfo> operation)
        {
            Info = operation.Result;
            worldDataInitialize.Start(GameData, Info, this).SubscribeCompleted(OnWorldDataCompleted);
        }

        void OnWorldDataCompleted(IAsyncOperation<IWorldData> operation)
        {
            Data = operation.Result;
            sceneComponentInitializer.Start(Data, this).SubscribeCompleted(OnSceneComponentCompleted);
        }

        void OnSceneComponentCompleted(IAsyncOperation<IWorldComponent> operation)
        {
            Component = operation.Result;
            sceneInitializer.Start(Data, Component, this).SubscribeCompleted(OnSceneCompleted);
        }

        void OnSceneCompleted(IAsyncOperation<IWorldScene> operation)
        {
            Scene = operation.Result;
            OnInitializeCompleted();
        }

        void OnInitializeCompleted()
        {
            worldTracker.Track(this);
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
