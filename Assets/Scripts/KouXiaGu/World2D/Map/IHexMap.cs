using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 游戏地图基本接口
    /// </summary>
    public interface IHexMap<TPoint, T>
    {
        T this[TPoint position] { get; set; }
        IEnumerable<KeyValuePair<TPoint, T>> NodePair { get; }

        void Add(TPoint position, T item);
        bool Remove(TPoint position);
        bool Contains(TPoint position);
        bool TryGetValue(TPoint position, out T item);

        void Clear();
    }

}
