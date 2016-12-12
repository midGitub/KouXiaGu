using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 六边形网格,中心点(0,0,0);
    /// </summary>
    public struct CubicHexGrid
    {

        /// <summary>
        /// 六边形起点;
        /// </summary>
        static readonly Vector2 Origin = Vector2.zero;
        /// <summary>
        /// 六边形的像素坐标起点;
        /// </summary>
        static readonly Vector3 OriginPixelPoint = Vector3.zero;

        readonly float outerRadius;

        /// <summary>
        /// 六边形半径;
        /// </summary>
        public float OuterRadius
        {
            get { return outerRadius; }
        }

        public CubicHexGrid(float outerRadius)
        {
            this.outerRadius = Math.Abs(outerRadius);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CubicHexGrid))
                return false;
            return (CubicHexGrid)obj == this;
        }

        public override int GetHashCode()
        {
            return outerRadius.GetHashCode();
        }

        public override string ToString()
        {
            return "OuterRadius :" + outerRadius;
        }

        /// <summary>
        /// 像素转换到立方体坐标;
        /// </summary>
        public CubicHexCoord GetCubic(Vector3 position)
        {
            Vector2 pt = new Vector2((position.x - Origin.x) / outerRadius, (position.z - Origin.y) / outerRadius);
            float x = (float)(2.0 / 3.0 * pt.x);
            float y = (float)(-1.0 / 3.0 * pt.x + Math.Sqrt(3.0) / 3.0 * pt.y);
            return new CubicHexCoord(x, y, (-x - y));
        }

        /// <summary>
        /// 立方体坐标 转换成 像素坐标;
        /// </summary>
        public Vector3 GetPixel(CubicHexCoord hex, float y = 0)
        {
            float x = outerRadius * 1.5f * hex.X;
            float z = (float)(Math.Sqrt(3.0) / 2.0 * hex.X + Math.Sqrt(3.0) * hex.Y) * outerRadius;
            return new Vector3(x, y, z);
        }

        public static bool operator ==(CubicHexGrid a, CubicHexGrid b)
        {
            return a.outerRadius == b.outerRadius;
        }

        public static bool operator !=(CubicHexGrid a, CubicHexGrid b)
        {
            return !(a == b);
        }

        public static CubicHexGrid operator *(CubicHexGrid rect, short n)
        {
            return new CubicHexGrid(rect.outerRadius * n);
        }

        public static CubicHexGrid operator /(CubicHexGrid rect, short n)
        {
            return new CubicHexGrid(rect.outerRadius / n);
        }

    }

}
