using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 建筑物预制;
    /// </summary>
    public interface IBuildingPrefab
    {
        /// <summary>
        /// 将建筑物建立到新的位置;
        /// </summary>
        IBuilding BuildAt(IWorld world, CubicHexCoord position, float angele, BuildingInfo info);
    }
}
