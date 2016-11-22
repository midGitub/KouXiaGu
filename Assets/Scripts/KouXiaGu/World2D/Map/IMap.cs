using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 只读的地图;
    /// </summary>
    public interface IReadOnlyMap<TPoint, T>
    {
        T this[TPoint position] { get; }

        bool Contains(TPoint position);
        bool TryGetValue(TPoint position, out T item);
    }

    /// <summary>
    /// 游戏地图基本接口
    /// </summary>
    public interface IMap<TPoint, T>
    {
        T this[TPoint position] { get; set; }
        IEnumerable<KeyValuePair<TPoint, T>> NodePair { get; }

        void Add(TPoint position, T item);
        bool Remove(TPoint position);
        bool Contains(TPoint position);
        bool TryGetValue(TPoint position, out T item);

        void Clear();
    }


    public interface IBlockMap<TPoint, T>
    {
        T this[TPoint position] { get; }
        IEnumerable<KeyValuePair<TPoint, T>> BlocksPair { get; }
        IEnumerable<TPoint> Addresses { get; }
        IEnumerable<T> Blocks { get; }
        ShortVector2 PartitionSizes { get; }

        void Add(TPoint position, T item);
        bool Remove(TPoint position);
        bool Contains(TPoint position);
        bool TryGetValue(TPoint position, out T item);

        void Clear();

        ShortVector2 GetAddress(IntVector2 position);
        ShortVector2 GetAddress(IntVector2 position, out ShortVector2 realPosition);
        IntVector2 AddressToPosition(ShortVector2 address, ShortVector2 realPosition);
    }

}
