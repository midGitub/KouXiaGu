using KouXiaGu.Terrain3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Resources
{

    public class WorldResources
    {
        public IDictionary<int, LandformInfo> Landform { get; internal set; }
        public IDictionary<int, BuildingInfo> Building { get; internal set; }
        public IDictionary<int, RoadInfo> Road { get; internal set; }
        public LandformTag Tags { get; internal set; }
    }
}
