using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Grids
{


    public static partial class RecGrids
    {


        /// <summary>
        /// 获取到目标节点邻居节点,若节点不存在则不返回;
        /// </summary>
        public static IEnumerable<CoordPack<ShortVector2, RecDirections, T>> GetNeighbours<T>(this IMap<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            foreach (var direction in Directions)
            {
                ShortVector2 offsetCoord = target.GetDirection(direction);
                if (map.TryGetValue(offsetCoord, out item))
                {
                    yield return new CoordPack<ShortVector2, RecDirections, T>(direction, offsetCoord, item);
                }
            }
        }

        /// <summary>
        /// 获取到目标节点和其邻居节点,若节点不存在则不返回;(存在本身方向,且最先返回);
        /// </summary>
        public static IEnumerable<CoordPack<ShortVector2, RecDirections, T>> GetNeighboursAndSelf<T>(this IMap<ShortVector2, T> map, ShortVector2 target)
        {
            T item;
            foreach (var direction in DirectionsAndSelf)
            {
                ShortVector2 offsetCoord = target.GetDirection(direction);
                if (map.TryGetValue(offsetCoord, out item))
                {
                    yield return new CoordPack<ShortVector2, RecDirections, T>(direction, offsetCoord, item);
                }
            }
        }




        /// <summary>
        /// 广度遍历;
        /// </summary>
        /// <param name="capacity">估计返回的节点数</param>
        public static IEnumerable<CoordPack<ShortVector2, T>> BreadthTraversal<T>(this IMap<ShortVector2, T> map, ShortVector2 target, int capacity = 81)
        {
            IEnumerable<ShortVector2> breadthTraversalPoints = target.BreadthTraversal(point => !map.Contains(point));

            foreach (var point in breadthTraversalPoints)
            {
                yield return new CoordPack<ShortVector2, T>(point, map[point]);
            }

        }

    }

}
