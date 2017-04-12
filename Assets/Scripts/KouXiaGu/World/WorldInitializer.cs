using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
{

    public interface IWorld
    {
        /// <summary>
        /// 世界信息;
        /// </summary>
        WorldInfo Info { get; }

        /// <summary>
        /// 地图信息;
        /// </summary>
        MapManager Map { get; }

        /// <summary>
        /// 资源\产品;
        /// </summary>
        ProductManager Product { get; }

        /// <summary>
        /// 建筑物;
        /// </summary>
        BuildingManager Building { get; }
    }

    /// <summary>
    /// 负责初始化游戏场景;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldInitializer : Initializer
    {

        #region 静态;

        static bool initialized;
        static WorldInfo worldInfo;

        /// <summary>
        /// 提供初始化的世界信息;
        /// </summary>
        public static WorldInfo WorldInfo
        {
            get { return worldInfo; }
            set {
                if (initialized)
                    throw new ArgumentException();
                worldInfo = value;
            }
        }

        static WorldInitializer()
        {
            initialized = false;
        }

        #endregion

        WorldInitializer()
        {
        }

        [SerializeField]
        WorldInfo info;
        public WorldManager World { get; private set; }

        public WorldInfo Info
        {
            get { return info; }
            set { info = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            initialized = true;
        }

        void Start()
        {
            Initialize();
        }

        void OnDestroy()
        {
            initialized = false;
        }

        void Initialize()
        {
            World = new WorldManager(info);
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
