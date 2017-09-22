using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Grids
{

    /// <summary>
    /// 六边形网格,中心点(0,0,0);
    /// </summary>
    public class CubicHexGrid
    {

        /// <summary>
        /// 六边形起点;
        /// </summary>
        static readonly Vector3 Origin = Vector3.zero;

        readonly float outerRadius;

        readonly Hexagon hexagon;

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
            this.hexagon = new Hexagon(outerRadius);
        }


        /// <summary>
        /// 像素转换到立方体坐标;
        /// </summary>
        public CubicHexCoord GetCubic(Vector3 position)
        {
            Vector2 pt = new Vector2((position.x - Origin.x) / outerRadius, (position.z - Origin.z) / outerRadius);
            float x = (float)(2.0 / 3.0 * pt.x);
            float y = (float)(-1.0 / 3.0 * pt.x + Trigonometric.Tan30 * pt.y);
            return new CubicHexCoord(x, y, (-x - y));
        }

        /// <summary>
        /// 立方体坐标 转换成 像素坐标;
        /// </summary>
        public Vector3 GetPixel(CubicHexCoord hex, float y = 0)
        {
            float x = outerRadius * 1.5f * hex.X;
            float z = (float)(Trigonometric.Cos30 * hex.X + Trigonometric.Tan60 * hex.Y) * outerRadius;
            return new Vector3(x, y, z);
        }


        /// <summary>
        /// 获取到边的中点偏移量,相对于原点;
        /// </summary>
        public Vector3 GetEdgeMidpoint(HexDirections direction)
        {
            Vector3 offset = Vector3.zero;
            switch (direction)
            {
                case HexDirections.North:
                    offset.z = outerRadius;
                    break;

                case HexDirections.Northeast:
                    offset.x = (float)((outerRadius / 2) / Trigonometric.Tan30);
                    offset.z = outerRadius / 2;
                    break;

                case HexDirections.Southeast:
                    offset.x = (float)((outerRadius / 2) / Trigonometric.Tan30);
                    offset.z = -(outerRadius / 2);
                    break;

                case HexDirections.South:
                    offset.z = -outerRadius;
                    break;

                case HexDirections.Southwest:
                    offset.x = -(float)((outerRadius / 2) / Trigonometric.Tan30);
                    offset.z = -(outerRadius / 2);
                    break;

                case HexDirections.Northwest:
                    offset.x = -(float)((outerRadius / 2) / Trigonometric.Tan30);
                    offset.z = (outerRadius / 2);
                    break;

                case HexDirections.Self:
                    break;
            }
            return offset;
        }

        /// <summary>
        /// 获取到六边形点坐标;
        /// </summary>
        public Vector3 GetPoint(HexDiagonals diagonals)
        {
            Vector3 offset = Vector3.zero;
            switch (diagonals)
            {
                case HexDiagonals.Northeast:
                    offset.x = (float)(OuterRadius / 2);
                    offset.z = (float)hexagon.InnerRadius;
                    break;

                case HexDiagonals.East:
                    offset.x = OuterRadius;
                    break;

                case HexDiagonals.Southeast:
                    offset.x = (float)(OuterRadius / 2);
                    offset.z = -(float)hexagon.InnerRadius;
                    break;

                case HexDiagonals.Southwest:
                    offset.x = -(float)(OuterRadius / 2);
                    offset.z = -(float)hexagon.InnerRadius;
                    break;

                case HexDiagonals.West:
                    offset.x = -OuterRadius;
                    break;

                case HexDiagonals.Northwest:
                    offset.x = -(float)(OuterRadius / 2);
                    offset.z = (float)hexagon.InnerRadius;
                    break;

                case HexDiagonals.Self:
                    break;
            }
            return offset;
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
