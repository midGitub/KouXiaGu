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
            LandformBuilder = new LandformUpdater(world);
            LandformBuilder.StartUpdate();
        }

        public LandformUpdater LandformBuilder { get; private set; }
    }
}
