using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map
{

    public interface IDynaMap<TPoint, T> : IMap<TPoint, T>
    {
        void UpdateMapData(IntVector2 targetMapPosition, bool check = true);
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
    /// 只读的地图;
    /// </summary>
    public interface IReadOnlyMap<TPoint, T>
    {
        T this[TPoint position] { get; }
        bool IsEmpty { get; }

        bool ContainsPosition(TPoint position);
        bool TryGetValue(TPoint position, out T item);
    }

}
