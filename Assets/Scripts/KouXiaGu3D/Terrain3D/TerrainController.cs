using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 用于初始化地形信息;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainController : UnitySingleton<TerrainController>
    {
        TerrainController() { }

        /// <summary>
        /// 地形地图的目录;
        /// </summary>
        [SerializeField]
        string mapDirectory;

        /// <summary>
        /// 初始化地形数据;
        /// </summary>
        void Initialize()
        {
            InitMap();
        }


        void InitMap()
        {
            TerrainMap.Load(mapDirectory);
        }



    }

}
