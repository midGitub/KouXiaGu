using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 地形导航信息;
    /// </summary>
    public sealed class NavigationRes : UnitySington<NavigationRes>
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

        public static string NavigationDescrFile
        {
            get { return TerrainResPath.Combine(GetInstance.navigationDescrName); }
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


    }

}
