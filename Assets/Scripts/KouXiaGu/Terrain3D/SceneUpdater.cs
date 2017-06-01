using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形更新处置;
    /// </summary>
    public class SceneUpdater
    {
        public SceneUpdater(IWorld world)
        {
            BuildingBuilder = new BuildingBuilder(world);
            LandformBuilder = new LandformBuilder(world);
        }

        public BuildingBuilder BuildingBuilder { get; private set; }
        public LandformBuilder LandformBuilder { get; private set; }
    }
}
