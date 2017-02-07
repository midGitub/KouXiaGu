using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 记录墙的所有顶点信息;
    /// </summary>
    public class WallVertice
    {


        /// <summary>
        /// 
        /// </summary>
        List<WallSection> wall;

        /// <summary>
        /// 重新计算墙的顶点坐标;
        /// </summary>
        public Vector3[] Recalculate(ISpline spline)
        {
            throw new NotImplementedException();
        }

    }

    /// <summary>
    /// 曲线接口;
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
