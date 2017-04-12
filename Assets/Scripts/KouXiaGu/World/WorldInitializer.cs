﻿using System;
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
            Map = new MapManager();
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
