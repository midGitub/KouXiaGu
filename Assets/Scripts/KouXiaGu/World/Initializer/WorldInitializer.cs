using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.World
{

    /// <summary>
    /// 世界场景;
    /// </summary>
    public interface IWorld
    {
        IWorldData Data { get; }
        IWorldScene Scene { get; }
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
        SceneInitializer sceneInitialize;
        public IGameData GameData { get; private set; }
        public WorldInfo Info { get; private set; }
        public IWorldData Data { get; private set; }
        public IWorldScene Scene { get; private set; }

        void Awake()
        {
            worldTracker = new ListTracker<IWorld>();
            worldDataInitialize = new WorldDataInitializer();
            sceneInitialize = new SceneInitializer();

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
            sceneInitialize.Start(Data, this).SubscribeCompleted(OnSceneCompleted);
        }

        void OnSceneCompleted(IAsyncOperation<IWorldScene> operation)
        {
            Scene = operation.Result;
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
