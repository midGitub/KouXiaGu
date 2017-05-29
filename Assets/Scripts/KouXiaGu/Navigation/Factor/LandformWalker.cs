using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Navigation
{

    public class LandformWalker : IWalker<CubicHexCoord>
    {
        public LandformWalker(IReadOnlyDictionary<CubicHexCoord, MapNode> map, TerrainResources resources)
        {
            Map = map;
            Resources = resources;
        }

        public IReadOnlyDictionary<CubicHexCoord, MapNode> Map { get; set; }
        public TerrainResources Resources { get; set; }
        public int WalkableTagsMask { get; set; }

        public bool IsWalkable(CubicHexCoord position)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CubicHexCoord> GetNeighbors(CubicHexCoord position)
        {
            throw new NotImplementedException();
        }

        public NavigationNode<CubicHexCoord> GetNavigationNode(CubicHexCoord position, CubicHexCoord destination)
        {
            throw new NotImplementedException();
        }
    }
}
