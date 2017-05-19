using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 根据点周围给定的数值,获取到多条路径信息;
    /// </summary>
    public static class PeripheralRoute
    {

        public delegate bool TryGetPeripheralValue(CubicHexCoord position, out uint value);


        [Obsolete]
        public static IEnumerable<CubicHexCoord[]> GetWallRoutes(CubicHexCoord target, TryGetPeripheralValue tryGetValue)
        {
            throw new NotImplementedException();
        }

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
                            route[0] = MinNeighbourAndSelf(target, targetValue, tryGetValue);
                            route[1] = target;
                            route[2] = neighbour.Point;
                            route[3] = MaxNeighbourAndSelf(neighbour, neighbourValue, tryGetValue);
                            yield return route;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取到值最小的邻居或本身节点;
        /// </summary>
        /// <param name="target">目标点;</param>
        public static CubicHexCoord MinNeighbourAndSelf(CubicHexCoord target, uint targetValue, TryGetPeripheralValue tryGetValue)
        {
            CubicHexCoord minPos = target;
            uint minValue = targetValue;

            foreach (var neighbour in target.GetNeighbours())
            {
                uint neighbourValue;
                if (tryGetValue(neighbour, out neighbourValue))
                {
                    if (neighbourValue < minValue)
                    {
                        minPos = neighbour;
                        minValue = neighbourValue;
                    }
                }
            }
            return minPos;
        }

        /// <summary>
        /// 获取到值最大的邻居或本身节点;
        /// </summary>
        /// <param name="target">目标点;</param>
        public static CubicHexCoord MaxNeighbourAndSelf(CubicHexCoord target, uint targetValue, TryGetPeripheralValue tryGetValue)
        {
            CubicHexCoord maxPos = target;
            uint maxValue = targetValue;

            foreach (var neighbour in target.GetNeighbours())
            {
                uint neighbourValue;
                if (tryGetValue(neighbour, out neighbourValue))
                {
                    if (neighbourValue > maxValue)
                    {
                        maxPos = neighbour;
                        maxValue = neighbourValue;
                    }
                }
            }
            return maxPos;
        }
    }
}
