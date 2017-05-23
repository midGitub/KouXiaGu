using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Navigation
{

    public class NavigationMap : INavigationMap<CubicHexCoord>
    {
        public NavigationMap(IReadOnlyDictionary<CubicHexCoord, MapNode> map, TerrainResource resource)
        {
        }

        public bool CanWalk(CubicHexCoord position)
        {
            throw new NotImplementedException();
        }

        public NodeInfo<CubicHexCoord> GetInfo(CubicHexCoord position, CubicHexCoord destination)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CubicHexCoord> GetNeighbors(CubicHexCoord position)
        {
            throw new NotImplementedException();
        }
    }
}
