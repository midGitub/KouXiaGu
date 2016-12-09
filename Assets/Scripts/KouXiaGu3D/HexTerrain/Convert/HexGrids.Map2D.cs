using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
{

    //对 IMap2D 接口的拓展;
    public static partial class HexGrids
    {


        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<ShortVector2, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap2D<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            var aroundPoints = GetNeighbours(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new HexDirectionPack<ShortVector2, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<ShortVector2, T>> GetNeighboursAndSelfOrDefault<T>(this IReadOnlyMap2D<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            var aroundPoints = GetNeighboursAndSelf(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new HexDirectionPack<ShortVector2, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<ShortVector2, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap2D<ShortVector2, T> map, ShortVector2 target, HexDirections directions)
        {
            T item;
            var aroundPoints = GetNeighbours(target, directions);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new HexDirectionPack<ShortVector2, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<ShortVector2, T>> GetNeighbours<T>(this IReadOnlyMap2D<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            var aroundPoints = GetNeighbours(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new HexDirectionPack<ShortVector2, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<ShortVector2, T>> GetNeighboursAndSelf<T>(this IReadOnlyMap2D<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            var aroundPoints = GetNeighboursAndSelf(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new HexDirectionPack<ShortVector2, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<ShortVector2, T>> GetNeighbours<T>(this IReadOnlyMap2D<ShortVector2, T> map, ShortVector2 target, HexDirections directions)
        {
            T item;
            var aroundPoints = GetNeighbours(target, directions);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new HexDirectionPack<ShortVector2, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到满足条件的方向;若方向不存在节点则为不满足;
        /// </summary>
        public static HexDirections GetNeighboursAndSelfMask<T>(this IReadOnlyMap2D<ShortVector2, T> map, ShortVector2 target, Func<T, bool> func)
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
        public static IEnumerable<HexDirectionPack<CubicHexCoord, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap2D<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            var aroundPoints = GetNeighbours(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new HexDirectionPack<CubicHexCoord, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<CubicHexCoord, T>> GetNeighboursAndSelfOrDefault<T>(this IReadOnlyMap2D<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            var aroundPoints = GetNeighboursAndSelf(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new HexDirectionPack<CubicHexCoord, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<CubicHexCoord, T>> GetNeighboursOrDefault<T>(this IReadOnlyMap2D<CubicHexCoord, T> map, CubicHexCoord target, HexDirections directions)
        {
            T item;
            var aroundPoints = GetNeighbours(target, directions);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point.Value, out item))
                {
                    item = default(T);
                }
                yield return new HexDirectionPack<CubicHexCoord, T>(point.Key, point.Value, item);
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<CubicHexCoord, T>> GetNeighbours<T>(this IReadOnlyMap2D<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            var aroundPoints = GetNeighbours(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new HexDirectionPack<CubicHexCoord, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<CubicHexCoord, T>> GetNeighboursAndSelf<T>(this IReadOnlyMap2D<CubicHexCoord, T> map, CubicHexCoord target)
        {
            T item;
            var aroundPoints = GetNeighboursAndSelf(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new HexDirectionPack<CubicHexCoord, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<HexDirectionPack<CubicHexCoord, T>> GetNeighbours<T>(this IReadOnlyMap2D<CubicHexCoord, T> map, CubicHexCoord target, HexDirections directions)
        {
            T item;
            var aroundPoints = GetNeighbours(target, directions);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point.Value, out item))
                {
                    yield return new HexDirectionPack<CubicHexCoord, T>(point.Key, point.Value, item);
                }
            }
        }

        /// <summary>
        /// 获取到满足条件的方向;若方向不存在节点则为不满足;
        /// </summary>
        public static HexDirections GetNeighboursAndSelfMask<T>(this IReadOnlyMap2D<CubicHexCoord, T> map, CubicHexCoord target, Func<T, bool> func)
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
