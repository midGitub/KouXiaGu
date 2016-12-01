using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu3D
{

	//对 IMap2D 接口的拓展;
    public static partial class HexGrids
    {

        const int maxDirectionMark = (int)HexDirection.Self;
        const int minDirectionMark = (int)HexDirection.North;

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;不包含本身
        /// </summary>
        static readonly HexDirection[] HexDirectionsArray = new HexDirection[]
        {
            HexDirection.Northwest,
            HexDirection.Southwest,
            HexDirection.South,
            HexDirection.Southeast,
            HexDirection.Northeast,
            HexDirection.North,
        };

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;
        /// </summary>
        static readonly HexDirection[] HexDirectionsAndSelfArray = Enum.GetValues(typeof(HexDirection)).
            Cast<HexDirection>().Reverse().ToArray();

        /// <summary>
        /// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        /// </summary>
        public static IEnumerable<HexDirection> HexDirections()
        {
            return HexDirectionsArray;
        }

        /// <summary>
        /// 获取到从 高位到低位 顺序返回的迭代结构;包括本身;
        /// </summary>
        public static IEnumerable<HexDirection> HexDirectionsAndSelf()
        {
            return HexDirectionsAndSelfArray;
        }

        /// <summary>
        /// 获取到方向集表示的所有方向;
        /// </summary>
        public static IEnumerable<HexDirection> HexDirections(HexDirection directions)
        {
            int mask = (int)directions;
            for (int intDirection = minDirectionMark; intDirection <= maxDirectionMark; intDirection <<= 1)
            {
                if ((intDirection & mask) == 1)
                {
                    yield return (HexDirection)intDirection;
                }
            }
        }


        #region 蜂窝地图拓展;

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<T> GetNeighboursOrDefault<T>(this IReadOnlyMap2D<T> map, ShortVector2 target)
        {
            T item;
            IEnumerable<ShortVector2> aroundPoints = GetNeighboursPoints(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point, out item))
                {
                    item = default(T);
                }
                yield return item;
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<T> GetNeighboursAndSelfOrDefault<T>(this IReadOnlyMap2D<T> map, ShortVector2 target)
        {
            T item;
            IEnumerable<ShortVector2> aroundPoints = GetNeighboursAndSelfPoints(target);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point, out item))
                {
                    item = default(T);
                }
                yield return item;
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点 不存在的点返回返回默认值;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<T> GetNeighboursOrDefault<T>(this IReadOnlyMap2D<T> map, ShortVector2 target, HexDirection directions)
        {
            T item;
            IEnumerable<ShortVector2> aroundPoints = GetNeighboursPoints(target, directions);
            foreach (var point in aroundPoints)
            {
                if (!map.TryGetValue(point, out item))
                {
                    item = default(T);
                }
                yield return item;
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<ShortVector2, T>> GetNeighbours<T>(this IReadOnlyMap2D<T> map, ShortVector2 target)
        {
            T item;
            IEnumerable<ShortVector2> aroundPoints = GetNeighboursPoints(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point, out item))
                {
                    yield return new KeyValuePair<ShortVector2, T>(point, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<ShortVector2, T>> GetNeighboursAndSelf<T>(this IReadOnlyMap2D<T> map, ShortVector2 target)
        {
            T item;
            IEnumerable<ShortVector2> aroundPoints = GetNeighboursAndSelfPoints(target);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point, out item))
                {
                    yield return new KeyValuePair<ShortVector2, T>(point, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个地图结构周围的点,若不存在则不返回;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<ShortVector2, T>> GetNeighbours<T>(this IReadOnlyMap2D<T> map, ShortVector2 target, HexDirection directions)
        {
            T item;
            IEnumerable<ShortVector2> aroundPoints = GetNeighboursPoints(target, directions);
            foreach (var point in aroundPoints)
            {
                if (map.TryGetValue(point, out item))
                {
                    yield return new KeyValuePair<ShortVector2, T>(point, item);
                }
            }
        }

        /// <summary>
        /// 获取到这个点周围的坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighboursPoints(ShortVector2 target)
        {
            foreach (var direction in HexDirections())
            {
                ShortVector2 point = OffSetNeighbor(target, direction);
                yield return point;
            }
        }

        /// <summary>
        /// 获取到这个点本身和周围的坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighboursAndSelfPoints(ShortVector2 target)
        {
            foreach (var direction in HexDirectionsAndSelf())
            {
                ShortVector2 point = OffSetNeighbor(target, direction);
                yield return point;
            }
        }

        /// <summary>
        /// 获取到这个点这些范围内的坐标;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighboursPoints(ShortVector2 target, HexDirection directions)
        {
            foreach (var direction in HexDirections(directions))
            {
                ShortVector2 point = OffSetNeighbor(target, direction);
                yield return point;
            }
        }

        #endregion

        #region 方向标记拓展;

        /// <summary>
        /// 获取到满足条件的方向;若方向不存在节点则为不满足;
        /// </summary>
        public static HexDirection GetAroundAndSelfMask<T>(this IReadOnlyMap2D<T> map, ShortVector2 target, Func<T, bool> func)
        {
            HexDirection directions = 0;
            T item;
            IEnumerable<HexDirection> aroundDirection = HexDirectionsAndSelf();
            foreach (var direction in aroundDirection)
            {
                ShortVector2 vePoint = OffSetNeighbor(target, direction);
                if (map.TryGetValue(vePoint, out item))
                {
                    if (func(item))
                        directions |= direction;
                }
            }
            return directions;
        }

        #endregion

    }

}
