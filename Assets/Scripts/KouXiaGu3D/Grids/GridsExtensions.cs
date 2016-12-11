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
        /// 广度遍历;
        /// </summary>
        /// <param name="close">这个节点是否为关闭状态,若为关闭状态则返回,也不返回其邻居节点;</param>
        /// <param name="capacity">估计返回的节点数</param>
        public static IEnumerable<IGridPoint> BreadthTraversal(this IGridPoint target, Func<IGridPoint, bool> close, int capacity = 81)
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
                        if (!returnedPoints.Contains(neighbour))
                        {
                            if (close(neighbour))
                            {
                                returnedPoints.Add(neighbour);
                            }
                            else
                            {
                                yield return neighbour;
                                returnedPoints.Add(neighbour);
                                waitPoints.Enqueue(neighbour);
                            }
                        }
                    }
                }
            }
        }

    }

}
