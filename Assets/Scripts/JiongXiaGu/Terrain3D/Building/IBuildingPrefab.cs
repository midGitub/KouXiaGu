using JiongXiaGu.Grids;
using JiongXiaGu.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.World;
using JiongXiaGu.World.Resources;

namespace JiongXiaGu.Terrain3D
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
