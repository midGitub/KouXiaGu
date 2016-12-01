using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu3D.Map
{


    public interface IMap2D<T> : IEnumerable<KeyValuePair<ShortVector2, T>>
    {

        T this[ShortVector2 position] { get; set; }

        void Add(ShortVector2 position, T item);
        bool Remove(ShortVector2 position);
        bool Contains(ShortVector2 position);
        bool TryGetValue(ShortVector2 position, out T item);

        void Clear();
    }

}
