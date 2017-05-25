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
        public bool IsWalkable;
        public int Cost;
    }
}
