using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 创建地形到场景;
    /// </summary>
    public class SceneCreater
    {

        public SceneCreater(IDictionary<CubicHexCoord, MapNode> data)
        {
            //Building = new BuildingCreater(data.Building);
            Landform = new LandformCreater(data);
        }

        /// <summary>
        /// 建筑物创建模块;
        /// </summary>
        public BuildingCreater Building { get; private set; }

        /// <summary>
        /// 地形创建模块;
        /// </summary>
        public LandformCreater Landform { get; private set; }

    }

}
