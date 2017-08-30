using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 矩形地图的地形;
    /// </summary>
    [DisallowMultipleComponent]
    public class RectTerrainController : SceneSington<RectTerrainController>
    {
        RectTerrainController()
        {
        }

        [SerializeField]
        RectTerrainParameter parameter;

        public RectTerrainParameter Parameter
        {
            get { return parameter; }
        }

    }
}
