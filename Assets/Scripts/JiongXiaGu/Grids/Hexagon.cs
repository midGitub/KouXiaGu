
//是否允许设置其它属性?
//#define SET_PROPERTY

using System;
using UnityEngine;

namespace JiongXiaGu.Grids
{

    /// <summary>
    /// 平顶六边形相关计算方法;
    /// </summary>
    [Serializable]
    public struct Hexagon
    {

        public Hexagon(double outerRadius)
        {
            this.outerRadius = outerRadius;
        }

        static readonly double cos30 = Math.Sqrt(3) / 2;

        [SerializeField]
        double outerRadius;

        /// <summary>
        /// 外半径
        /// </summary>
        public double OuterRadius
        {
            get { return outerRadius; }
        }

        /// <summary>
        /// 外直径
        /// </summary>
        public double OuterDiameters
        {
            get { return OuterRadius * 2; }
        }

        /// <summary>
        /// 内半径
        /// </summary>
        public double InnerRadius
        {
            get { return cos30 * OuterRadius; }
        }

        /// <summary>
        /// 内直径
        /// </summary>
        public double InnerDiameters
        {
            get { return InnerRadius * 2; }
        }

        /// <summary>
        /// 边长;
        /// </summary>
        public double SideLength
        {
            get { return outerRadius; }
        }

    }

}
