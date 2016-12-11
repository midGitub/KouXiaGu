using System.Collections.Generic;

namespace KouXiaGu
{


    public interface IGrids<TCoord, TDirection>
    {
        TCoord GetDirection(TDirection direction);
        TCoord GetDirectionOffset(TDirection direction);
        IEnumerable<TCoord> GetNeighbours();
        IEnumerable<TCoord> GetNeighbours(TDirection directions);
        IEnumerable<TCoord> GetNeighboursAndSelf();
    }


}