using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    public interface ILandformBuilding
    {
        void Build(CubicHexCoord coord, Landform landform, IWorldData data);
        void Destroy();
    }

    public class BuildingManager
    {
        public BuildingManager()
        {
        }



    }

}
