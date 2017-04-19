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

        public static WorldInitializer Instance { get; private set; }
        public static bool IsInitialized
        {
            get { return Instance != null; }
        }


        WorldInitializer()
        {
        }

        [SerializeField]
        bool useEditorialInfo = false;
        [SerializeField]
        WorldInfo editorialInfo;
        ListTracker<IWorld> worldTracker;

        public bool UseEditorialInfo
        {
            get { return useEditorialInfo; }
            set { useEditorialInfo = value; }
        }

        public WorldInfo Info
        {
            get { return editorialInfo; }
            set { editorialInfo = value; }
        }

        void Awake()
        {
            Instance = this;
            worldTracker = new ListTracker<IWorld>();
        }

        void OnDestroy()
        {
            Instance = null;
        }

        /// <summary>
        /// 初始化游戏世界场景;
        /// </summary>
        internal void Initialize(WorldInfo worldInfo)
        {
            if(!UseEditorialInfo)
                editorialInfo = worldInfo;
        }

        public IDisposable Subscribe(IObserver<IWorld> observer)
        {
            return worldTracker.Subscribe(observer);
        }



        /// <summary>
        /// 世界信息;
        /// </summary>
        public WorldManager World { get; private set; }

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
