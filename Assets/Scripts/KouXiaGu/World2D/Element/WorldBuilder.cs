using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 监视地图变化,和初始化地图;
    /// </summary>
    public class WorldBuilder : UnitySingleton<WorldBuilder>
    {

        /// <summary>
        /// 已这个物体为中心更新;
        /// </summary>
        [SerializeField]
        Transform target;

    }

}
