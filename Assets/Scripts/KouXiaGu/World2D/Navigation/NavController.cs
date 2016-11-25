using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 提供寻路路径;
    /// </summary>
    [DisallowMultipleComponent]
    public class NavController : UnitySingleton<NavController>
    {

        /// <summary>
        /// 返回到达这儿的路径;
        /// </summary>
        public NavPath FreeToGo(ShortVector2 mapPoint)
        {
            throw new NotImplementedException();
        }

    }

}
