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

        [SerializeField]
        bool useEditorialInfo = false;
        [SerializeField]
        WorldInfo editorialInfo;
        internal ListTracker<IWorld> Tracker { get; private set; }
        public WorldManager World { get; private set; }

        public WorldInfo Info
        {
            get { return useEditorialInfo ? editorialInfo : staticWorldInfo; }
        }

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
            if(Tracker == null)
                Tracker = new ListTracker<IWorld>();

            return Tracker.Subscribe(observer);
        }

        /// <summary>
        /// 同步的初始化,手动调用;
        /// </summary>
        public void StartInit()
        {
            try
            {
                Initialize();
                Tracker.Track(this);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                Tracker.TrackError(ex);
            }
            finally
            {
                Tracker.TrackCompleted();
            }
        }

        void Initialize()
        {
            World = new WorldManager(Info);
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
