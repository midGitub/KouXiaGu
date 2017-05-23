using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    public interface INavigationMap<T>
    {
        bool CanWalk(T position);
        NodeInfo<T> GetInfo(T position, T destination);
        IEnumerable<T> GetNeighbors(T position);
    }

    public struct NodeInfo<T>
    {
        public T Position;
        public T Destination;
        public bool IsWalkable;
        public int Cost;
    }
}
