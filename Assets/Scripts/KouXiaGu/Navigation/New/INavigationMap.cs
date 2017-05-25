using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    public interface INavigationMap<T>
    {
        bool IsWalkable(T position);
        IEnumerable<T> GetNeighbors(T position);
        NavigationNode<T> GetNavigationNode(T position, T destination);
    }
}
