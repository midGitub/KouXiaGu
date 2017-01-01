using System.Collections.Generic;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 导航路径;将地图路径转换成导航路径提供单独实例使用;
    /// </summary>
    public class NavPath<TPoint, TNode>
    {
        public NavPath(LinkedList<TPoint> wayPath, IDictionary<TPoint, TNode> worldMap)
        {
            this.wayPath = wayPath;
            this.WorldMap = worldMap;

            current = wayPath.First;
        }

        /// <summary>
        /// 路径点合集;
        /// </summary>
        LinkedList<TPoint> wayPath;

        /// <summary>
        /// 当前行走到;
        /// </summary>
        LinkedListNode<TPoint> current;

        /// <summary>
        /// 寻路的地图;
        /// </summary>
        public IDictionary<TPoint, TNode> WorldMap { get; private set; }

        /// <summary>
        /// 起点;
        /// </summary>
        public TPoint Starting
        {
            get { return wayPath.First.Value; }
        }

        /// <summary>
        /// 寻路的终点;
        /// </summary>
        public TPoint Destination
        {
            get { return wayPath.Last.Value; }
        }

        /// <summary>
        /// 当前行走到的位置;
        /// </summary>
        public TPoint Current
        {
            get { return current.Value; }
        }

        /// <summary>
        /// 移动到下一步,直到返回false代表已经到终点;
        /// </summary>
        public bool MoveNext()
        {
            if (current.Next == null)
                return false;

            current = current.Next;
            return true;
        }

    }

}
