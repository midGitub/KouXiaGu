using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.DynamicMesh
{

    /// <summary>
    /// 墙体建筑;
    /// </summary>
    [RequireComponent(typeof(DynamicMeshScript))]
    public class WallDynamicMesh : Building, IBuilding
    {
        DynamicMeshScript dynamicMesh;

        void Awake()
        {
            dynamicMesh = GetComponent<DynamicMeshScript>();
        }
    }
}
