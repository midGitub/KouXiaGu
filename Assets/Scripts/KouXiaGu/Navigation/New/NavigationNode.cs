using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    public struct NavigationNode<T>
    {
        public T Position;
        public T Destination;

        /// <summary>
        /// 是否可用?
        /// </summary>
        public bool IsWalkable;

        /// <summary>
        /// 到终点的代价值;
        /// </summary>
        public int Cost;
    }
}
