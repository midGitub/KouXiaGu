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
    public static partial class GridConvert
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
        public static readonly ShortVector2 Origin = new ShortVector2(0, 0);
        /// <summary>
        /// 六边形的像素坐标起点;
        /// </summary>
        public static readonly Vector3 OriginPixelPoint = Vector3.zero;





        /// <summary>
        /// 从像素坐标获取到所在的 立方体坐标;
        /// </summary>
        public static CubicHexCoord ToHexCubic(this Vector2 point)
        {
            Vector2 pt = new Vector2((point.x - Origin.x) / OuterRadius, (point.y - Origin.y) / OuterRadius);
            float x = (float)(2.0 / 3.0 * pt.x);
            float y = (float)(-1.0 / 3.0 * pt.x + Math.Sqrt(3.0) / 3.0 * pt.y);
            return new CubicHexCoord(x, y, (-x - y));
        }

        /// <summary>
        /// 像素转换到立方体坐标;
        /// </summary>
        public static CubicHexCoord ToHexCubic(this Vector3 point)
        {
            Vector2 v2 = new Vector2(point.x, point.z);
            return ToHexCubic(v2);
        }

        /// <summary>
        /// 立方体坐标 转换成 2D像素坐标;
        /// </summary>
        public static Vector2 ToPixel2D(this CubicHexCoord hex)
        {
            float x = OuterRadius * 1.5f * hex.X;
            float y = (float)(Math.Sqrt(3.0) / 2.0 * hex.X + Math.Sqrt(3.0) * hex.Y) * OuterRadius;
            return new Vector2(x, y);
        }

        /// <summary>
        /// 立方体坐标 转换成 3D像素坐标;
        /// </summary>
        public static Vector3 ToPixel(this CubicHexCoord hex, float y = 0)
        {
            Vector2 v2 = ToPixel2D(hex);
            return new Vector3(v2.x, y, v2.y);
        }

    }

}