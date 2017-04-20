using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.World.Map;
using UniRx;

namespace KouXiaGu.World
{

    /// <summary>
    /// 场景实例;
    /// </summary>
    public interface IWorld
    {
        WorldInfo Info { get; }
        TimeManager Time { get; }
        MapResource Map { get; }
    }


    /// <summary>
    /// 负责初始化游戏场景;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldInitializer : OperationMonoBehaviour, IWorld, IObservable<IWorld>
    {

        public static WorldInitializer Instance { get; private set; }
        public static bool IsInitialized
        {
            get { return Instance != null; }
        }

        public static WorldInfo WorldInfo { get; private set; }


        WorldInitializer()
        {
        }

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

        public TimeManager Time { get; private set; }
        public MapResource Map { get; private set; }

        ListTracker<IWorld> worldTracker;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
            worldTracker = new ListTracker<IWorld>();
            GameInitializer.Instance.SubscribeCompleted(_ => InitializeAsync());
        }

        void OnDestroy()
        {
            Instance = null;
        }

        public IDisposable Subscribe(IObserver<IWorld> observer)
        {
            return worldTracker.Subscribe(observer);
        }

        /// <summary>
        /// 初始化游戏世界场景,手动调用;
        /// </summary>
        internal void InitializeAsync()
        {
            Debug.Log("开始场景初始化;");

            IAsyncOperation[] missions = new IAsyncOperation[]
              {
                  MapResource.ReadOrCreateAsync().Subscribe(OnMapResourceCompleted, OnFaulted),
                  TimeManager.Create(Info.Time, this).Subscribe(OnTimeCompleted, OnFaulted),
              };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(OnCompleted, OnFaulted);
        }

        void OnCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted();
            worldTracker.Track(this);
            Debug.Log("场景初始化完毕;");
        }

        void OnFaulted(IList<IAsyncOperation> operations)
        {
            AggregateException ex = operations.ToAggregateException();
            OnFaulted(ex);
            Debug.LogWarning("场景初始化失败;");
        }

        void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError("场景初始化时遇到异常:\n" + operation.Exception);
        }


        void OnMapResourceCompleted(IAsyncOperation<MapResource> operation)
        {
            const string prefix = "[地图资源]";

            Map = operation.Result;
            Debug.Log(prefix + "初始化完毕;总共有 " + Map.Data.Count + " 个节点;");
        }

        void OnTimeCompleted(IAsyncOperation<TimeManager> operation)
        {
            const string prefix = "[时间]";

            Time = operation.Result;
            Debug.Log(prefix + "初始化完毕;");
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
