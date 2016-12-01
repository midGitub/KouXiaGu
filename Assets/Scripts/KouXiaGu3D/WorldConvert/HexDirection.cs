using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu3D
{

    /// <summary>
    /// 平顶六边形边对应的方向;
    /// </summary>
    [Flags]
    public enum HexDirection
    {
        North = 1,
        Northeast = 2,
        Southeast = 4,
        South = 8,
        Southwest = 16,
        Northwest = 32,
        Self = 64,
    }

}
