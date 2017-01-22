using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    public static class VectorExtensions
    {

        /// <summary>
        /// 将其值限制在0~1之间;
        /// </summary>
        public static Vector2 Clamp01(this Vector2 o)
        {
            o.x = Mathf.Clamp01(o.x);
            o.y = Mathf.Clamp01(o.y);
            return o;
        }


    }

}
