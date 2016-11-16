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
    public interface IMap<TPoint, T> : IReadOnlyMap<TPoint, T>
    {
        new T this[TPoint position] { get; set; }

        void Add(TPoint position, T item);
        bool Remove(TPoint position);

        void Clear();
    }

    /// <summary>
    /// 使用块加载的地图;
    /// </summary>
    public interface IBlockMap<TPoint, T> : IMap<TPoint, T>
    {
        void UpdateBlock(IntVector2 position, bool check = true);
    }

}
