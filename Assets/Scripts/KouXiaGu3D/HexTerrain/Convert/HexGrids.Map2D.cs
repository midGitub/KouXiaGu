﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    //对 IMap2D 接口的拓展;
    public static partial class HexGrids
    {


        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            var aroundPoints = GetNeighbours(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighboursAndSelfOrDefault<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            var aroundPoints = GetNeighboursAndSelf(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target, HexDirections directions)
        {
            T item;
            var aroundPoints = GetNeighbours(target, directions);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighbours<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            var aroundPoints = GetNeighbours(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighboursAndSelf<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            var aroundPoints = GetNeighboursAndSelf(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<ShortVector2, HexDirections, T>> GetNeighbours<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target, HexDirections directions)
        {
            T item;
            var aroundPoints = GetNeighbours(target, directions);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new CoordPack<ShortVector2, HexDirections, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到满足条件的方向;若方向不存在节点则为不满足;
        /// </summary>
        public static HexDirections GetNeighboursAndSelfMask<T>(this IReadOnlyMap<ShortVector2, T> map, ShortVector2 target, Func<T, bool> func)
        {
            HexDirections directions = 0;
            T item;
            IEnumerable<HexDirections> aroundDirection = GetHexDirectionsAndSelf();
            foreach (var direction in aroundDirection)
            {
                ShortVector2 vePoint = OffSetDirectionVector(target, direction) + target;
                if (map.TryGetValue(vePoint, out item))
                {
                    if (func(item))
                        directions |= direction;
                }
            }
            return directions;
        }




        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            var aroundPoints = GetNeighbours(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new CoordPack<CubicHexCoord, HexDirections, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighboursAndSelfOrDefault<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            var aroundPoints = GetNeighboursAndSelf(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new CoordPack<CubicHexCoord, HexDirections, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target, HexDirections directions)
        {
            T item;
            var aroundPoints = GetNeighbours(target, directions);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new CoordPack<CubicHexCoord, HexDirections, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighbours<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            var aroundPoints = GetNeighbours(target);
            foreach (var point in aroundPoints)
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
        public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighboursAndSelf<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            var aroundPoints = GetNeighboursAndSelf(target);
            foreach (var point in aroundPoints)
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
        public static IEnumerable<CoordPack<CubicHexCoord, HexDirections, T>> GetNeighbours<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target, HexDirections directions)
        {
            T item;
            var aroundPoints = GetNeighbours(target, directions);
            foreach (var point in aroundPoints)
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
        public static HexDirections GetNeighboursAndSelfMask<T>(this IReadOnlyMap<CubicHexCoord, T> map, CubicHexCoord target, Func<T, bool> func)
        {
            HexDirections directions = 0;
            T item;
            IEnumerable<HexDirections> aroundDirection = GetHexDirectionsAndSelf();
            foreach (var direction in aroundDirection)
            {
                CubicHexCoord vePoint = GetDirection(direction) + target;
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
