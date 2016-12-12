using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Grids
{


    public static class IGridExtensions
    {


        /// <summary>
        /// 广度遍历;
        /// </summary>
        /// <param name="close">这个节点是否为关闭状态,若为关闭状态则返回,也不返回其邻居节点;</param>
        /// <param name="capacity">估计返回的节点数</param>
        public static IEnumerable<TC> BreadthTraversal<TC>(this TC target, Func<TC, bool> close, int capacity = 81)
             where TC : IGrid
        {
            Queue<IGrid> waitPoints;
            HashSet<IGrid> returnedPoints;

            if (!close(target))
            {
                yield return target;

                waitPoints = new Queue<IGrid>(capacity);
                returnedPoints = new HashSet<IGrid>();

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
        /// 广度遍历;
        /// </summary>
        /// <param name="capacity">估计返回的节点数</param>
        public static IEnumerable<CoordPack<TC, T>> BreadthTraversal<TC, T>(this IMap<TC, T> map, TC target, int capacity = 81)
             where TC : IGrid
        {
            IEnumerable<TC> breadthTraversalPoints = target.BreadthTraversal(point => !map.Contains(point));

            foreach (var point in breadthTraversalPoints)
            {
                yield return new CoordPack<TC, T>(point, map[point]);
            }
        }



    }

}
