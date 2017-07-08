using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    public static class RectExtensions
    {
        public static Vector2 Northeast(this Rect rect)
        {
            return new Vector2(rect.xMax, rect.yMax);
        }

        public static Vector2 Southeast(this Rect rect)
        {
            return new Vector2(rect.xMax, rect.yMin);
        }

        public static Vector2 Southwest(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMin);
        }

        public static Vector2 Northwest(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMax);
        }
    }
}
