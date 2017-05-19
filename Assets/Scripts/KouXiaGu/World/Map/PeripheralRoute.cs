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
                            route[0] = MinNeighbourOrSelf(target, targetValue, tryGetValue);
                            route[1] = target;
                            route[2] = neighbour.Point;
                            route[3] = MaxNeighbourOrSelf(neighbour, neighbourValue, tryGetValue);
                            yield return route;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取到值最小的邻居的,若不存在邻居则返回其本身;
        /// </summary>
        /// <param name="target">目标点;</param>
        public static CubicHexCoord MinNeighbourOrSelf(CubicHexCoord target, uint targetValue, TryGetPeripheralValue tryGetValue)
        {
            CubicHexCoord minPos = default(CubicHexCoord);
            uint minValue = uint.MaxValue;
            bool isChanged = false;

            foreach (var neighbour in target.GetNeighbours())
            {
                uint neighbourValue;
                if (tryGetValue(neighbour, out neighbourValue))
                {
                    if (neighbourValue < minValue)
                    {
                        minPos = neighbour;
                        minValue = neighbourValue;
                        isChanged = true;
                    }
                }
            }
            return isChanged ? minPos : target;
        }

        /// <summary>
        /// 获取到值最大的邻居,若不存在邻居则返回其本身;
        /// </summary>
        /// <param name="target">目标点;</param>
        public static CubicHexCoord MaxNeighbourOrSelf(CubicHexCoord target, uint targetValue, TryGetPeripheralValue tryGetValue)
        {
            CubicHexCoord maxPos = default(CubicHexCoord);
            uint maxValue = 0;
            bool isChanged = false;

            foreach (var neighbour in target.GetNeighbours())
            {
                uint neighbourValue;
                if (tryGetValue(neighbour, out neighbourValue))
                {
                    if (neighbourValue > maxValue)
                    {
                        maxPos = neighbour;
                        maxValue = neighbourValue;
                        isChanged = true;
                    }
                }
            }
            return isChanged ? maxPos : target;
        }
    }
}
