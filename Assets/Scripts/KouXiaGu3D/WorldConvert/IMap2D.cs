using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu3D
{

    public interface IReadOnlyMap2D<T>
    {
        T this[ShortVector2 position] { get; }
        int Count { get; }

        bool Contains(ShortVector2 position);
        bool TryGetValue(ShortVector2 position, out T item);
    }

    public interface IMap2D<T> : IEnumerable<KeyValuePair<ShortVector2, T>>
    {

        T this[ShortVector2 position] { get; set; }
        IEnumerable<ShortVector2> Points { get; }
        IEnumerable<T> Nodes { get; }
        int Count { get; }

        void Add(ShortVector2 position, T item);
        bool Remove(ShortVector2 position);
        bool Contains(ShortVector2 position);
        bool TryGetValue(ShortVector2 position, out T item);
        void Clear();
    }

}
