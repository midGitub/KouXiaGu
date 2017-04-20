using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形控制;
    /// </summary>
    public class Terrain
    {

        public static IAsyncOperation<Terrain> Initialize(IWorld world)
        {
            return new AsyncInitializer();
        }


        Terrain()
        {
        }

        public IDictionary<CubicHexCoord, MapNode> Map { get; private set; }



        class AsyncInitializer : AsyncOperation<Terrain>
        {
            public AsyncInitializer()
            {
                throw new NotImplementedException();
            }
        }

    }

}
