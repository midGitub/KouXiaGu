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
        NavigationNode<T> GetInfo(T position, T destination);
    }
}
