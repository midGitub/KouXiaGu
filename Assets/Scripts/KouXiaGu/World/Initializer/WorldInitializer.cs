using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.World.Map;
using UniRx;
using KouXiaGu.Terrain3D;

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
        public static WorldInfo staticWorldInfo { get; set; }


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
        public IWorldData Data { get; private set; }
        public IWorldScene Scene { get; private set; }

        public WorldInfo WorldInfo
        {
            get { return useEditorialInfo ? editorialInfo : staticWorldInfo; }
        }

        void Awake()
        {
            worldTracker = new ListTracker<IWorld>();
            worldDataInitialize = new WorldDataInitializer();
            sceneInitialize = new SceneInitializer();

            GameInitializer.GameDataInitialize.SubscribeCompleted(OnGameDataCompleted);
        }

        public IDisposable Subscribe(IObserver<IWorld> observer)
        {
            return worldTracker.Subscribe(observer);
        }

        void OnGameDataCompleted(IAsyncOperation<IGameData> operation)
        {
            IGameData gameDate = operation.Result;
            worldDataInitialize.Start(gameDate, WorldInfo, this).SubscribeCompleted(OnWorldDataCompleted);
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
