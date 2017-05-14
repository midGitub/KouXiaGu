using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Wall
{

    [DisallowMultipleComponent]
    public class DynamicMeshManager : UnitySington<DynamicMeshManager>
    {
        DynamicMeshManager()
        {
        }

        [SerializeField]
        Dictionary<string, DynamicMeshData> dynamicMeshs;

        void Awake()
        {
            SetInstance(this);
        }
    }

}
