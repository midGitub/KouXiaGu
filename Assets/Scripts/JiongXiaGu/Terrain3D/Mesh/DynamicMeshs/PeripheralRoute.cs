using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Terrain3D.DynamicMeshs
{

    /// <summary>
    /// 升序;
    /// </summary>
    class UintAscendingComparer : IComparer<uint>
    {
        public static readonly UintAscendingComparer Default = new UintAscendingComparer();

        public int Compare(uint x, uint y)
        {
            if (x > y)
                return 1;
            else if (x < y)
                return -1;
            else
                return 0;
        }
    }

    /// <summary>
    /// 降序;
    /// </summary>
    class UintDescendingComparer : IComparer<uint>
    {
        public static readonly UintDescendingComparer Default = new UintDescendingComparer();

        public int Compare(uint x, uint y)
        {
            if (x > y)
                return -1;
            else if (x < y)
                return 1;
            else
                return 0;
        }
    }

    public delegate bool TryGetPeripheralValue(CubicHexCoord position, out uint value);

    /// <summary>
    /// 根据点周围给定的数值,获取到多条路径信息;
    /// </summary>
    public static class PeripheralRoute
    {
        /// <summary>
        /// 迭代获取到这个点通往价值大于本身的邻居点的路径点,若不存在节点则不进行迭代;
        /// </summary>
        public static IEnumerable<CubicHexCoord[]> GetRoadRoutes(CubicHexCoord target, TryGetPeripheralValue tryGetValue)
        {
            uint targetValue;
            if (tryGetValue(target, out targetValue))
            {
                foreach (var neighbour in target.GetNeighbours())
                {
                    uint neighbourValue;
                    if (tryGetValue(neighbour, out neighbourValue))
                    {
                        if (neighbourValue > targetValue)
                        {
                            CubicHexCoord[] route = new CubicHexCoord[4];
                            route[0] = MinNeighbourOrSelf(target, tryGetValue);
                            route[1] = target;
                            route[2] = neighbour.Point;
                            route[3] = MaxNeighbourOrSelf(neighbour, tryGetValue);
                            yield return route;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取到邻居排序结构;
        /// </summary>
        public static SortedList<uint, CoordPack<CubicHexCoord, HexDirections>> SortNeighbours(CubicHexCoord target, TryGetPeripheralValue tryGetValue, IComparer<uint> comparer)
        {
            var sortedList = new SortedList<uint, CoordPack<CubicHexCoord, HexDirections>>(comparer);
            foreach (CoordPack<CubicHexCoord, HexDirections> neighbour in target.GetNeighbours())
            {
                uint neighbourValue;
                if (tryGetValue(neighbour, out neighbourValue))
                {
                    sortedList.Add(neighbourValue, neighbour);
                }
            }
            return sortedList;
        }

        /// <summary>
        /// 获取到值最小的邻居的,若不存在邻居则返回其本身;
        /// </summary>
        /// <param name="target">目标点;</param>
        public static CoordPack<CubicHexCoord, HexDirections> MinNeighbourOrSelf(CubicHexCoord target, TryGetPeripheralValue tryGetValue)
        {
            CoordPack<CubicHexCoord, HexDirections> min;
            if (TryGetMinNeighbour(target, tryGetValue, out min))
            {
                return min;
            }
            return new CoordPack<CubicHexCoord, HexDirections>(target, HexDirections.Self);
        }

        /// <summary>
        /// 尝试获取到最小的邻居;
        /// </summary>
        public static bool TryGetMinNeighbour(CubicHexCoord target, TryGetPeripheralValue tryGetValue, out CoordPack<CubicHexCoord, HexDirections> min)
        {
            min = default(CoordPack<CubicHexCoord, HexDirections>);
            uint minValue = uint.MaxValue;
            bool isChanged = false;

            foreach (CoordPack<CubicHexCoord, HexDirections> neighbour in target.GetNeighbours())
            {
                uint neighbourValue;
                if (tryGetValue(neighbour, out neighbourValue))
                {
                    if (neighbourValue < minValue)
                    {
                        min = neighbour;
                        minValue = neighbourValue;
                        isChanged = true;
                    }
                }
            }
            return isChanged;
        }


        /// <summary>
        /// 获取到值最大的邻居,若不存在邻居则返回其本身;
        /// </summary>
        /// <param name="target">目标点;</param>
        public static CoordPack<CubicHexCoord, HexDirections> MaxNeighbourOrSelf(CubicHexCoord target, TryGetPeripheralValue tryGetValue)
        {
            CoordPack<CubicHexCoord, HexDirections> max;
            if (TryGetMaxNeighbour(target, tryGetValue, out max))
            {
                return max;
            }
            return new CoordPack<CubicHexCoord, HexDirections>(target, HexDirections.Self);
        }

        /// <summary>
        /// 尝试获取到最大邻居;
        /// </summary>
        public static bool TryGetMaxNeighbour(CubicHexCoord target, TryGetPeripheralValue tryGetValue, out CoordPack<CubicHexCoord, HexDirections> max)
        {
            max = default(CoordPack<CubicHexCoord, HexDirections>);
            uint maxValue = 0;
            bool isChanged = false;

            foreach (var neighbour in target.GetNeighbours())
            {
                uint neighbourValue;
                if (tryGetValue(neighbour, out neighbourValue))
                {
                    if (neighbourValue > maxValue)
                    {
                        max = neighbour;
                        maxValue = neighbourValue;
                        isChanged = true;
                    }
                }
            }
            return isChanged;
        }
    }
}
