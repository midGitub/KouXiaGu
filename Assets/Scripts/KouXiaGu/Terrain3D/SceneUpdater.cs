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
    public class SceneLandformUpdater : IDisposable
    {
        LandformUpdater landformUpdater;
        BuildingManager buildingUpdater;

        public IAsyncOperation Start(IWorld world)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
