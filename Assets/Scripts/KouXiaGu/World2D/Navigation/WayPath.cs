using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 行走到的路径;
    /// </summary>
    public class WayPath : IEnumerable<ShortVector2>
    {
        public WayPath()
        {
            wayPath = new LinkedList<ShortVector2>();
        }

        /// <summary>
        /// 起点;
        /// </summary>
        public LinkedListNode<ShortVector2> StartingNode
        {
            get { return wayPath.First; }
        }
        /// <summary>
        /// 寻路的终点;
        /// </summary>
        public LinkedListNode<ShortVector2> DestinationNode
        {
            get { return wayPath.Last; }
        }
        /// <summary>
        /// 路径点合集;
        /// </summary>
        public LinkedList<ShortVector2> wayPath { get; private set; }

        /// <summary>
        /// 加入点到起点之前;
        /// </summary>
        public void AddFirst(ShortVector2 mapPoint)
        {
            wayPath.AddFirst(mapPoint);
        }
        /// <summary>
        /// 加入到终点之后;
        /// </summary>
        public void Add(ShortVector2 mapPoint)
        {
            wayPath.AddLast(mapPoint);
        }

        /// <summary>
        /// 从起点迭代到终点;
        /// </summary>
        public IEnumerator<ShortVector2> GetEnumerator()
        {
            for (LinkedListNode<ShortVector2> pathNode = wayPath.First; pathNode != null; pathNode = pathNode.Next)
            {
                yield return pathNode.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
