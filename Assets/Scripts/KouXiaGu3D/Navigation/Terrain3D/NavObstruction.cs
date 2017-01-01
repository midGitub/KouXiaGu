using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.Navigation;

namespace KouXiaGu.Terrain3D.Navigation
{


    public class NavObstruction : IObstructive<CubicHexCoord, NavNode>
    {
        public bool CanWalk(NavNode item)
        {
            throw new NotImplementedException();
        }

        public float GetCost(CubicHexCoord currentPoint, NavNode targetNode, CubicHexCoord destination)
        {
            throw new NotImplementedException();
        }
    }

}
