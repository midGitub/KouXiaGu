using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Grids
{

    //对 IMap2D 接口的拓展;
    public static partial class HexGrids
    {

        ///// <summary>
        ///// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target)
        //{
        //    T item;
        //    var aroundPoints = GetNeighbours(target);
        //    foreach (var point in aroundPoints)
        //    {
        //        if (!map.TryGetValue(point.Value, out item))
        //        {
        //            item = default(T);
        //        }
        //        yield return new CoordPack<CubicHexCoord, HexDirections, T>(point.Key, point.Value, item);
        //    }
        //}

        ///// <summary>
        ///// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighboursAndSelfOrDefault<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target)
        //{
        //    T item;
        //    var aroundPoints = GetNeighboursAndSelf(target);
        //    foreach (var point in aroundPoints)
        //    {
        //        if (!map.TryGetValue(point.Value, out item))
        //        {
        //            item = default(T);
        //        }
        //        yield return new CoordPack<CubicHexCoord, HexDirections, T>(point.Key, point.Value, item);
        //    }
        //}

        ///// <summary>
        ///// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        ///// </summary>
        //public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target, HexDirections directions)
        //{
        //    T item;
        //    var aroundPoints = GetNeighbours(target, directions);
        //    foreach (var point in aroundPoints)
        //    {
        //        if (!map.TryGetValue(point.Value, out item))
        //        {
        //            item = default(T);
        //        }
        //        yield return new CoordPack<CubicHexCoord, HexDirections, T>(point.Key, point.Value, item);
        //    }
        //}

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighbours<T>(this IMap<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            foreach (var point in target.GetNeighbours())
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new CoordPack<CubicHexCoord, HexDirections, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighboursAndSelf<T>(this IMap<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            foreach (var point in target.GetNeighboursAndSelf())
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new CoordPack<CubicHexCoord, HexDirections, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighbours<T>(this IMap<CubicHexCoord, T> map, CubicHexCoord target, HexDirections directions)
        {
            T item;
            foreach (var point in target.GetNeighbours(directions))
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new CoordPack<CubicHexCoord, HexDirections, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到满足条件的方向;若方向不存在节点则为不满足;
        /// </summary>
        public static HexDirections GetNeighboursAndSelfMask<T>(this IMap<CubicHexCoord, T> map, CubicHexCoord target, Func<T, bool> func)
        {
            HexDirections directions = 0;
            T item;
            IEnumerable<HexDirections> aroundDirection = HexGrids.GetDirectionsAndSelf();
            foreach (var direction in aroundDirection)
            {
                CubicHexCoord vePoint = target.GetDirection(direction);
                if (map.TryGetValue(vePoint, out item))
                {
                    if (func(item))
                        directions |= direction;
                }
            }
            return directions;
        }

    }

}
