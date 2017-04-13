using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Rx;

namespace KouXiaGu.World
{

    /// <summary>
    /// 场景实例;
    /// </summary>
    public interface IWorld
    {
        WorldInfo Info { get; }
        WorldManager World { get; }
        WorldElementManager ElementInfo { get; }
    }

    /// <summary>
    /// 负责初始化游戏场景;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldInitializer : MonoBehaviour, IWorld, IObservable<IWorld>
    {

        static bool initialized = false;
        static WorldInfo staticWorldInfo;

        /// <summary>
        /// 提供初始化的世界信息;
        /// </summary>
        public static WorldInfo WorldInfo
        {
            get { return staticWorldInfo; }
            set {
                if (initialized)
                    throw new ArgumentException();
                staticWorldInfo = value;
            }
        }


        WorldInitializer()
        {
        }

#if UNITY_EDITOR

        [SerializeField]
        bool useEditorialInfo = false;
        [SerializeField]
        WorldInfo editorialInfo;

        public bool UseEditorialInfo
        {
            get { return useEditorialInfo; }
            set { useEditorialInfo = value; }
        }
#endif

        public WorldInfo Info
        {
#if UNITY_EDITOR
            get { return useEditorialInfo ? editorialInfo : staticWorldInfo; }
            set { editorialInfo = value; }
#else
             get { return staticWorldInfo; }
#endif
        }


        ListTracker<IWorld> tracker;

        /// <summary>
        /// 世界信息;
        /// </summary>
        public WorldManager World { get; private set; }

        /// <summary>
        /// 基础信息;
        /// </summary>
        public WorldElementManager ElementInfo { get; private set; }

        void Awake()
        {
            initialized = true;
        }

        void Start()
        {
            StartInit();
        }

        void OnDestroy()
        {
            initialized = false;
        }

        public IDisposable Subscribe(IObserver<IWorld> observer)
        {
            if(tracker == null)
                tracker = new ListTracker<IWorld>();

            return tracker.Subscribe(observer);
        }

        /// <summary>
        /// 同步的初始化,手动调用;
        /// </summary>
        public void StartInit()
        {
            try
            {
                Initialize();
                tracker.Track(this);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                tracker.TrackError(ex);
            }
            finally
            {
                tracker.TrackCompleted();
            }
        }

        void Initialize()
        {
            ElementInfo = WorldElementManager.Read();

            World = new WorldManager(Info, ElementInfo);
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
            WorldElementManager item = WorldElementManager.Read();

            RoadInfo info;
            if (item.RoadInfos.TryGetValue(2, out info))
            {
                Debug.Log((info.Terrain == null).ToString());
            }
        }

    }


}
