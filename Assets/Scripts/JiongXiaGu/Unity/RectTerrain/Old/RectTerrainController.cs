using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using JiongXiaGu.Unity.Maps;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 矩形地图全局管理器;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectTerrainController : UnitySington<RectTerrainController>
    {
        RectTerrainController()
        {
        }
    }
}
