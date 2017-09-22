using System;
using System.Collections.Generic;
using JiongXiaGu.World;
using UnityEngine;

namespace JiongXiaGu.Terrain3D
{

    /// <summary>
    /// 地形控制;
    /// </summary>
    public class OLandform
    {
        public OLandform()
        {
            LandformChunks = new SceneLandformCollection();
            Buildings = new SceneBuildingCollection();
        }

        internal SceneLandformCollection LandformChunks { get; private set; }
        internal SceneBuildingCollection Buildings { get; private set; }

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
