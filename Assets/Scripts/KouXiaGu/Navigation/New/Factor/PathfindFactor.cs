using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Navigation
{

    public interface IPathfindFactor
    {
        bool IsWalkable(CubicHexCoord position);
        bool IsWalkable(CubicHexCoord position, out int cost);
    }

    public class PathfindFactor
    {

    }
}
