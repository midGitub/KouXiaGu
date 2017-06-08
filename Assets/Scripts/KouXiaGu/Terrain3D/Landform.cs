using System;
using System.Collections.Generic;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形控制;
    /// </summary>
    public class Landform
    {
        public Landform()
        {
            LandformChunks = new SceneLandformCollection();
            Buildings = new SceneBuildingCollection();
        }

        internal SceneLandformCollection LandformChunks { get; private set; }
        internal SceneBuildingCollection Buildings { get; private set; }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            return LandformChunks.GetHeight(position);
        }
    }
}
