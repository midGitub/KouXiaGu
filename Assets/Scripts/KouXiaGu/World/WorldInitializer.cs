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
        /// 时间;
        /// </summary>
        TimeManager Time { get; }

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
    public class WorldInitializer : Initializer, IWorld
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


        /// <summary>
        /// 世界信息;
        /// </summary>
        public WorldInfo Info
        {
            get { return WorldInfo; }
        }

        /// <summary>
        /// 时间;
        /// </summary>
        public TimeManager Time { get; private set; }

        /// <summary>
        /// 地图信息;
        /// </summary>
        public MapManager Map { get; private set; }

        /// <summary>
        /// 资源\产品;
        /// </summary>
        public ProductManager Product { get; private set; }

        /// <summary>
        /// 建筑物;
        /// </summary>
        public BuildingManager Building { get; private set; }


        WorldInitializer()
        {
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
            Time = SceneObject.GetObject<TimeManager>();
            Time.Initialize(Info.Time);

            Map = new MapManager();
        }

    }


}
