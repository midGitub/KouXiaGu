using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 对网格类型的结构拓展;
    /// </summary>
    public static class GridsExtensions
    {


        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        public static IEnumerable<TC> GetNeighbours<TC, TD>(this TC target, TD directions)
             where TC : IGridPoint<TC, TD>
        {
            foreach (var direction in target.GetDirections(directions))
            {
                yield return (TC)target.GetDirection(direction);
            }
        }

        /// <summary>
        /// 获取到目标节点邻居节点;
        /// </summary>
        public static IEnumerable<CoordPack<TC, TD>> GetNeighbours<TC, TD>(this IGridPoint<TC, TD> target)
             where TC : IGridPoint<TC, TD>
        {
            foreach (var direction in target.Directions)
            {
                TC offsetCoord = (TC)target.GetDirection(direction);
                yield return new CoordPack<TC, TD>(offsetCoord, direction);
            }
        }

        /// <summary>
        /// 获取到目标节点和其邻居节点;(存在本身方向,且最先返回);
        /// </summary>
        public static IEnumerable<CoordPack<TC, TD>> GetNeighboursAndSelf<TC, TD>(this IGridPoint<TC, TD> target)
           where TC : IGridPoint<TC, TD>
        {
            foreach (var direction in target.DirectionsAndSelf)
            {
                TC offsetCoord = (TC)target.GetDirection(direction);
                yield return new CoordPack<TC, TD>(offsetCoord, direction);
            }
        }


        /// <summary>
        /// 广度遍历;
        /// </summary>
        /// <param name="close">这个节点是否为关闭状态,若为关闭状态则返回,也不返回其邻居节点;</param>
        /// <param name="capacity">估计返回的节点数</param>
        public static IEnumerable<TC> BreadthTraversal<TC>(this TC target, Func<TC, bool> close, int capacity = 81)
             where TC : IGridPoint
        {
            if (!close(target))
            {
                yield return target;

                Queue<IGridPoint> waitPoints = new Queue<IGridPoint>(capacity);
                HashSet<IGridPoint> returnedPoints = new HashSet<IGridPoint>();

                waitPoints.Enqueue(target);
                returnedPoints.Add(target);

                while (waitPoints.Count != 0)
                {
                    var point = waitPoints.Dequeue();

                    foreach (var neighbour in point.GetNeighbours())
                    {
                        TC coord = (TC)neighbour;
                        if (!returnedPoints.Contains(neighbour))
                        {
                            if (close(coord))
                            {
                                returnedPoints.Add(neighbour);
                            }
                            else
                            {
                                yield return coord;
                                returnedPoints.Add(neighbour);
                                waitPoints.Enqueue(neighbour);
                            }
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 获取到目标节点邻居节点,若节点不存在则不返回;
        /// </summary>
        public static IEnumerable<CoordPack<TC, TD, T>> GetNeighbours<TC, TD, T>(this IMap<TC, T> map, TC target)
             where TC : IGridPoint<TC, TD>
        {
            T item;
            foreach (var direction in target.Directions)
            {
                TC offsetCoord = (TC)target.GetDirection(direction);
                if (map.TryGetValue(offsetCoord, out item))
                {
                    yield return new CoordPack<TC, TD, T>(direction, offsetCoord, item);
                }
            }
        }

        /// <summary>
        /// 获取到目标节点和其邻居节点,若节点不存在则不返回;(存在本身方向,且最先返回);
        /// </summary>
        public static IEnumerable<CoordPack<TC, TD, T>> GetNeighboursAndSelf<TC, TD, T>(this IMap<TC, T> map, TC target)
             where TC : IGridPoint<TC, TD>
        {
            T item;
            foreach (var direction in target.DirectionsAndSelf)
            {
                TC offsetCoord = (TC)target.GetDirection(direction);
                if (map.TryGetValue(offsetCoord, out item))
                {
                    yield return new CoordPack<TC, TD, T>(direction, offsetCoord, item);
                }
            }
        }

        /// <summary>
        /// 广度遍历;
        /// </summary>
        /// <param name="capacity">估计返回的节点数</param>
        public static IEnumerable<CoordPack<TC, T>> BreadthTraversal<TC, TD, T>(this IMap<TC, T> map, TC target, int capacity = 81)
             where TC : IGridPoint<TC, TD>
        {
            IEnumerable<TC> breadthTraversalPoints = target.BreadthTraversal(point => !map.Contains(point));

            foreach (var point in breadthTraversalPoints)
            {
                yield return new CoordPack<TC, T>(point, map[point]);
            }

        }

        /// <summary>
        /// 获取到满足条件的方向;若方向不存在节点则为不满足;
        /// </summary>
        public static int GetNeighboursAndSelfMask<TC, TD, T>(this IMap<TC, T> map, TC target, Func<T, bool> func)
            where TC : IGridPoint<TC, TD>
            where TD : IConvertible
        {
            int directions = 0;
            T item;
            IEnumerable<TD> aroundDirection = target.DirectionsAndSelf;
            foreach (var direction in aroundDirection)
            {
                TC vePoint = (TC)target.GetDirection(direction);
                if (map.TryGetValue(vePoint, out item))
                {
                    if (func(item))
                        directions |= direction.ToInt32(null);
                }
            }
            return directions;
        }

    }

}
