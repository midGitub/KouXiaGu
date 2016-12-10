using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public interface IReadOnlyMap<TP, T>
    {
        T this[TP position] { get; }
        int Count { get; }

        bool Contains(TP position);
        bool TryGetValue(TP position, out T item);
    }

    public interface IMap<TP, T> : IEnumerable<KeyValuePair<TP, T>>
    {

        T this[TP position] { get; set; }
        IEnumerable<TP> Points { get; }
        IEnumerable<T> Nodes { get; }
        int Count { get; }

        void Add(TP position, T item);
        bool Remove(TP position);
        bool Contains(TP position);
        bool TryGetValue(TP position, out T item);
        void Clear();
    }

}
