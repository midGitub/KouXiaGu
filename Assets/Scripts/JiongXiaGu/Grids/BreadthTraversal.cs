using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Grids
{

    /// <summary>
    /// 广度遍历;
    /// </summary>
    public class BreadthTraversal
    {

        Queue<IGrid> waitPoints;
        HashSet<IGrid> returnedPoints;

        public BreadthTraversal()
        {
            waitPoints = new Queue<IGrid>();
            returnedPoints = new HashSet<IGrid>();
        }

        /// <param name="capacity">预计返回的节点数</param>
        public BreadthTraversal(int capacity)
        {
            waitPoints = new Queue<IGrid>(capacity);
            returnedPoints = new HashSet<IGrid>();
        }

        /// <summary>
        /// 广度遍历;
        /// </summary>
        /// <param name="close">这个节点是否为关闭状态,若为关闭状态则返回,也不返回其邻居节点;</param>
        public IEnumerable<TC> Traversal<TC>(TC target, Func<TC, bool> close)
             where TC : IGrid
        {

            if (!close(target))
            {
                yield return target;

                waitPoints.Clear();
                returnedPoints.Clear();

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

    }

}
