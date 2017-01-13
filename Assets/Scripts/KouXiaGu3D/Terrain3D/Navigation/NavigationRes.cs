using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Initialization;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 地形导航信息;
    /// </summary>
    public sealed class NavigationRes : GlobalSington<NavigationRes>
    {
        NavigationRes() { }

        /// <summary>
        /// 对应的地形信息;
        /// </summary>
        static readonly Dictionary<int, NavigationDescr> terrainInfos = new Dictionary<int, NavigationDescr>();

        /// <summary>
        /// 对应的地形信息;
        /// </summary>
        public static IDictionary<int, NavigationDescr> TerrainInfos
        {
            get { return terrainInfos; }
        }

        static readonly NavigationDescr defaultNavigationDescr = new NavigationDescr()
        {
            Landform = 0,
            Walkable = false,
            SpeedOfTravel = 1,
            NavigationCost = 10,
        };

        /// <summary>
        /// 获取到寻路信息,若不存在则返回默认的寻路信息;
        /// </summary>
        public static NavigationDescr GetNavigationDescr(int landform)
        {
            NavigationDescr descr;
            if (terrainInfos.TryGetValue(landform, out descr))
            {
                return descr;
            }

            Debug.LogWarning("获取不存在的导航信息;地貌:" + landform);
            return defaultNavigationDescr;
        }

        public static string NavigationDescrFile
        {
            get { return TerrainFiler.Combine(GetInstance.navigationDescrName); }
        }

        /// <summary>
        /// 从文件读取到地形导航信息;
        /// </summary>
        public static void Load()
        {
            string filePath = NavigationDescrFile;
            var descr = (NavigationDescr[])NavigationDescr.ArraySerializer.DeserializeXiaGu(filePath);
            terrainInfos.AddOrUpdate(descr, item => item.Landform);
        }

        /// <summary>
        /// 保存到文件;
        /// </summary>
        public static void Save(NavigationDescr[] descr)
        {
            string filePath = NavigationDescrFile;
            NavigationDescr.ArraySerializer.SerializeXiaGu(filePath, descr);
        }

        /// <summary>
        /// 清除所有地形导航信息;
        /// </summary>
        public static void Clear()
        {
            terrainInfos.Clear();
        }

        /// <summary>
        /// 导航描述文件名;
        /// </summary>
        [SerializeField]
        string navigationDescrName = "NavigationDescr.xml";


        protected override void Awake()
        {
            base.Awake();
        }

    }
}
