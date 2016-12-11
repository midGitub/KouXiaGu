using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 六边形偏移网格;
    /// </summary>
    [Obsolete]
    public static class HexOffsetGrids
    {


        ///// <summary>
        ///// 立方体坐标 转换为 偏移坐标;
        ///// </summary>
        //public static ShortVector2 HexToOffset(this CubicHexCoord hex)
        //{
        //    int x = hex.X;
        //    int y = hex.Y + (int)((hex.X + 1 * (hex.X & 1)) / 2);
        //    return new ShortVector2(x, y);
        //}

        ///// <summary>
        ///// 偏移坐标 转换成 立方体坐标;
        ///// </summary>
        //public static CubicHexCoord OffsetToHex(ShortVector2 offset)
        //{
        //    int x = offset.x;
        //    int y = offset.y - (int)((offset.x + 1 * (offset.x & 1)) / 2);
        //    int z = -x - y;
        //    return new CubicHexCoord(x, y, z);
        //}


        ///// <summary>
        ///// 从像素坐标获取到所在的 偏移坐标;
        ///// </summary>
        //public static ShortVector2 Pixel2DToOffset(Vector2 point)
        //{
        //    CubicHexCoord hex = Pixel2DToHex(point);
        //    return HexToOffset(hex);
        //}

        ///// <summary>
        ///// 3D像素坐标 转换到 偏移坐标;
        ///// </summary>
        //public static ShortVector2 PixelToOffset(Vector3 point)
        //{
        //    CubicHexCoord hex = Pixel2DToHex(new Vector2(point.x, point.z));
        //    return HexToOffset(hex);
        //}

        ///// <summary>
        ///// 偏移坐标 转换成 像素坐标;
        ///// </summary>
        //public static Vector2 OffsetToPixel2D(ShortVector2 offset)
        //{
        //    float x = (float)(OuterRadius * 1.5f * offset.x);
        //    float y = (float)(OuterRadius * Math.Sqrt(3) * (offset.y - 0.5 * (offset.x & 1)));
        //    return new Vector2(x, y);
        //}

        ///// <summary>
        ///// 偏移坐标 转换成 3D的像素坐标;
        ///// </summary>
        //public static Vector3 OffsetToPixel(ShortVector2 offset, float y = 0)
        //{
        //    Vector2 v2 = OffsetToPixel2D(offset);
        //    return new Vector3(v2.x, y, v2.y);
        //}


        ///// <summary>
        ///// 曼哈顿距离;
        ///// </summary>
        //public static int ManhattanDistances(ShortVector2 offset1, ShortVector2 offset2)
        //{
        //    CubicHexCoord hex1 = OffsetToHex(offset1);
        //    CubicHexCoord hex2 = OffsetToHex(offset2);
        //    return ManhattanDistances(hex1, hex2);
        //}

        //#region 偏移坐标方向;

        ///// <summary>
        ///// 方向转换偏移量合集;
        ///// </summary>
        //static readonly Dictionary<int, DirectionVector> DirectionVectorSet = GetDirectionVector();

        //static Dictionary<int, DirectionVector> GetDirectionVector()
        //{
        //    var directionVectorSet = new Dictionary<int, DirectionVector>(DirectionsNumber);

        //    AddIn(directionVectorSet, HexDirections.North, new ShortVector2(0, 1), new ShortVector2(0, 1));
        //    AddIn(directionVectorSet, HexDirections.Northeast, new ShortVector2(1, 0), new ShortVector2(1, 1));
        //    AddIn(directionVectorSet, HexDirections.Southeast, new ShortVector2(1, -1), new ShortVector2(1, 0));
        //    AddIn(directionVectorSet, HexDirections.South, new ShortVector2(0, -1), new ShortVector2(0, -1));
        //    AddIn(directionVectorSet, HexDirections.Southwest, new ShortVector2(-1, -1), new ShortVector2(-1, 0));
        //    AddIn(directionVectorSet, HexDirections.Northwest, new ShortVector2(-1, 0), new ShortVector2(-1, 1));
        //    AddIn(directionVectorSet, HexDirections.Self, new ShortVector2(0, 0), new ShortVector2(0, 0));

        //    return directionVectorSet;
        //}

        //static void AddIn(Dictionary<int, DirectionVector> directionVectorDictionary,
        //    HexDirections direction, ShortVector2 oddVector, ShortVector2 evenVector)
        //{
        //    DirectionVector directionVector = new DirectionVector(direction, oddVector, evenVector);
        //    directionVectorDictionary.Add((int)direction, directionVector);
        //}

        ///// <summary>
        ///// 获取到这个地图坐标这个方向需要偏移的量(不进行相加,仅是偏移量);
        ///// </summary>
        //public static ShortVector2 OffSetDirectionVector(ShortVector2 target, HexDirections direction)
        //{
        //    DirectionVector directionVector = DirectionVectorSet[(int)direction];
        //    if ((target.x & 1) == 1)
        //    {
        //        return directionVector.OddVector;
        //    }
        //    else
        //    {
        //        return directionVector.EvenVector;
        //    }
        //}

        ///// <summary>
        ///// 六边形 x轴奇数位和偶数位 对应方向的偏移向量;
        ///// </summary>
        //struct DirectionVector
        //{
        //    public DirectionVector(HexDirections direction, ShortVector2 oddVector, ShortVector2 evenVector)
        //    {
        //        this.Direction = direction;
        //        this.OddVector = oddVector;
        //        this.EvenVector = evenVector;
        //    }

        //    public HexDirections Direction { get; private set; }
        //    public ShortVector2 OddVector { get; private set; }
        //    public ShortVector2 EvenVector { get; private set; }
        //}

        //#endregion



        ///// <summary>
        ///// 获取到这个点周围的方向和坐标;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<KeyValuePair<HexDirections, ShortVector2>> GetNeighbours(ShortVector2 target)
        //{
        //    foreach (var direction in GetHexDirections())
        //    {
        //        ShortVector2 point = OffSetDirectionVector(target, direction) + target;
        //        yield return new KeyValuePair<HexDirections, ShortVector2>(direction, point);
        //    }
        //}

        ///// <summary>
        ///// 获取到这个点本身和周围的方向和坐标;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<KeyValuePair<HexDirections, ShortVector2>> GetNeighboursAndSelf(ShortVector2 target)
        //{
        //    foreach (var direction in GetHexDirectionsAndSelf())
        //    {
        //        ShortVector2 point = OffSetDirectionVector(target, direction) + target;
        //        yield return new KeyValuePair<HexDirections, ShortVector2>(direction, point);
        //    }
        //}

        ///// <summary>
        ///// 获取到这个点这些方向的坐标和方向,从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<KeyValuePair<HexDirections, ShortVector2>> GetNeighbours(ShortVector2 target, HexDirections directions)
        //{
        //    foreach (var direction in GetHexDirections(directions))
        //    {
        //        ShortVector2 point = OffSetDirectionVector(target, direction) + target;
        //        yield return new KeyValuePair<HexDirections, ShortVector2>(direction, point);
        //    }
        //}



        ///// <summary>
        ///// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target)
        //{
        //    T item;
        //    var aroundPoints = GetNeighbours(target);
        //    foreach (var point in aroundPoints)
        //    {
        //        if (!map.TryGetValue(point.Value, out item))
        //        {
        //            item = default(T);
        //        }
        //        yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
        //    }
        //}

        ///// <summary>
        ///// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighboursAndSelfOrDefault<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target)
        //{
        //    T item;
        //    var aroundPoints = GetNeighboursAndSelf(target);
        //    foreach (var point in aroundPoints)
        //    {
        //        if (!map.TryGetValue(point.Value, out item))
        //        {
        //            item = default(T);
        //        }
        //        yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
        //    }
        //}

        ///// <summary>
        ///// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target, HexDirections directions)
        //{
        //    T item;
        //    var aroundPoints = GetNeighbours(target, directions);
        //    foreach (var point in aroundPoints)
        //    {
        //        if (!map.TryGetValue(point.Value, out item))
        //        {
        //            item = default(T);
        //        }
        //        yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
        //    }
        //}

        ///// <summary>
        ///// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighbours<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target)
        //{
        //    T item;
        //    var aroundPoints = GetNeighbours(target);
        //    foreach (var point in aroundPoints)
        //    {
        //        if (map.TryGetValue(point.Value, out item))
        //        {
        //            yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighboursAndSelf<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target)
        //{
        //    T item;
        //    var aroundPoints = GetNeighboursAndSelf(target);
        //    foreach (var point in aroundPoints)
        //    {
        //        if (map.TryGetValue(point.Value, out item))
        //        {
        //            yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighbours<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target, HexDirections directions)
        //{
        //    T item;
        //    var aroundPoints = GetNeighbours(target, directions);
        //    foreach (var point in aroundPoints)
        //    {
        //        if (map.TryGetValue(point.Value, out item))
        //        {
        //            yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 获取到满足条件的方向;若方向不存在节点则为不满足;
        ///// </summary>
        //public static HexDirections GetNeighboursAndSelfMask<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target, Func<T, bool> func)
        //{
        //    HexDirections directions = 0;
        //    T item;
        //    IEnumerable<HexDirections> aroundDirection = GetHexDirectionsAndSelf();
        //    foreach (var direction in aroundDirection)
        //    {
        //        ShortVector2 vePoint = OffSetDirectionVector(target, direction) + target;
        //        if (map.TryGetValue(vePoint, out item))
        //        {
        //            if (func(item))
        //                directions |= direction;
        //        }
        //    }
        //    return directions;
        //}


    }

}
