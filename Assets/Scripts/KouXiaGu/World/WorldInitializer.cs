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
    /// 世界数据;
    /// </summary>
    public interface IWorldData
    {
        IGameData GameData { get; }
        WorldInfo Info { get; }
        TimeManager Time { get; }
        MapResource Map { get; }
    }

    /// <summary>
    /// 世界场景;
    /// </summary>
    public interface IWorldScene : IWorldData
    {
        Landform Landform { get; }
    }


    /// <summary>
    /// 负责初始化游戏场景;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldInitializer : OperationMonoBehaviour, IWorldScene, IObservable<IWorldScene>
    {

        const string InitializationCompletedStr = "初始化完毕;";
        public static WorldInitializer Instance { get; private set; }
        public static bool IsInitialized
        {
            get { return Instance != null; }
        }

        public static WorldInfo WorldInfo { get; private set; }


        WorldInitializer()
        {
        }

        ListTracker<IWorldScene> worldTracker;

        [SerializeField]
        bool useEditorialInfo = false;

        public bool UseEditorialInfo
        {
            get { return useEditorialInfo; }
            set { useEditorialInfo = value; }
        }

        [SerializeField]
        WorldInfo editorialInfo;

        public WorldInfo Info
        {
            get { return useEditorialInfo ? editorialInfo : WorldInfo; }
        }

        IGameData IWorldData.GameData
        {
            get { return GameInitializer.Instance.Data; }
        }

        public TimeManager Time { get; private set; }
        public MapResource Map { get; private set; }

        public Landform Landform { get; private set; }


        protected override void Awake()
        {
            Instance = this;
            base.Awake();
            worldTracker = new ListTracker<IWorldScene>();
            GameInitializer.Instance.SubscribeCompleted(_ => BuildingData());
        }

        void OnDestroy()
        {
            Instance = null;
        }

        public IDisposable Subscribe(IObserver<IWorldScene> observer)
        {
            return worldTracker.Subscribe(observer);
        }


        void OnFaulted(IList<IAsyncOperation> operations)
        {
            AggregateException ex = operations.ToAggregateException();
            OnFaulted(ex);
            Debug.LogWarning("世界初始化失败;");
        }

        void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError("场景初始化时遇到异常:\n" + operation.Exception);
        }


        /// <summary>
        /// 初始化游戏世界数据;
        /// </summary>
        void BuildingData()
        {
            Debug.Log("------开始初始化游戏世界数据;");

            IAsyncOperation[] missions = new IAsyncOperation[]
              {
                  MapResource.ReadOrCreateAsync().Subscribe(OnMapResourceCompleted, OnFaulted),
                  TimeManager.Create(Info.Time, this).Subscribe(OnTimeCompleted, OnFaulted),
              };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(OnBuildingDataCompleted, OnFaulted);
        }

        void OnMapResourceCompleted(IAsyncOperation<MapResource> operation)
        {
            const string prefix = "[地图]";
            Map = operation.Result;
            Debug.Log(prefix + InitializationCompletedStr + " 总共有 " + Map.Data.Count + " 个节点;");
        }

        void OnTimeCompleted(IAsyncOperation<TimeManager> operation)
        {
            const string prefix = "[时间]";
            Time = operation.Result;
            Debug.Log(prefix + InitializationCompletedStr);
        }

        void OnBuildingDataCompleted(IList<IAsyncOperation> operations)
        {
            Debug.Log("------游戏世界数据初始化完毕;");
            BuildingScene(this);
        }



        /// <summary>
        /// 初始化游戏场景;
        /// </summary>
        void BuildingScene(IWorldScene world)
        {
            Debug.Log("------开始初始化游戏场景;");

            IAsyncOperation[] missions = new IAsyncOperation[]
              {
                  Landform.Initialize(world).Subscribe(OnLandformCompleted, OnFaulted),
              };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(OnBuildingSceneCompleted, OnFaulted);
        }

        void OnLandformCompleted(IAsyncOperation<Landform> operation)
        {
            const string prefix = "[地形]";
            Landform = operation.Result;
            Debug.Log(prefix + InitializationCompletedStr);
        }

        void OnBuildingSceneCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted();
            Debug.Log("------游戏场景初始化完毕;");

            worldTracker.Track(this);
            Debug.Log("------开始游戏状态;");
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
