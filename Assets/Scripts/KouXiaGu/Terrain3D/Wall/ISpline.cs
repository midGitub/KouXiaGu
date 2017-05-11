using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 曲线;
    /// </summary>
    public interface ISpline
    {
        /// <summary>
        /// 获取到曲线上一点;
        /// </summary>
        /// <param name="f">数值 0~1 </param>
        Vector3 GetPoint(float f);
    }

}
