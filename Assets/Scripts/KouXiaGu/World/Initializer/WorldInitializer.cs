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
        const string Name = "WorldInitializer";
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
        SceneInitializer sceneInitializer;
        public IGameData GameData { get; private set; }
        public WorldInfo Info { get; private set; }
        public IWorldData Data { get; private set; }
        public IWorldScene Component { get; private set; }

        public event Action<IWorld> OnInitializeCompleted
        {
            add { onInitializeCompleted += value; }
            remove { onInitializeCompleted -= value; }
        }

        void Awake()
        {
            worldTracker = new LinkedListTracker<IWorld>();
            worldDataInitialize = new DataInitializer();
            sceneInitializer = new SceneInitializer();
            GameInitializer.Instance.GameDataInitialize.SubscribeCompleted(Name + "等待游戏数据初始化完毕;", Initialize);
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
