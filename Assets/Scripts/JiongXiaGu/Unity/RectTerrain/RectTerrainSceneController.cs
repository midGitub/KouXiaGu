using JiongXiaGu.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 矩形地图场景管理器;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectTerrainSceneController : SceneSington<RectTerrainSceneController>
    {
        RectTerrainSceneController()
        {
        }

        [SerializeField]
        LandformController landform;

        public LandformController Landform
        {
            get { return landform; }
        }
    }
}
