using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 矩形拓展;
    /// </summary>
    public static class RectExtensions
    {

        public static Vector3 Northeast(this Rect rect)
        {
            return new Vector3(rect.xMax, 0, rect.yMax);
        }

        public static Vector3 Southeast(this Rect rect)
        {
            return new Vector3(rect.xMax, 0, rect.yMin);
        }

        public static Vector3 Southwest(this Rect rect)
        {
            return new Vector3(rect.xMin, 0, rect.yMin);
        }

        public static Vector3 Northwest(this Rect rect)
        {
            return new Vector3(rect.xMin, 0, rect.yMax);
        }

    }

}
