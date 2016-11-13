using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 只读的地图;
    /// </summary>
    public interface IReadOnlyMap<T>
    {
        T this[IntVector2 position] { get; set; }

        bool ContainsPosition(IntVector2 position);
        bool TryGetValue(IntVector2 position, out T item);
    }

}
