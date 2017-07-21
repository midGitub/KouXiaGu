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

        ///// <summary>
        ///// 获取到高度,若不存在高度信息,则返回0;
        ///// </summary>
        //public float GetHeight(Vector3 position)
        //{
        //    return LandformChunks.GetHeight(position);
        //}

        /// <summary>
        /// 使坐标贴与地面;
        /// </summary>
        public Vector3 Normalize(Vector3 position)
        {
            position.y = 10;
            RaycastHit raycastHit;
            if (LandformRay.Instance.Raycast(position, Vector3.down, out raycastHit))
            {
                return raycastHit.point;
            }
            else
            {
                return position;
            }
        }
    }
}
