using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 六边形网格拓展;
    /// </summary>
    public static partial class HexGridConvert
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


        #region 坐标转换;

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

        #endregion


        //#region 距离

        ///// <summary>
        ///// 曼哈顿距离;
        ///// </summary>
        //public static int ManhattanDistances(this CubicHexCoord a, CubicHexCoord b)
        //{
        //    CubicHexCoord hex = a - b;
        //    return (int)((Math.Abs(hex.X) + Math.Abs(hex.Y) + Math.Abs(hex.Z)) / 2);
        //}

        //#endregion


        //#region 方向;

        //#region 立方体坐标方向;

        ///// <summary>
        ///// 存在方向数;
        ///// </summary>
        //public const int DirectionsNumber = 7;

        ///// <summary>
        ///// 方向偏移量;
        ///// </summary>
        //static Dictionary<int, CubicHexCoord> directions = HexDirectionDictionary();

        ///// <summary>
        ///// 获取到方向偏移量;
        ///// </summary>
        //static Dictionary<int, CubicHexCoord> HexDirectionDictionary()
        //{
        //    Dictionary<int, CubicHexCoord> directions = new Dictionary<int, CubicHexCoord>(DirectionsNumber);

        //    directions.Add((int)HexDirections.North, CubicHexCoord.DIR_North);
        //    directions.Add((int)HexDirections.Northeast, CubicHexCoord.DIR_Northeast);
        //    directions.Add((int)HexDirections.Southeast, CubicHexCoord.DIR_Southeast);
        //    directions.Add((int)HexDirections.South, CubicHexCoord.DIR_South);
        //    directions.Add((int)HexDirections.Southwest, CubicHexCoord.DIR_Southwest);
        //    directions.Add((int)HexDirections.Northwest, CubicHexCoord.DIR_Northwest);
        //    directions.Add((int)HexDirections.Self, CubicHexCoord.Zero);

        //    return directions;
        //}

        ///// <summary>
        ///// 获取到方向偏移量;
        ///// </summary>
        //public static CubicHexCoord GetDirection(HexDirections direction)
        //{
        //    return directions[(int)direction];
        //}

        ///// <summary>
        ///// 获取到这个方向的坐标;
        ///// </summary>
        //public static CubicHexCoord GetDirection(this CubicHexCoord coord, HexDirections direction)
        //{
        //    return coord + GetDirection(direction);
        //}

        //#endregion

        //#region 立方体坐标对角线方向;

        ///// <summary>
        ///// 存在的对角线方向数目;
        ///// </summary>
        //public const int DiagonalsNumber = 7;

        ///// <summary>
        ///// 对角线偏移量;
        ///// </summary>
        //static Dictionary<int, CubicHexCoord> diagonals = HexDiagonalDictionary();

        ///// <summary>
        ///// 获取到对角线偏移量;
        ///// </summary>
        //static Dictionary<int, CubicHexCoord> HexDiagonalDictionary()
        //{
        //    Dictionary<int, CubicHexCoord> diagonals = new Dictionary<int, CubicHexCoord>(DiagonalsNumber);

        //    diagonals.Add((int)HexDiagonals.Northeast, CubicHexCoord.DIA_Northeast);
        //    diagonals.Add((int)HexDiagonals.East, CubicHexCoord.DIA_East);
        //    diagonals.Add((int)HexDiagonals.Southeast, CubicHexCoord.DIA_Southeast);
        //    diagonals.Add((int)HexDiagonals.Southwest, CubicHexCoord.DIA_Southwest);
        //    diagonals.Add((int)HexDiagonals.West, CubicHexCoord.DIA_West);
        //    diagonals.Add((int)HexDiagonals.Northwest, CubicHexCoord.DIA_Northwest);
        //    diagonals.Add((int)HexDiagonals.Self, CubicHexCoord.Zero);

        //    return diagonals;
        //}

        ///// <summary>
        ///// 获取到对角线偏移量;
        ///// </summary>
        //public static CubicHexCoord GetDiagonal(HexDiagonals diagonal)
        //{
        //    return diagonals[(int)diagonal];
        //}

        ///// <summary>
        ///// 获取到这个对角线的偏移量;
        ///// </summary>
        //public static CubicHexCoord GetDiagonal(this CubicHexCoord coord, HexDiagonals diagonal)
        //{
        //    return coord + GetDiagonal(diagonal);
        //}

        //#endregion

        //#endregion


        //#region 经过排序的方向;

        //const int maxDirectionMark = (int)HexDirections.Self;
        //const int minDirectionMark = (int)HexDirections.North;

        ///// <summary>
        ///// 按标记为从 高位到低位 循序排列的数组;不包含本身
        ///// </summary>
        //static readonly HexDirections[] DirectionsArray = new HexDirections[]
        //{
        //    HexDirections.Northwest,
        //    HexDirections.Southwest,
        //    HexDirections.South,
        //    HexDirections.Southeast,
        //    HexDirections.Northeast,
        //    HexDirections.North,
        //};

        ///// <summary>
        ///// 按标记为从 高位到低位 循序排列的数组;
        ///// </summary>
        //static readonly HexDirections[] DirectionsAndSelfArray = Enum.GetValues(typeof(HexDirections)).
        //    Cast<HexDirections>().Reverse().ToArray();

        ///// <summary>
        ///// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        ///// </summary>
        //public static IEnumerable<HexDirections> GetDirections()
        //{
        //    return DirectionsArray;
        //}

        ///// <summary>
        ///// 获取到从 高位到低位 顺序返回的迭代结构;包括本身;
        ///// </summary>
        //public static IEnumerable<HexDirections> GetDirectionsAndSelf()
        //{
        //    return DirectionsAndSelfArray;
        //}

        ///// <summary>
        ///// 获取到方向集表示的所有方向;
        ///// </summary>
        //public static IEnumerable<HexDirections> GetDirections(HexDirections directions)
        //{
        //    int mask = (int)directions;
        //    for (int intDirection = minDirectionMark; intDirection <= maxDirectionMark; intDirection <<= 1)
        //    {
        //        if ((intDirection & mask) == 1)
        //        {
        //            yield return (HexDirections)intDirection;
        //        }
        //    }
        //}

        //#endregion



        ///// <summary>
        ///// 获取到这个点周围的方向和坐标;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<KeyValuePair<HexDirections, CubicHexCoord>> GetNeighbours(this CubicHexCoord target)
        //{
        //    foreach (var direction in GetDirections())
        //    {
        //        CubicHexCoord point = target.GetDirection(direction);
        //        yield return new KeyValuePair<HexDirections, CubicHexCoord>(direction, point);
        //    }
        //}

        ///// <summary>
        ///// 获取到这个点本身和周围的方向和坐标;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<KeyValuePair<HexDirections, CubicHexCoord>> GetNeighboursAndSelf(this CubicHexCoord target)
        //{
        //    foreach (var direction in GetDirectionsAndSelf())
        //    {
        //        CubicHexCoord point = target.GetDirection(direction);
        //        yield return new KeyValuePair<HexDirections, CubicHexCoord>(direction, point);
        //    }
        //}

        ///// <summary>
        ///// 获取到这个点这些方向的坐标和方向,从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<KeyValuePair<HexDirections, CubicHexCoord>> GetNeighbours(this CubicHexCoord target, HexDirections directions)
        //{
        //    foreach (var direction in GetDirections(directions))
        //    {
        //        CubicHexCoord point = target.GetDirection(direction);
        //        yield return new KeyValuePair<HexDirections, CubicHexCoord>(direction, point);
        //    }
        //}

        ///// <summary>
        ///// 获取到六边形的范围;
        ///// 半径覆盖到的节点;
        ///// </summary>
        //public static IEnumerable<CubicHexCoord> GetHexRange(CubicHexCoord target, int step)
        //{
        //    for (int x = -step; x <= step; x++)
        //    {
        //        for (int y = Math.Max(-step, -x - step); y <= Math.Min(step, -x + step); y++)
        //        {
        //            int z = -x - y;
        //            yield return new CubicHexCoord(x, y, z);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 获取到目标所在的半径;
        ///// </summary>
        //public static int GetRadius(CubicHexCoord center, CubicHexCoord target)
        //{
        //    var cube = target - center;
        //    return GetRadius(cube);
        //}

        ///// <summary>
        ///// 获取到目标所在 (0,0,0) 的半径;
        ///// </summary>
        //public static int GetRadius(CubicHexCoord target)
        //{
        //    return Mathf.Max(Mathf.Abs(target.X), Mathf.Abs(target.Y), Mathf.Abs(target.Z));
        //}


        ///// <summary>
        /////  按 环状 返回点;
        ///// </summary>
        ///// <param name="radius">需要大于0</param>
        //public static IEnumerable<CubicHexCoord> GetHexRing(CubicHexCoord center, int radius)
        //{
        //    var cube = center + (CubicHexCoord.GetDirectionOffset(HexDirections.Northeast) * radius);

        //    foreach (var direction in GetDirections())
        //    {
        //        for (int j = 0; j < radius; j++)
        //        {
        //            yield return cube;
        //            cube = cube.GetDirection(direction);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 按 螺旋 形状返回点;
        ///// </summary>
        ///// <param name="radius">需要大于0</param>
        //public static List<CubicHexCoord> GetHexSpiral(CubicHexCoord center, int radius)
        //{
        //    List<CubicHexCoord> coords = new List<CubicHexCoord>();
        //    for (int k = 1; k <= radius; k++)
        //    {
        //        coords.AddRange(GetHexRing(center, k));
        //    }
        //    return coords;
        //}


    }

}