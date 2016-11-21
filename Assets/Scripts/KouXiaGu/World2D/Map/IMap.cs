using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D
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

        void Add(TPoint position, T item);
        bool Remove(TPoint position);
        bool Contains(TPoint position);
        bool TryGetValue(TPoint position, out T item);

        void Clear();
    }


    public interface IBlockMap<TPoint, T> : IMap<TPoint, T>, IEnumerable<KeyValuePair<TPoint, T>>
    {
        //IEnumerable<KeyValuePair<TPoint, T>> Blocks { get; }
        //IEnumerable<TPoint> Addresses { get; }
        //IEnumerable<T> Blocks { get; }
    }

}
