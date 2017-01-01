using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.Navigation;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 寻路代价,寻路方式;
    /// </summary>
    [Serializable]
    public class Obstruction : IObstructive<CubicHexCoord, TerrainNode>
    {

        public bool CanWalk(TerrainNode item)
        {
            throw new NotImplementedException();
        }

        public float GetCost(CubicHexCoord currentPoint, TerrainNode targetNode, CubicHexCoord destination)
        {
            throw new NotImplementedException();
        }
    }

}
