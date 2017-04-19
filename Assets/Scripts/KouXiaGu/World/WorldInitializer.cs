using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Rx;
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
        WorldManager World { get; }
        MapResource Map { get; }
    }


    /// <summary>
    /// 负责初始化游戏场景;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldInitializer : OperationMonoBehaviour, IWorld, IXiaGuObservable<IWorld>
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

        public WorldManager World { get; private set; }
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

        public IDisposable Subscribe(IXiaGuObserver<IWorld> observer)
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
                  MapResource.ReadAsync().Subscribe(OnMapResourceCompleted, OnFaulted),
              };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(OnCompleted, OnFaulted);
        }

        void OnCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted();
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
            Debug.LogError("场景初始化时遇到错误:\n" + operation.Exception);
        }


        void OnMapResourceCompleted(IAsyncOperation<MapResource> operation)
        {
            Map = operation.Result;
            Debug.Log("地图读取完毕;");
        }


        /// <summary>
        /// 同步的初始化,手动调用;
        /// </summary>
        public void StartInit(GameData data)
        {
            try
            {
                Initialize(data);
                worldTracker.Track(this);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                worldTracker.TrackError(ex);
            }
            finally
            {
                worldTracker.TrackCompleted();
            }
        }

        void Initialize(GameData data)
        {
            World = new WorldManager(Info, data.ElementInfo);
            Subscribe(World);
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
