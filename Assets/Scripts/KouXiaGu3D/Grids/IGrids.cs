using System.Collections.Generic;

namespace KouXiaGu
{

    /// <summary>
    /// 网格的点结构;
    /// </summary>
    public interface IGridPoint
    {
        short X { get; }
        short Y { get; }

        IEnumerable<IGridPoint> GetNeighbours();
    }

    /// <summary>
    /// 带方向的网格点;
    /// </summary>
    public interface IGridPoint<TDirection> : IGridPoint
    {
        IGridPoint GetDirection(TDirection direction);
        IGridPoint GetDirectionOffset(TDirection direction);
        IEnumerable<IGridPoint> GetNeighbours(TDirection directions);
        IEnumerable<IGridPoint> GetNeighboursAndSelf();
    }


}