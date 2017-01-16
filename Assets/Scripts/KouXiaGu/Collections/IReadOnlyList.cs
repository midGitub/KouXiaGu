using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Collections
{

    public interface IReadOnlyList<T> : IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }

}
