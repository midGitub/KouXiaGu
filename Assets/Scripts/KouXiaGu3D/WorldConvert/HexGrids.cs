// Generated code -- http://www.redblobgames.com/grids/hexagons/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏六边形网格拓展;
    /// 六边形为平顶的偶数垂直布局;
    /// </summary>
    public static partial class HexGrids
    {

        /// <summary>
        /// 六边形半径定义(单位像素);
        /// </summary>
        public const int OuterRadius = 1;

        /// <summary>
        /// 六边形起点;
        /// </summary>
        static readonly ShortVector2 origin = new ShortVector2(0, 0);

        #region 坐标转换;

        /// <summary>
        /// 立方体坐标 转换为 偏移坐标;
        /// </summary>
        public static ShortVector2 CubeToOffset(CubeCoordinate cube)
        {
            int x = cube.q;
            int y = cube.r + (int)((cube.q + 1 * (cube.q & 1)) / 2);
            return new ShortVector2(x, y);
        }

        /// <summary>
        /// 偏移坐标 转换成 立方体坐标;
        /// </summary>
        public static CubeCoordinate OffsetToCube(ShortVector2 offset)
        {
            int q = offset.x;
            int r = offset.y - (int)((offset.x + 1 * (offset.x & 1)) / 2);
            int s = -q - r;
            return new CubeCoordinate(q, r, s);
        }

        /// <summary>
        /// 从像素坐标获取到所在的 偏移坐标;
        /// </summary>
        public static ShortVector2 Pixel2DToOffset(Vector2 point)
        {
            CubeCoordinate cube = Pixel2DToCube(point);
            return CubeToOffset(cube);
        }

        /// <summary>
        /// 3D像素坐标 转换到 偏移坐标;
        /// </summary>
        public static ShortVector2 PixelToOffset(Vector3 point)
        {
            CubeCoordinate cube = Pixel2DToCube(new Vector2(point.x, point.z));
            return CubeToOffset(cube);
        }

        /// <summary>
        /// 从像素坐标获取到所在的 立方体坐标;
        /// </summary>
        public static CubeCoordinate Pixel2DToCube(Vector2 point)
        {
            Vector2 pt = new Vector2((point.x - origin.x) / OuterRadius, (point.y - origin.y) / OuterRadius);
            float q = (float)(2.0 / 3.0 * pt.x);
            float r = (float)(-1.0 / 3.0 * pt.x + Math.Sqrt(3.0) / 3.0 * pt.y);
            return new CubeCoordinate(q, r, (-q - r));
        }


        /// <summary>
        /// 立方体坐标 转换成 像素坐标;
        /// </summary>
        public static Vector2 CubeToPixel2D(CubeCoordinate hex)
        {
            float x = OuterRadius * 1.5f * hex.q;
            float y = (float)(OuterRadius * Math.Sqrt(3) * (hex.r + hex.q / 2));
            return new Vector2(x, y);
        }

        /// <summary>
        /// 偏移坐标 转换成 像素坐标;
        /// </summary>
        public static Vector2 OffsetToPixel2D(ShortVector2 offset)
        {
            float x = (float)(OuterRadius * 1.5f * offset.x);
            float y = (float)(OuterRadius * Math.Sqrt(3) * (offset.y - 0.5 * (offset.x & 1)));
            return new Vector2(x, y);
        }

        /// <summary>
        /// 偏移坐标 转换成 3D的像素坐标;
        /// </summary>
        public static Vector3 OffsetToPixel(ShortVector2 offset)
        {
            float x = (float)(OuterRadius * 1.5f * offset.x);
            float z = (float)(OuterRadius * Math.Sqrt(3) * (offset.y - 0.5 * (offset.x & 1)));
            return new Vector3(x, 0, z);
        }

        #endregion

        #region 距离

        /// <summary>
        /// 曼哈顿距离;
        /// </summary>
        public static int ManhattanDistances(CubeCoordinate a, CubeCoordinate b)
        {
            CubeCoordinate hex = a - b;
            return (int)((Math.Abs(hex.q) + Math.Abs(hex.r) + Math.Abs(hex.s)) / 2);
        }

        /// <summary>
        /// 曼哈顿距离;
        /// </summary>
        public static int ManhattanDistances(ShortVector2 offset1, ShortVector2 offset2)
        {
            CubeCoordinate hex1 = OffsetToCube(offset1);
            CubeCoordinate hex2 = OffsetToCube(offset2);
            return ManhattanDistances(hex1, hex2);
        }

        #endregion

        #region 方向;

        /// <summary>
        /// 存在方向数;
        /// </summary>
        public const int DirectionNumber = 7;

        /// <summary>
        /// 方向偏移量;
        /// </summary>
        static Dictionary<int, CubeCoordinate> directions = GetDirections();

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        static Dictionary<int, CubeCoordinate> GetDirections()
        {
            Dictionary<int, CubeCoordinate> directions = new Dictionary<int, CubeCoordinate>(DirectionNumber);

            directions.Add((int)HexDirection.North, new CubeCoordinate(0, 1, -1));
            directions.Add((int)HexDirection.Northeast, new CubeCoordinate(1, 0, -1));
            directions.Add((int)HexDirection.Southeast, new CubeCoordinate(1, -1, 0));
            directions.Add((int)HexDirection.South, new CubeCoordinate(0, -1, 1));
            directions.Add((int)HexDirection.Southwest, new CubeCoordinate(-1, 0, 1));
            directions.Add((int)HexDirection.Northwest, new CubeCoordinate(-1, 1, 0));
            directions.Add((int)HexDirection.Self, new CubeCoordinate(0, 0, 0));

            return directions;
        }

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        public static CubeCoordinate CubeDirectionVector(HexDirection direction)
        {
            return directions[(int)direction];
        }

        /// <summary>
        /// 获取到邻居;
        /// </summary>
        public static CubeCoordinate CubeNeighbor(CubeCoordinate cube, HexDirection direction)
        {
            CubeCoordinate cubeVector = CubeDirectionVector(direction);
            return cube + cubeVector;
        }

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        public static ShortVector2 OffSetDirectionVector(HexDirection direction)
        {
            CubeCoordinate hex = directions[(int)direction];
            return CubeToOffset(hex);
        }

        /// <summary>
        /// 获取到邻居;
        /// </summary>
        public static ShortVector2 OffSetNeighbor(ShortVector2 offset, HexDirection direction)
        {
            ShortVector2 offsetVector = OffSetDirectionVector(direction);
            return offset + offsetVector;
        }

        #endregion

    }

    /// <summary>
    /// 六边形立方体坐标;
    /// </summary>
    public struct CubeCoordinate
    {

        public short q;
        public short r;
        public short s;

        public CubeCoordinate(short q, short r, short s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public CubeCoordinate(int q, int r, int s)
        {
            this.q = (short)q;
            this.r = (short)r;
            this.s = (short)s;
        }

        public CubeCoordinate(float q, float r, float s)
        {
            int intQ = (short)(Math.Round(q));
            int intR = (short)(Math.Round(r));
            int intS = (short)(Math.Round(s));
            double q_diff = Math.Abs(intQ - q);
            double r_diff = Math.Abs(intR - r);
            double s_diff = Math.Abs(intS - s);
            if (q_diff > r_diff && q_diff > s_diff)
            {
                intQ = -intR - intS;
            }
            else
            {
                if (r_diff > s_diff)
                {
                    intR = -intQ - intS;
                }
                else
                {
                    intS = -intQ - intR;
                }
            }
            this.q = (short)intQ;
            this.r = (short)intR;
            this.s = (short)intS;
        }

        public static CubeCoordinate operator +(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.q + b.q, a.r + b.r, a.s + b.s);
        }

        public static CubeCoordinate operator -(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.q - b.q, a.r - b.r, a.s - b.s);
        }

        public static CubeCoordinate operator *(CubeCoordinate a, int k)
        {
            return new CubeCoordinate(a.q * k, a.r * k, a.s * k);
        }

        public static CubeCoordinate operator /(CubeCoordinate a, int k)
        {
            return new CubeCoordinate(a.q / k, a.r / k, a.s / k);
        }

    }


    //struct FractionalHex
    //{
    //    public FractionalHex(double q, double r, double s)
    //    {
    //        this.q = q;
    //        this.r = r;
    //        this.s = s;
    //    }
    //    public readonly double q;
    //    public readonly double r;
    //    public readonly double s;

    //    public Hex HexRound()
    //    {
    //        int q = (int)(Math.Round(this.q));
    //        int r = (int)(Math.Round(this.r));
    //        int s = (int)(Math.Round(this.s));
    //        double q_diff = Math.Abs(q - this.q);
    //        double r_diff = Math.Abs(r - this.r);
    //        double s_diff = Math.Abs(s - this.s);
    //        if (q_diff > r_diff && q_diff > s_diff)
    //        {
    //            q = -r - s;
    //        }
    //        else
    //            if (r_diff > s_diff)
    //        {
    //            r = -q - s;
    //        }
    //        else
    //        {
    //            s = -q - r;
    //        }
    //        return new Hex(q, r, s);
    //    }


    //    static public FractionalHex HexLerp(FractionalHex a, FractionalHex b, double t)
    //    {
    //        return new FractionalHex(a.q * (1 - t) + b.q * t, a.r * (1 - t) + b.r * t, a.s * (1 - t) + b.s * t);
    //    }


    //    //static public List<Hex> HexLinedraw(Hex a, Hex b)
    //    //{
    //    //    int N = Hex.ManhattanDistances(a, b);
    //    //    FractionalHex a_nudge = new FractionalHex(a.q + 0.000001, a.r + 0.000001, a.s - 0.000002);
    //    //    FractionalHex b_nudge = new FractionalHex(b.q + 0.000001, b.r + 0.000001, b.s - 0.000002);
    //    //    List<Hex> results = new List<Hex> { };
    //    //    double step = 1.0 / Math.Max(N, 1);
    //    //    for (int i = 0; i <= N; i++)
    //    //    {
    //    //        results.Add(FractionalHex.HexLerp(a_nudge, b_nudge, step * i).HexRound());
    //    //    }
    //    //    return results;
    //    //}

    //}

    //struct OffsetCoord
    //{
    //    public OffsetCoord(int col, int row)
    //    {
    //        this.col = col;
    //        this.row = row;
    //    }
    //    public readonly int col;
    //    public readonly int row;
    //    static public int EVEN = 1;
    //    static public int ODD = -1;

    //    static public OffsetCoord QoffsetFromCube(int offset, Hex h)
    //    {
    //        int col = h.q;
    //        int row = h.r + (int)((h.q + offset * (h.q & 1)) / 2);
    //        return new OffsetCoord(col, row);
    //    }


    //    static public Hex QoffsetToCube(int offset, OffsetCoord h)
    //    {
    //        int q = h.col;
    //        int r = h.row - (int)((h.col + offset * (h.col & 1)) / 2);
    //        int s = -q - r;
    //        return new Hex(q, r, s);
    //    }


    //    static public OffsetCoord RoffsetFromCube(int offset, Hex h)
    //    {
    //        int col = h.q + (int)((h.r + offset * (h.r & 1)) / 2);
    //        int row = h.r;
    //        return new OffsetCoord(col, row);
    //    }


    //    static public Hex RoffsetToCube(int offset, OffsetCoord h)
    //    {
    //        int q = h.col - (int)((h.row + offset * (h.row & 1)) / 2);
    //        int r = h.row;
    //        int s = -q - r;
    //        return new Hex(q, r, s);
    //    }

    //}

    //struct Orientation
    //{
    //    public Orientation(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double start_angle)
    //    {
    //        this.f0 = f0;
    //        this.f1 = f1;
    //        this.f2 = f2;
    //        this.f3 = f3;
    //        this.b0 = b0;
    //        this.b1 = b1;
    //        this.b2 = b2;
    //        this.b3 = b3;
    //        this.start_angle = start_angle;
    //    }
    //    public readonly double f0;
    //    public readonly double f1;
    //    public readonly double f2;
    //    public readonly double f3;
    //    public readonly double b0;
    //    public readonly double b1;
    //    public readonly double b2;
    //    public readonly double b3;
    //    public readonly double start_angle;
    //}

    //struct Layout
    //{
    //    public Layout(Orientation orientation, Vector2 size, Vector2 origin)
    //    {
    //        this.orientation = orientation;
    //        this.size = size;
    //        this.origin = origin;
    //    }
    //    public readonly Orientation orientation;
    //    public readonly Vector2 size;
    //    public readonly Vector2 origin;
    //    //static public Orientation pointy = new Orientation(Math.Sqrt(3.0), Math.Sqrt(3.0) / 2.0, 0.0, 3.0 / 2.0, Math.Sqrt(3.0) / 3.0, -1.0 / 3.0, 0.0, 2.0 / 3.0, 0.5);
    //    static public Orientation flat = new Orientation(
    //        3.0 / 2.0,
    //        0.0,
    //        Math.Sqrt(3.0) / 2.0,
    //        Math.Sqrt(3.0), 
    //        2.0 / 3.0,
    //        0.0,
    //        -1.0 / 3.0,
    //        Math.Sqrt(3.0) / 3.0, 
    //        0.0);

    //    static public Vector2 HexToPixel(Layout layout, Hex h)
    //    {
    //        Orientation M = layout.orientation;
    //        Vector2 size = layout.size;
    //        Vector2 origin = layout.origin;
    //        float x = (float)((M.f0 * h.q + M.f1 * h.r) * size.x);
    //        float y = (float)((M.f2 * h.q + M.f3 * h.r) * size.y);
    //        return new Vector2(x + origin.x, y + origin.y);
    //    }


    //    static public FractionalHex PixelToHex(Layout layout, Vector2 p)
    //    {
    //        Orientation M = layout.orientation;
    //        Vector2 size = layout.size;
    //        Vector2 origin = layout.origin;
    //        Vector2 pt = new Vector2((p.x - origin.x) / size.x, (p.y - origin.y) / size.y);
    //        double q = M.b0 * pt.x + M.b1 * pt.y;
    //        double r = M.b2 * pt.x + M.b3 * pt.y;
    //        return new FractionalHex(q, r, -q - r);
    //    }


    //    static public Vector2 HexCornerOffset(Layout layout, int corner)
    //    {
    //        Orientation M = layout.orientation;
    //        Vector2 size = layout.size;
    //        double angle = 2.0 * Math.PI * (M.start_angle - corner) / 6;
    //        return new Vector2((float)(size.x * Math.Cos(angle)), (float)(size.y * Math.Sin(angle)));
    //    }


    //    static public List<Vector2> PolygonCorners(Layout layout, Hex h)
    //    {
    //        List<Vector2> corners = new List<Vector2> { };
    //        Vector2 center = Layout.HexToPixel(layout, h);
    //        for (int i = 0; i < 6; i++)
    //        {
    //            Vector2 offset = Layout.HexCornerOffset(layout, i);
    //            corners.Add(new Vector2(center.x + offset.x, center.y + offset.y));
    //        }
    //        return corners;
    //    }

    //}

}