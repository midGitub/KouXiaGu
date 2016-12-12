using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 网格坐标转换;
    /// </summary>
    public static class GridConvert
    {

        /// <summary>
        /// 六边形半径定义(单位像素);
        /// </summary>
        public const int OuterRadius = 1;

        /// <summary>
        /// 提供的六边形运算;
        /// </summary>
        public static readonly Hexagon hexagon = new Hexagon(OuterRadius);

        /// <summary>
        /// 六边形起点;
        /// </summary>
        public static readonly RectCoord Origin = new RectCoord(0, 0);
        /// <summary>
        /// 六边形的像素坐标起点;
        /// </summary>
        public static readonly Vector3 OriginPixelPoint = Vector3.zero;

        static readonly CubicHexGrid grid = new CubicHexGrid(OuterRadius);

        /// <summary>
        /// 在场景使用的网格;
        /// </summary>
        public static CubicHexGrid Grid
        {
            get { return grid; }
        }

    }

}